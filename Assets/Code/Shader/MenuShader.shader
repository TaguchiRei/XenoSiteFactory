Shader "Custom/UIFlowLike3D"
{
    Properties
    {
        _FlowTex ("Flowing Texture", 2D) = "white" {}
        _Tiling ("Tiling", Vector) = (1,1,0,0)
        _Speed ("Scroll Speed", Vector) = (0.1, 0, 0, 0)
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0; // UI.Image のUV
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0; // マスク用
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Tiling;
                float4 _Speed;
                float4 _Color;
            CBUFFER_END

            // UI.Image の SourceImage が入る（マスク用）
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            // 流れる模様
            TEXTURE2D(_FlowTex); SAMPLER(sampler_FlowTex);

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float t = _Time.y;

                // === 3Dマテリアルと同じ計算 (Tiling + Offset) ===
                float2 flowUV = IN.uv * _Tiling.xy + _Speed.xy * t;

                half4 flowCol = SAMPLE_TEXTURE2D(_FlowTex, sampler_FlowTex, flowUV);

                // マスク用アルファ（角丸・不規則形状保持）
                half maskA = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;

                flowCol *= _Color;
                flowCol.a = maskA * _Color.a;

                return flowCol;
            }
            ENDHLSL
        }
    }
}
