Shader "Unlit/WaterSineShader"
{
    Properties
    {
        _LightColor("Light Color", Color) = (1,1,1,1)
        _DarkColor("Dark Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 worldNormal : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float4 vertexObjectPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _LightColor;
            fixed4 _DarkColor;
            float4 _LightPoint;
            
            int _WaveCount = 3;
            float _Amplitudes[3];
            float _WaveLengths[3];
            float2 _WaveDirections[3];

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
                v.vertex.xyz = waveValue;
                
                v.normal = float4(normalize(cross(binormal, tangent)), 1);
                o.worldNormal = float4(UnityObjectToWorldNormal(v.normal), 0);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertexObjectPos = v.vertex;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 lightDifference = i.worldPosition - _LightPoint;
                fixed3 lightDir = normalize(lightDifference);
                fixed intensity = -1 * dot(lightDir, i.worldNormal);
                fixed4 col = lerp(_DarkColor, _LightColor, i.vertexObjectPos.y);
                return col * intensity;
            }
            ENDCG
        }
    }
}
