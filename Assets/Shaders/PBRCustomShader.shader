Shader "Unlit/PBRCustomShader"
{
    Properties
    {
        _MainTexture ("Base Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (0,0,0,0)
        _NormalMap ("Normal", 2D) = "white" {}
        _NormalMapStrength ("Normal Strength", Range(0, 1)) = 0
        _MetallicTexture ("Metallic Map", 2D) = "white" {}
        _MetallicStrength ("Metalic Strength", Range(0, 1)) = 0
        _OcclusionTexture ("Occlusion Map", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline" = "UniversalRenderPipeline"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal@14.0.9/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION; // vertex coordinates in object-space
                float2 uv : TEXCOORD0;
                // TEXCOORD1 is where Unity defines the global illumination
                // This includes Light mapping from baked lighting
                // and spherical harmonics for the light probes
                float4 GILightingData : TEXCOORD1;
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                // Position, Normal, and View direction in world space for the lighting calculations
                float3 worldSpacePosition : TEXCOORD1;
                float3 worldSpaceNormal : TEXCOORD2;
                float3 viewDirection : TEXCOORD3;
                // Tangent and Bitangent to for the normal map
                float4 tangent : TEXCOORD4;
                float3 bitangent : TEXCOORD5;
                // This stores the light map UV (from the baked lighting) and the
                // spherical harmonics into TEXCOORD6.
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 6);
            };

            sampler2D _MainTexture, _NormalMap, _MetallicTexture, _OcclusionTexture;

            // CBUFFER_START and CBUFFER_END ensure this material is compatible with SRP
            // so it is batched.
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTexture_ST;
            half4 _BaseColor;
            float _MetallicStrength, _Smoothness, _NormalMapStrength;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz); // where the vertex will be in Homogenous space
                o.worldSpacePosition = TransformObjectToWorld(v.vertex.xyz);
                o.worldSpaceNormal = normalize(TransformObjectToWorldNormal(v.normal.xyz));
                o.tangent.xyz = TransformObjectToWorld(v.tangent.xyz);
                o.tangent.w = v.tangent.w; // maintain the UV orientation
                o.bitangent = cross(o.worldSpaceNormal, o.tangent.xyz) * o.tangent.w;
                // The viewDirection is the vector from the camera to the object.
                o.viewDirection = normalize(_WorldSpaceCameraPos - o.worldSpacePosition);
                // unity_LightmapST is the lightmap for the active scene
                OUTPUT_LIGHTMAP_UV(v.GILightingData, unity_LightmapST, o.lightmapUV);
                OUTPUT_SH(o.worldSpaceNormal, o.vertexSH);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 unpackedNormalMap = UnpackNormalScale(tex2D(_NormalMap, i.uv), _NormalMapStrength / 100);
                float3 calculatedNormal = unpackedNormalMap.r * i.tangent
                    + unpackedNormalMap.g * i.bitangent
                    + unpackedNormalMap.b * i.worldSpaceNormal;
                // InputData and SurfaceData contain the variables required for UniversalFragmentPBR
                InputData input_data = (InputData) 0;
                input_data.positionWS = i.worldSpacePosition;
                input_data.normalWS = normalize(calculatedNormal);
                input_data.viewDirectionWS = i.viewDirection;
                input_data.bakedGI = SAMPLE_GI(i.lightmapUV, i.vertexSH, i.worldSpaceNormal); // Sampling the light map data
                SurfaceData surface_data = (SurfaceData) 0;
                surface_data.albedo = tex2D(_MainTexture, i.uv) * _BaseColor;
                surface_data.metallic = tex2D(_MetallicTexture, i.uv) * _MetallicStrength;
                surface_data.smoothness = _Smoothness;
                surface_data.occlusion = tex2D(_OcclusionTexture, i.uv);
                return UniversalFragmentPBR(input_data, surface_data);
            }
            ENDHLSL
        }
    }
}
