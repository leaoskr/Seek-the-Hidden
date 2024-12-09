Shader "Unlit/heartShader"
{
 Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1) // Default no emission
        _EmissionStrength ("Emission Strength", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Pass
        {
            Cull Back
            ZTest Always    // Always passes depth testing
            ZWrite Off      // Prevents writing to the depth buffer
            Blend SrcAlpha OneMinusSrcAlpha // Transparency blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _Color;
            float4 _EmissionColor;
            float _EmissionStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                float4 texColor = tex2D(_MainTex, i.uv);

                // Calculate emission
                float4 emission = _EmissionColor * _EmissionStrength;

                // Combine base color, texture, and emission
                float4 finalColor = texColor * _Color + emission;

                // Return with alpha transparency
                return float4(finalColor.rgb, texColor.a);

            }
            ENDCG
        }
    }
}
