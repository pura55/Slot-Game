Shader "Custom/WhiteLineGlow"
{
    Properties
    {
        [PerRendererData]
        _MainTex ("Texture", 2D) = "white" {}

        [Header(Rainbow)]
        _RainbowSpeed ("Rainbow Speed", Range(0,10)) = 1
        _RainbowScale ("Rainbow Scale", Range(0,10)) = 2
        _RainbowAngle ("Rainbow Angle", Range(0,360)) = 45
        _Saturation ("Saturation", Range(0,1)) = 1
        _Brightness ("Brightness", Range(0,5)) = 1.5

        [Header(Line and Glow)]
        _LineStrength ("Line Strength", Range(0,10)) = 3
        _GlowStrength ("Glow Strength", Range(0,10)) = 3
        _GlowSize ("Glow Size", Range(0,20)) = 4
        _Threshold ("Threshold", Range(0,1)) = 0.05

        [Header(Pulse)]
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

            float _RainbowSpeed;
            float _RainbowScale;
            float _RainbowAngle;
            float _Saturation;
            float _Brightness;

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

            // 黒い線画を抽出
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

            // HSVからRGBに変換
            float3 HSVToRGB(float3 hsv)
            {
                float3 rgb = saturate(
                    abs(
                        frac(
                            hsv.x +
                            float3(0.0, 0.666667, 0.333333)
                        ) * 6.0 - 3.0
                    ) - 1.0
                );

                return hsv.z *
                    lerp(float3(1,1,1), rgb, hsv.y);
            }

            // 虹色を生成
            float3 GetRainbowColor(float2 uv)
            {
                float angle = radians(_RainbowAngle);

                float2 direction = float2(
                    cos(angle),
                    sin(angle)
                );

                float hue = frac(
                    dot(uv, direction) * _RainbowScale
                    - _Time.y * _RainbowSpeed
                );

                return HSVToRGB(
                    float3(
                        hue,
                        _Saturation,
                        _Brightness
                    )
                );
            }

            float4 frag(v2f i) : SV_Target
            {
                // 元の線画
                float lineMask = GetLineMask(i.uv);

                // 発光範囲
                float2 offset =
                    _MainTex_TexelSize.xy * _GlowSize;

                float glow = 0.0;

                // 上下左右
                glow += GetLineMask(
                    i.uv + float2(offset.x, 0));

                glow += GetLineMask(
                    i.uv - float2(offset.x, 0));

                glow += GetLineMask(
                    i.uv + float2(0, offset.y));

                glow += GetLineMask(
                    i.uv - float2(0, offset.y));

                // 斜め方向
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

                // 発光の明滅
                float pulse = 1.0 -
                    _PulseAmount *
                    (sin(_Time.y * _PulseSpeed)
                    * 0.5 + 0.5);

                // 線画と発光を合成
                float alpha = saturate(
                    lineMask * _LineStrength +
                    glow * _GlowStrength * pulse
                );

                alpha *= _Opacity;
                alpha *= i.color.a;

                // 虹色を取得
                float3 rainbow = GetRainbowColor(i.uv);

                return float4(
                    rainbow * i.color.rgb,
                    alpha
                );
            }

            ENDCG
        }
    }
}