//Shader "Custom/TransparentSingleColorShader" {
//	Properties{
//	  _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
//	}
//		SubShader
//	{
//		  Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
//		  Blend SrcAlpha OneMinusSrcAlpha
//		  Cull Off
//		  LOD 200
//
//		  CGPROGRAM
//		  #pragma surface surf Lambert
//
//		  fixed4 _Color;
//
//
//	struct Input {
//	  float2 uv_MainTex;
//	};
//
//	float4 frag(v2f_img i) : COLOR
//	{
//		//float4 original = tex2D(_MainTex, i.uv);
//		//float tmp = (original.r + original.g) * 0.5;
//		//original.r = original.g = tmp;
//		//original.
//
//		return original;
//	}
//
//	//void surf(Input IN, inout SurfaceOutput o) {
//	//	//if (_Color.r >= 1 || _Color.g >= 1)
//	//	//{
//	//	//	//o.Albedo = _Color.rgb;
//	//	//	o.Emission = _Color.rgb; // * _Color.a;
//	//	//	o.Alpha = 0;
//	//	//}
//	//	//else
//	//	{
//	//		//o.Albedo = _Color.rgb;
//	//		o.Emission = _Color.rgb ; // * _Color.a;
//	//		o.Alpha = 100;
//	//	}
//	//}
//	ENDCG
//	}
//		FallBack "Diffuse"
//}


Shader "Custom/TransparentSingleColorShader" {
Properties{
	_MainTex("Base (RGB)", RECT) = "white" {}
}

SubShader{
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;

float4 frag(v2f_img i) : COLOR
{
	float4 original = tex2D(_MainTex, i.uv);
	float tmp = (original.r + original.g) * 0.5;
	original.r = original.g = 0;
	original.a = 0;

	return original;
}
ENDCG

	}
}

Fallback off

}
