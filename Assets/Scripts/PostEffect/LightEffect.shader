Shader "Hidden/LightEffect"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightPosition("Light Position", Vector) = (0.0, 0.0, 0.0, 0.0)
		_LightRange("Light Range", Float) = 5.0
		_LightColor("Light Color", Color) = (1.0, 1.0, 1.0, 1.0)
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
			uniform fixed4 _LightPosition;
			uniform fixed4 _LightColor;
			uniform float _LightRange;

			fixed4 frag (v2f_img i) : COLOR
			{
				float dist = sqrt((i.uv[0] - _LightPosition.x)*(i.uv[0] - _LightPosition.x) + (i.uv[1] - _LightPosition.y)*(i.uv[1] - _LightPosition.y)*9.0*9.0/(16.0*16.0));
				fixed4 col = _LightColor * clamp(1.0 - dist / _LightRange, 0.0, 1.0);
				return col;
			}
			ENDCG
		}
	}
}
