Shader "Unlit/Transparent Colored Mask (AlphaClip)"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_MaskTex ("Base (RGB), Alpha (A)", 2D) = "white" {}	
		_Grey ("Grey scale", Range (0,1)) = 0			
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent-256"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MaskTex;
			half4 _MainTex_ST;
			float _Grey;
			
			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.worldPos = TRANSFORM_TEX(v.vertex.xy, _MainTex);
				return o;
			}

			half4 frag (v2f IN) : COLOR
			{
				half4 col = tex2D(_MainTex, IN.texcoord) * IN.color;
				half4 mask = tex2D(_MaskTex, IN.texcoord);
			
				float2 factor = abs(IN.worldPos);
				float val = 1.0 - max(factor.x, factor.y);
				if (val < 0.0) col = half4(0.0, 0.0, 0.0, 0.0);
				col.a = min(col.a, mask.a);
				
				if (_Grey > 0.5)
				{
					fixed grayscale = Luminance(col.rgb);
					col = half4(grayscale, grayscale, grayscale, col.a);
				}
					
				if(col.r < 0.5 &&
					col.g < 0.5 &&
					col.b < 0.5 &&
					col.a < 1)
				{
				//删除摄像机背景色
					col.a = IN.color.a;
				}
				
				return col;
			}
			ENDCG
		}
	}
	Fallback Off
}