Shader "Hidden/DayNightCycleEffect"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GradientTex("Gradient", 2D) = "white" {}

		_TOD("Black & White blend", Range(0, 1)) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _GradientTex;
			uniform float _TOD;

			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 grad = tex2D(_GradientTex, fixed2(_TOD, 0));

				col = grad * col;
				return col;
			}
			ENDCG
		}
	}
}
