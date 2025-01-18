Shader "Unlit/WaterSineShader"
{
    Properties
    {
        _LightColor("Light Color", Color) = (1,1,1,1)
        _DarkColor("Dark Color", Color) = (0,0,0,0)
        _PeakVisibility("Peak Visibility", Range(0, 4)) = 3
        _Speed("Speed", Range(0, 1)) = 0.5
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.5
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
            // Allows multiple lights to impact the water
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS

            #include "Library/PackageCache/com.unity.render-pipelines.universal@14.0.9/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 GILightingData : TEXCOORD1;
            };

            struct v2f
            {
                float3 worldNormal : TEXCOORD1;
                float3 worldPosition : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float3 viewDirection : TEXCOORD3;
                float waterDepth : TEXCOORD5;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 6);
            };
            
            int _WaveCount = 3;
            float _Amplitudes[3];
            float _WaveLengths[3];
            float2 _WaveDirections[3];

            float4 _LightPoint; // TODO: Remove
            half4 _LightColor, _DarkColor;
            float _PeakVisibility, _Smoothness, _Metallic, _Speed;
            
            float3 addWave(float3 vertex, float amplitude, float2 waveDirection, float waveLength, inout float4 tangent, inout float4 binormal)
            {
                float k = (2 * PI) / waveLength;
                float w = sqrt(9.81 / k);
                float peak = (amplitude / k) * _PeakVisibility;
                float waveDirSpeed = k * (dot(normalize(waveDirection), vertex.xz) - (w * _Time.y)) * _Speed;
                float y = peak * sin(waveDirSpeed);
                float x = peak * cos(waveDirSpeed);
                float z = peak * cos(waveDirSpeed);

                float3 waveDerivative = float3(
                    peak * cos(waveDirSpeed),
                    peak * sin(waveDirSpeed),
                    peak * cos(waveDirSpeed)
                );

                tangent.xyz += waveDerivative;
                binormal.xyz += waveDerivative;
                return float3(x, y, z);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 tangent = float4(1, 0, 0, v.tangent.w);
			    float4 binormal = float4(0, 0, 1, v.tangent.w);
                
                o.worldPosition = TransformObjectToWorld(v.vertex);
                o.waterDepth = 0;
                for (int i = 0; i < _WaveCount; i++)
                {
                  o.waterDepth += addWave(v.vertex, _Amplitudes[i], _WaveDirections[i], _WaveLengths[i], tangent, binormal);
                }
                v.vertex.xyz += o.waterDepth;
                
                v.normal = float4(normalize(cross(binormal, tangent)), 1);
                o.worldNormal = normalize(TransformObjectToWorldNormal(v.normal.xyz));
                
                o.vertex = TransformObjectToHClip(v.vertex);
                o.viewDirection = normalize(GetWorldSpaceViewDir(o.worldPosition));
                OUTPUT_LIGHTMAP_UV(v.GILightingData, unity_LightmapST, o.lightmapUV);
                OUTPUT_SH(o.worldNormal, o.vertexSH);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Setting the color based on the Visibility and Saturation
                half4 col = lerp(_LightColor, _DarkColor, saturate(i.waterDepth));

                InputData input_data = (InputData) 0;
                input_data.positionWS = i.worldPosition;
                input_data.normalWS = normalize(i.worldNormal);
                input_data.viewDirectionWS = i.viewDirection;
                // Sampling the light map data
                input_data.bakedGI = SAMPLE_GI(i.lightmapUV, i.vertexSH, i.worldNormal);
                SurfaceData surface_data = (SurfaceData) 0;
                surface_data.albedo = col;
                surface_data.metallic = _Metallic;
                surface_data.smoothness = _Smoothness;
                surface_data.occlusion = 1;
                surface_data.alpha = col.a;
                
                return UniversalFragmentPBR(input_data, surface_data);
            }
            ENDHLSL
        }
    }
}
