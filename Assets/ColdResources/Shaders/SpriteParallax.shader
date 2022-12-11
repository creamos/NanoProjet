Shader "Custom/SpriteParallax"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Depth ("Depth", Range(-1.0, 1.0)) = 0
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" "Queue"="Transparent-1" "PreviewType"="Plane"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Depth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half2 _uv = mul(unity_ObjectToWorld,i.uv) + (_WorldSpaceCameraPos * _Depth);
                half4 col = tex2D(_MainTex, _uv) * i.color;
                return col;
            }
            ENDHLSL
        }
    }
}
