Shader "Unlit/VelvetShader"
{
    Properties
    {
        _MainTexture ("Base Texture", 2D) = "white" {}
        _Color ("Diffuse Color", Color) = (1,1,1,1)
        _Smoothness("Smoothness",Range(0,1)) = 1
        _MetallicTexture ("Metallic Map", 2D) = "white" {}
        _Metallic("Metalness",Range(0,1)) = 0
        _NormalMap ("Normal", 2D) = "white" {}
        _NormalMapStrength ("Normal Strength", Range(0, 1)) = 0
        _OcclusionTexture ("Occlusion Map", 2D) = "white" {}
        _IndexOfRefraction ("IOR", Range(1.5, 20)) = 1.54
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            sampler2D _MainTexture, _NormalMap, _MetallicTexture, _OcclusionTexture;

            // CBUFFER_START and CBUFFER_END ensure this material is compatible with SRP
            // so it is batched.
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTexture_ST;
            half4 _Color;
            float _Smoothness, _Metallic, _NormalMapStrength, _IndexOfRefraction;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION; // vertex coordinates in object-space
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldSpaceNormal : TEXCOORD1;
                float3 worldSpacePosition : TEXCOORD2;
                float4 tangent : TEXCOORD3;
                float3 bitangent : TEXCOORD4;
                float3 viewDirection : TEXCOORD5;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz); // where the vertex will be in Homogenous space
                o.worldSpacePosition = TransformObjectToWorld(v.vertex.xyz);
                o.worldSpaceNormal = normalize(TransformObjectToWorldNormal(v.normal.xyz));
                o.tangent.xyz = TransformObjectToWorld(v.tangent.xyz);
                o.tangent.w = v.tangent.w; // maintain the UV orientation
                o.bitangent = cross(o.worldSpaceNormal, o.tangent.xyz) * o.tangent.w;
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                // The viewDirection is the vector from the camera to the object.
                o.viewDirection = normalize(_WorldSpaceCameraPos - o.worldSpacePosition);
                return o;
            }

            // Based on Ashikhmin (2000): A microfacet-based BRDF generator
            // Implementation derived from https://knarkowicz.wordpress.com/2018/01/04/cloth-shading/
            float AshikhminD(float roughness, float NdotH)
            {
                float cos2h = NdotH * NdotH;
                float sin2h = 1.0 - cos2h;
                float sin4h = sin2h * sin2h;
                return (sin4h + 4.0 * exp(-cos2h / (sin2h * roughness))) / (PI * (1.0 + 4.0 * roughness) * sin4h);
            }

            // Based on Majercik (2021): The Schlick Fresnel Approximation
            float3 SchlickFresnelFunction(float lDotH) {
                float f0 = pow((1-_IndexOfRefraction) / (1 + _IndexOfRefraction), 2);
                return f0 + (1 - f0) * pow(saturate(1 - lDotH), 5);
            }

            // This method is derived from a Unity provided function:
            // https://docs.unity3d.com/Manual/urp/use-built-in-shader-methods-indirect-lighting.html
            float3 CustomLightingAmbient(float3 BaseColor, float3 NormalWS, float Metallic, float Smoothness,
                            float AmbientOcclusion, float PositionWS, float2 screenspaceUV, float3 ViewDirWS)
            {
                float3 DiffuseAmbient = GlossyEnvironmentReflection(NormalWS, PositionWS,
                    1, 1, screenspaceUV);
                DiffuseAmbient *= lerp(BaseColor, float3(0,0,0), Metallic);
                float3 ReflectionVector = reflect(-ViewDirWS, NormalWS);
                float3 SpecularAmbient = GlossyEnvironmentReflection(ReflectionVector, PositionWS,
                    1 - Smoothness, 1, screenspaceUV);
                return (DiffuseAmbient + SpecularAmbient) * AmbientOcclusion;
            }

            
            half4 frag (v2f i) : SV_Target
            {
                // Unpack the normal map and apply it to the vertex based on the normal map strength
                float3 unpackedNormalMap = UnpackNormalScale(tex2D(_NormalMap, i.uv), _NormalMapStrength / 100);
                float3 calculatedNormal = unpackedNormalMap.r * i.tangent
                    + unpackedNormalMap.g * i.bitangent
                    + unpackedNormalMap.b * i.worldSpaceNormal;
                // Get the Albedo color from the texture, taking into account the color tint
                half4 albedo = tex2D(_MainTexture, i.uv) * _Color;
                // Get the Metallic amount from the texture
                float metallic = tex2D(_MetallicTexture, i.uv) * _Metallic;
                // Calculate the roughness value from the smoothness via equation 21 in Mikkelsen (2009) 
                float roughness = sqrt(2 / (_Smoothness + 2));
                // Caluclate the lighting from the main light's perspective
                Light mainLight = GetMainLight();
                float3 directLight = LightingLambert(mainLight.color, mainLight.direction, calculatedNormal.xyz);
                // Common dot products used in the implementations
                // half vector between the light and view directions
                float3 halfDirection = normalize(i.viewDirection + mainLight.direction);
                float NdotH = saturate(dot(calculatedNormal, halfDirection));
                float NdotL = saturate(dot(calculatedNormal, mainLight.direction));
                float NdotV = saturate(dot(calculatedNormal, i.viewDirection));
                float LdotH = saturate(dot(mainLight.direction, halfDirection));
                // Calculate the Fresnel component via Schlick's approximation
                float3 FresnelFunction = SchlickFresnelFunction(LdotH);
                // Calculate the Normal Distribution via Ashikhmin's calculation,
                // taking into account the base color.
                float3 NormalDistribution = albedo.rbg + AshikhminD(roughness, NdotH) * albedo.rgb;
                // Combining the components to make the BRDF, using the denominator proposed
                // in Karis (2013): Real Shading in Unreal Engine 4 (SIGGRAPH 2013 Course Notes)
                float3 BRDF = NdotL * (NormalDistribution * FresnelFunction) / (4.0 * (NdotL + NdotV - NdotL * NdotV));
                // Calculating the lighting for the remaining lights
                int numOfLights = GetAdditionalLightsCount();
                for (int index = 0; index < numOfLights; index++)
                {
                    Light currentLight = GetAdditionalLight(index, i.worldSpacePosition);
                    float3 lightDirection = currentLight.direction;
                    halfDirection = normalize(i.viewDirection + lightDirection); 
                    NdotH = saturate(dot(calculatedNormal, halfDirection));
                    NdotL = saturate(dot(calculatedNormal, lightDirection));
                    LdotH = saturate(dot(lightDirection, halfDirection));
                    FresnelFunction +=  SchlickFresnelFunction(LdotH);
                    NormalDistribution += AshikhminD(roughness, NdotH) * albedo.rgb;
                    BRDF += NdotL * currentLight.distanceAttenuation * ((NormalDistribution * FresnelFunction)
                        / (4.0 * (NdotL + NdotV - NdotL * NdotV)));
                    directLight += LightingLambert(currentLight.color, currentLight.direction, calculatedNormal.xyz);
                }
                // Calculate the ambient lighting via Unity's Builtin function
                float3 indirectLight = CustomLightingAmbient(albedo, calculatedNormal, metallic, _Smoothness,
                    tex2D(_OcclusionTexture, i.uv), i.worldSpacePosition, i.uv, i.viewDirection);
                // Combine BRDF, Direct, and indirect lighting to get the final color
                return _Color * PI * (float4(BRDF, 1) * (float4(directLight, 1) + float4(indirectLight, 1)));
            }
            ENDHLSL
        }
    }
}
