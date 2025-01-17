// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture Splatting"
{

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset] _Texture1 ("Texture 1", 2D) = "white" {}
        [NoScaleOffset] _Texture2 ("Texture 2", 2D) = "white" {}
        [NoScaleOffset] _Texture3 ("Texture 3", 2D) = "white" {}
        [NoScaleOffset] _Texture4 ("Texture 4", 2D) = "white" {}
    }
    SubShader
    {
        Tags {
				"LightMode" = "ForwardBase"
			}
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            #include "UnityCG.cginc"


            sampler2D _MainTex;
            sampler2D _Texture1;
            sampler2D _Texture2, _Texture3, _Texture4;
            float4 _MainTex_ST;

            struct Interpolators
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uvSplat : TEXCOORD1;
            };

            struct VertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.uvSplat = v.uv;
                return i;
            }

            float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                float4 splat = tex2D(_MainTex, i.uvSplat);
                return float4(1,0.6,0,1);
                return
                    tex2D(_Texture1, i.uv ) * splat.r +
                    tex2D(_Texture2, i.uv) * splat.g +
                    tex2D(_Texture3, i.uv) * splat.b +
                    tex2D(_Texture4, i.uv) * (1 - splat.r -  splat.g -  splat.b);
            }
            ENDCG
        }
    }
}