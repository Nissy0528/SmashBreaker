// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Player"
{
	Properties
	{
		_MainTex("Texture",2D) = "white"{}
		_Color("Color",Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent"
					"Queue" = "Transparent"
					"IgnoreProjector" = "True"
				 }
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull off

			//影を描画する処理
				Pass
			{
				Name "ShadowCaster"
				Tags{ "LightMode" = "ShadowCaster" }

				Fog{ Mode Off }
				ZWrite On ZTest LEqual Cull Off
				Offset 1, 1

				CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_shadowcaster
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"

				struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
					return o;
			}

			float4 frag(v2f i) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
				ENDCG
			}


			//ステンシルバッファが１の部分を黒く塗りつぶす
			Pass
			{
				Stencil
				{
					Ref 1
					Comp Equal
				}

				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;

				fixed4 frag(v2f_img i) :SV_Target
				{
					float alpha = tex2D(_MainTex,i.uv).a - 0.5;
					fixed4 col = fixed4(0, 0, 0, alpha);
					return col;
				}
					ENDCG
			}

				//ステンシルバッファが０の部分は通常描画
				Pass
				{
					Stencil
					{
						Ref 0
						Comp Equal
					}

					CGPROGRAM
					#pragma vertex vert_img
					#pragma fragment frag
					#include "UnityCG.cginc"

					sampler2D _MainTex;
					fixed4 _Color;

					fixed4 frag(v2f_img i) :SV_Target
					{
						fixed4 c = tex2D(_MainTex,i.uv);
					float alpha = tex2D(_MainTex, i.uv).a * _Color.a;
					fixed4 col = fixed4(c.r, c.g, c.b, alpha);
						return col;
					}
					ENDCG
				}
		}
}


