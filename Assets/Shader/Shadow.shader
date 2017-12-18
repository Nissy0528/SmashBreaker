// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadow"
{
	Properties
	{
		_MainTex("Texture",2D) = "white"{}
	}

	Pass
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
	}

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase

#include "UnityCG.cginc"
#include "AutoLight.cginc"

		uniform fixed4 _LightColor0;

	struct v2f
	{
		float4 pos      : SV_POSITION;
		float3 lightDir : TEXCOORD0;
		float3 normal   : TEXCOORD1;
		LIGHTING_COORDS(3, 4)
	};

	v2f vert(appdata_base v)
	{
		v2f o;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
		o.normal = normalize(v.normal).xyz;

		TRANSFER_VERTEX_TO_FRAGMENT(o);
		TRANSFER_SHADOW(o);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target{
		fixed atten = LIGHT_ATTENUATION(i);
	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT;
	fixed4 lightColor = _LightColor0 * saturate(dot(i.lightDir, i.normal));

	fixed4 c = fixed4(1, 0, 0, 1);
	c.rgb = (c * lightColor * atten) + ambient;
	return c;
	}
		ENDCG
	}
}