
Shader "Custom/RainbowEnergy"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
        _Speed ("Flow Speed", Range(0,5)) = 0.8
        _ColorSpeed ("Color Speed", Range(0,3)) = 0.2
        _Scale ("Energy Scale", Range(1,15)) = 4
        _Brightness ("Brightness", Range(0,3)) = 1.4
        _Contrast ("Contrast", Range(0.5,5)) = 2.2
        _Glow ("White Glow", Range(0,2)) = 0.7
        _Darkness ("Darkness", Range(0,1)) = 0.35
        _CenterGlow ("Center Glow", Range(0,2)) = 0.8
        _Distortion ("Distortion", Range(0,3)) = 1.2
        _Aspect ("Aspect Ratio", Float) = 1.7778
        _BlurStrength ("Blur Strength", Range(0,0.1)) = 0.02
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)

                float _FadeAlpha;
                float _Speed;
                float _ColorSpeed;
                float _Scale;
                float _Brightness;
                float _Contrast;
                float _Glow;
                float _Darkness;
                float _CenterGlow;
                float _Distortion;
                float _Aspect;
                float _BlurStrength;

            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.positionCS =
                    TransformObjectToHClip(input.positionOS.xyz);

                output.uv = input.uv;
                output.color = input.color;

                return output;
            }

            // ランダム値
            float hash(float2 p)
            {
                return frac(
                    sin(dot(p, float2(127.1, 311.7)))
                    * 43758.5453
                );
            }

            // 滑らかなノイズ
            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(
                    lerp(hash(i),
                         hash(i + float2(1,0)), u.x),

                    lerp(hash(i + float2(0,1)),
                         hash(i + float2(1,1)), u.x),

                    u.y
                );
            }

            // 複数のノイズを重ねる
            float fbm(float2 p)
            {
                float value = 0;
                float amplitude = 0.5;

                for (int i = 0; i < 5; i++)
                {
                    value += amplitude * noise(p);

                    p = p * 2.03 + float2(7.3, 4.1);
                    amplitude *= 0.5;
                }

                return value;
            }

            // 虹色
            float3 rainbow(float h)
            {
                return saturate(
                    abs(frac(h + float3(
                        0.0, 0.666667, 0.333333
                    )) * 6.0 - 3.0) - 1.0
                );
            }

            half4 EnergyColor(Varyings input)
            {
                float2 p = input.uv - 0.5;

                p.x *= _Aspect;

                float radius = length(p);

                float2 direction =
                    p / max(radius, 0.08);

                float t = _Time.y * _Speed;

               float2 flow =
    p * _Scale - direction * (12.0 + sin(t) * 0.5);

                // ノイズを使って形を歪ませる
                float2 warp;

                warp.x = fbm(
                    flow + float2(0, t * 0.15)
                );

                warp.y = fbm(
                    flow + float2(5.2, -t * 0.12)
                );

                float2 distorted =
                    flow + (warp - 0.5) * _Distortion;

                float energy = fbm(distorted);

                // 光の濃淡
                float lightValue = saturate(
                    (energy - _Darkness) * _Contrast
                );

                // 虹色の生成
                float hue =
                    p.x * 0.35
                    + p.y * 0.25
                    + warp.x * 0.8
                    + energy * 0.35
                    - _Time.y * _ColorSpeed;

                float3 color = rainbow(hue);

                color *= lightValue * _Brightness;

                // 白い発光
                float whiteGlow =
                    smoothstep(0.55, 0.85, energy)
                    * _Glow;

                color += whiteGlow;

                // 中心の発光
                float center =
                    exp(-radius * radius * 35.0)
                    * _CenterGlow;

                color += center;

                return half4(
                    color * input.color.rgb,
                    _FadeAlpha * input.color.a
                );
            }

            half4 frag(Varyings input) : SV_Target
{
    half4 result = EnergyColor(input) * 4.0;

    float2 originalUV = input.uv;

    float2 blur = float2(
        _BlurStrength / max(_Aspect, 0.001),
        _BlurStrength
    );

    input.uv = originalUV + float2(blur.x, 0);
    result += EnergyColor(input) * 2.0;

    input.uv = originalUV - float2(blur.x, 0);
    result += EnergyColor(input) * 2.0;

    input.uv = originalUV + float2(0, blur.y);
    result += EnergyColor(input) * 2.0;

    input.uv = originalUV - float2(0, blur.y);
    result += EnergyColor(input) * 2.0;

    input.uv = originalUV + blur;
    result += EnergyColor(input);

    input.uv = originalUV - blur;
    result += EnergyColor(input);

    input.uv = originalUV + float2(blur.x, -blur.y);
    result += EnergyColor(input);

    input.uv = originalUV + float2(-blur.x, blur.y);
    result += EnergyColor(input);

    return result / 16.0;
}

            ENDHLSL
        }
    }
}
