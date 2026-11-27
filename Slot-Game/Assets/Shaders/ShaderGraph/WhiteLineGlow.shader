Shader "Custom/WhiteLineGlow"
{
    Properties
    {
        [PerRendererData]
        _MainTex ("Texture", 2D) = "white" {}

        _LineColor ("Line Color", Color) = (1,1,1,1)
        _LineStrength ("Line Strength", Range(0,10)) = 3
        _GlowStrength ("Glow Strength", Range(0,10)) = 3
        _GlowSize ("Glow Size", Range(0,20)) = 4
        _Threshold ("Threshold", Range(0,1)) = 0.05
        _PulseSpeed ("Pulse Speed", Range(0,10)) = 2
        _PulseAmount ("Pulse Amount", Range(0,1)) = 0.2
        _Opacity ("Opacity", Range(0,1)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "False"
        }

        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float4 _LineColor;

            float _LineStrength;
            float _GlowStrength;
            float _GlowSize;
            float _Threshold;
            float _PulseSpeed;
            float _PulseAmount;
            float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }

            // çïÇ¢ê¸âÊÇíäèo
            float GetLineMask(float2 uv)
            {
                float4 col = tex2D(_MainTex, uv);

                float gray = dot(
                    col.rgb,
                    float3(0.299, 0.587, 0.114)
                );

                float maskValue = 1.0 - gray;

                return saturate(
                    (maskValue - _Threshold) /
                    max(1.0 - _Threshold, 0.001)
                ) * col.a;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // å≥ÇÃê¸âÊ
                float lineMask = GetLineMask(i.uv);

                // î≠åıîÕàÕ
                float2 offset =
                    _MainTex_TexelSize.xy * _GlowSize;

                float glow = 0.0;

                // è„â∫ç∂âE
                glow += GetLineMask(
                    i.uv + float2(offset.x, 0));

                glow += GetLineMask(
                    i.uv - float2(offset.x, 0));

                glow += GetLineMask(
                    i.uv + float2(0, offset.y));

                glow += GetLineMask(
                    i.uv - float2(0, offset.y));

                // éŒÇﬂï˚å¸
                float2 diagonal = offset * 0.7071;

                glow += GetLineMask(
                    i.uv + diagonal);

                glow += GetLineMask(
                    i.uv - diagonal);

                glow += GetLineMask(
                    i.uv + float2(
                        diagonal.x, -diagonal.y));

                glow += GetLineMask(
                    i.uv + float2(
                        -diagonal.x, diagonal.y));

                glow /= 8.0;

                // î≠åıÇÃñæñ≈
                float pulse = 1.0 -
                    _PulseAmount *
                    (sin(_Time.y * _PulseSpeed)
                    * 0.5 + 0.5);

                // ê¸âÊÇ∆î≠åıÇçáê¨
                float alpha = saturate(
                    lineMask * _LineStrength +
                    glow * _GlowStrength * pulse
                );

                alpha *= _Opacity;
                alpha *= _LineColor.a;
                alpha *= i.color.a;

                return fixed4(
                    _LineColor.rgb * i.color.rgb,
                    alpha
                );
            }

            ENDCG
        }
    }
}