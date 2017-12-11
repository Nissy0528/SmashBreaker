Shader "Unlit/Stage"
{
	Properties
	{
		_MainTex("Texture",2D) = "white"{}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull off

		//ステンシルバッファを0で塗りつぶす
		Pass
	{
		Stencil
	{
		Ref 0
		Comp Always
		Pass Replace
	}

		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		sampler2D _MainTex;

	fixed4 frag(v2f_img i) :COLOR
	{
		fixed4 col = tex2D(_MainTex,i.uv);
	return col;
	}
		ENDCG
	}
	}
}
