Shader "Unlit/SetPaint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", color) = (0,0,0,0)
        _PaintPos ("PaintPos", Vector) = (0,0,0) 
        _Radius ("Radius", float) = 1
        _Hardness ("Hardness", float) = 1
        _Strength ("Strength", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct meshdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct interpolated
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _PaintPos;
            float _Radius;
            float _Hardness;
            float _Strength;
            float4 _Color;

            float mask(float3 position, float3 center, float radius, float hardness)
            {
                float m = distance(center, position);
                //return 1 - m;
                return 1 - smoothstep(radius * hardness, radius, m);
            }

            interpolated vert (meshdata v)
            {
                interpolated o;
                float4 uv = float4(0, 0, 0, 1);
                uv.xy = float2(1, _ProjectionParams.x) * (v.uv.xy * float2( 2, 2) - float2(1, 1));
                o.vertex = uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (interpolated i) : SV_Target
            {
                float m = mask(i.worldPos, _PaintPos, _Radius, _Hardness);
                float edge = m * _Strength;
                
                return _Color * edge;
            }
            ENDCG
        }
    }
}
