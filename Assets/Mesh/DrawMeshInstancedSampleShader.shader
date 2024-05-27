Shader "Sample/DrawMeshInstancedSampleShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                uint instancedId : SV_InstanceID;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            // C#側から座標情報が渡される
            StructuredBuffer<float3> _Positions;
            StructuredBuffer<float3> _Normal;

            // テクスチャ
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // ライティングのための定数
            float3 _WorldSpaceLightPos0;
            float4 _LightColor0;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 positionOS = IN.positionOS.xyz + _Positions[IN.instancedId];
                OUT.vertex = TransformWorldToHClip(positionOS);
                OUT.uv = IN.uv;
                OUT.worldPos = TransformObjectToWorld(positionOS);
                OUT.normalWS = TransformObjectToWorldNormal(float3(0,0,1)); //_Normal[IN.instancedId]
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // テクスチャカラー
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // 環境光
                half3 ambient = half3(0.1, 0.1, 0.1);

                // 簡易的なライティング計算
                half3 normal = normalize(IN.normalWS);
                half3 viewDir = normalize(_WorldSpaceCameraPos - IN.worldPos);
                half3 lightDir = normalize(_WorldSpaceLightPos0 - IN.worldPos);
                half3 lightColor = _LightColor0.rgb;

                // ディフューズシェーディング
                half3 diffuse = max(0.0, dot(normal, lightDir)) * lightColor;

                // スペキュラーシェーディング（簡易的な反射モデル）
                half3 reflectDir = reflect(-lightDir, normal);
                half3 specular = pow(max(0.0, dot(viewDir, reflectDir)), 16.0) * lightColor;

                // 最終的なライティング計算
                half3 lighting = ambient + diffuse + specular;

                // 最終カラー
                half4 finalColor = half4(texColor.rgb * lighting, texColor.a);

                return finalColor;
            }
            ENDHLSL
        }
    }
}