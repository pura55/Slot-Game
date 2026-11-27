Shader "Custom/SlashGlow"
{
    Properties
    {
        _BladeColor ("Blade Color", Color) =
            (0.4, 0.75, 1, 1)

        _MetalPower ("Metal Power", Range(0, 1)) = 1

        _FlowSpeed ("Flow Speed", Float) = 0.8

        _FlowWidth ("Flow Width", Range(0.01, 0.5)) = 0.08

        _FlowPower ("Flow Power", Range(0, 2)) = 0.6
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            float4 _BladeColor;
            float _MetalPower;
            float _FlowSpeed;
            float _FlowWidth;
            float _FlowPower;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex =
                    UnityObjectToClipPos(v.vertex);

                o.color = v.color;
                o.uv = v.uv;

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float y = i.uv.y;

                // ‹à‘®‚Ì”Z’W
                float metal =
                    abs(sin(y * 12.56637));

                float3 darkColor =
                    _BladeColor.rgb * 0.25;

                float3 lightColor =
                    lerp(_BladeColor.rgb, 1.0, 0.8);

                float3 col =
                    lerp(darkColor, lightColor, metal);

                col = lerp(
                    _BladeColor.rgb,
                    col,
                    _MetalPower);

                // ‰¡‚É—¬‚ê‚éŒõ
                float flowPos =
                    frac(i.uv.x - _Time.y * _FlowSpeed);

                float flow =
                    1.0 - smoothstep(
                        0.0,
                        _FlowWidth,
                        abs(flowPos - 0.5));

                col += flow * _FlowPower;

                col *= i.color.rgb;

                return float4(col, i.color.a);
            }

            ENDCG
        }
    }
}