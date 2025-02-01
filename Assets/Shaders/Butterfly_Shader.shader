Shader "Unlit/Butterfly_Shader"
{
    Properties
    {
        _MainTexture ("Base Texture", 2D) = "white" {}
        [HDR] _BaseColor ("Color", Color) = (0,0,0,1)
        _FresnelStrength ("Fresnel Strength", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline" = "UniversalRenderPipeline"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

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

            sampler2D _MainTexture;

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTexture_ST;
            half4 _BaseColor;
            float _FresnelStrength;
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
                float fresnel = saturate(pow(1 - dot(i.worldSpaceNormal, i.viewDirection), _FresnelStrength));
                // InputData and SurfaceData contain the variables required for UniversalFragmentPBR
                InputData input_data = (InputData) 0;
                input_data.positionWS = i.worldSpacePosition;
                input_data.normalWS = normalize(i.worldSpaceNormal);
                input_data.viewDirectionWS = i.viewDirection;
                input_data.bakedGI = SAMPLE_GI(i.lightmapUV, i.vertexSH, i.worldSpaceNormal); // Sampling the light map data
                SurfaceData surface_data = (SurfaceData) 0;
                surface_data.albedo = tex2D(_MainTexture, i.uv);
                surface_data.metallic = 0;
                surface_data.smoothness = 1;
                surface_data.occlusion = 1;
                surface_data.alpha = _BaseColor.a;
                surface_data.emission = fresnel * _BaseColor;
                return UniversalFragmentPBR(input_data, surface_data);
            }
            ENDHLSL
        }
    }
}
