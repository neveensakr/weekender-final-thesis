Shader "Unlit/WaterSineShader"
{
    Properties
    {
        _LightColor("Light Color", Color) = (1,1,1,1)
        _DarkColor("Dark Color", Color) = (0,0,0,0)
        _RefractionColor("Refraction Color", Color) = (0,0,0,0)
        _Visibility("Visibility", Range(0, 1)) = 0.5
        _Shininess("Shininess", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 worldNormal : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float3 viewDirection : TEXCOORD4;
                float accumulatedWater : TEXCOORD6;
                float waterDepth : TEXCOORD7;
            };
            
            int _WaveCount = 3;
            float _Amplitudes[3];
            float _WaveLengths[3];
            float2 _WaveDirections[3];

            float4 _LightPoint;
            fixed4 _LightColor;
            fixed4 _DarkColor;
            fixed4 _RefractionColor;
            float _Visibility;
            float _Shininess;

            float3 addWave(float4 vertex, float amplitude, float2 waveDirection, float waveLength, inout float3 tangent, inout float3 binormal)
            {
                float k = (2 * UNITY_PI) / waveLength;
                float w = sqrt(9.81 / k);
                float peak = amplitude / k;
                float waveDirSpeed = k * (dot(normalize(waveDirection), vertex.xz) - (w * _Time.y));
                float y = peak * sin(waveDirSpeed);
                float x = peak * cos(waveDirSpeed);
                float z = peak * cos(waveDirSpeed);

                float3 waveDerivative = float3(
                    peak * cos(waveDirSpeed),
                    peak * sin(waveDirSpeed),
                    peak * cos(waveDirSpeed)
                );

                tangent += waveDerivative;
                binormal += waveDerivative;
                return float3(x, y, z);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float3 waveValue = v.vertex.xyz;
                float3 tangent = float3(1, 0, 0);
			    float3 binormal = float3(0, 0, 1);
                
                for (int i = 0; i < _WaveCount; i++)
                {
                  waveValue += addWave(v.vertex, _Amplitudes[i], _WaveDirections[i], _WaveLengths[i], tangent, binormal);
                }
                o.accumulatedWater = length(v.vertex - waveValue);
                o.waterDepth = v.vertex.y - waveValue.y;
                v.vertex.xyz = waveValue;
                
                v.normal = float4(normalize(cross(binormal, tangent)), 1);
                o.worldNormal = float4(UnityObjectToWorldNormal(v.normal), 0);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewDirection = normalize(UnityWorldSpaceViewDir(o.worldPosition));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 lightDifference = i.worldPosition - _LightPoint;
                fixed3 lightDir = normalize(lightDifference);
                fixed intensity = -1 * dot(lightDir, i.worldNormal);
                // Angle between the view direction and the world normal
                float a = acos(dot(i.viewDirection, i.worldNormal) / (length(i.viewDirection) * length(i.worldNormal)));
                // Fresnel based on the Schlick Approximation
                float r0 = 0.02040466482;
                float fresnel = r0 + (1 - r0) * pow(1 - cos(a), 5);
                // Specular Calculation based on https://www.gamedev.net/articles/programming/graphics/rendering-water-as-a-post-process-effect-r2642/
                half3 eyeDir = (2 * dot(i.viewDirection, i.worldNormal) * i.worldNormal - i.viewDirection);
                half sepcularDot = saturate(dot(eyeDir.xyz, -lightDir) * 0.5 + 0.5);
                float specular = (1.0 - fresnel) * saturate(-lightDir.y) * ((pow(sepcularDot, 512.0)) * (_Shininess * 1.8 + 0.2));
                specular += specular * 25 * saturate(_Shininess - 0.05);
                // Setting the color based on the Visibility and Saturation
                fixed4 surfaceColor = lerp(_RefractionColor, _LightColor, saturate(i.accumulatedWater / _Visibility));
                fixed3 col = lerp(surfaceColor, _DarkColor, saturate(i.waterDepth / fixed3(4.5, 75, 300)));
                return fixed4(col, surfaceColor.a) * intensity + specular;
            }
            ENDCG
        }
    }
}
