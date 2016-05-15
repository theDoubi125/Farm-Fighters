Shader "Hidden/DayNightCycleEffect"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap("LightMap", 2D) = "black" {}
		_Color("SkyColor", Color) = (1.0, 1.0, 1.0, 1.0)
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
			uniform sampler2D _LightMap;
			uniform fixed4 _Color;

			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 light = tex2D(_LightMap, i.uv);

				col = clamp(_Color*col + col*light, 0.0, 1.0);
				return col;
			}
			ENDCG
		}
	}
}
