Shader "Unlit/EdgeGlow_frag"
{
	Properties
	{
		_Color ("Color", Color) = (0,0,0,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_HighlightColor ("Highlight", Color) = (1,1,1,1)
		_HighlightPower ("Highlight Power", Range(0,1)) = 0
		_PulseFactor ("Pulse Factor", Range(0,1)) = 1
		_TileFactor ("Tile Factor", float) = 1
		_TexScaleX ("Texture Scale (X)", float) = 1
		_TexScaleY ("Texture Scale (Y)", float) = 1
		_TexScaleZ ("Texture Scale (Z)", float) = 1
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 norm : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 norm : NORMAL;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _HighlightColor;
			float _HighlightPower;
			float _PulseFactor;
			float4 _MainTex_ST;
			float _TileFactor;
			float _TexScaleX;
			float _TexScaleY;
			float _TexScaleZ;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.norm = v.norm;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float minimumX = _HighlightPower;
				float maximumX = 1 - _HighlightPower; 
				float minimumY = _HighlightPower;
				float maximumY = 1 - _HighlightPower; 

				float2 tc = i.uv;

				// Scale texture size (per face)
				/*if (i.norm.x == 1 || i.norm.x == -1) {
					tc.x *= _TexScaleZ / _TileFactor;
					tc.y *= _TexScaleY / _TileFactor;

					minimumX = _HighlightPower / _TexScaleZ;
					maximumX = 1 - _HighlightPower / _TexScaleZ; 
					minimumY = _HighlightPower / _TexScaleY;
					maximumY = 1 - _HighlightPower / _TexScaleY; 
				}

				else if (i.norm.y == 1 || i.norm.y == -1) {
					tc.x *= _TexScaleX / _TileFactor;
					tc.y *= _TexScaleZ / _TileFactor;

					minimumX = _HighlightPower / _TexScaleX;
					maximumX = 1 - _HighlightPower / _TexScaleX; 
					minimumY = _HighlightPower / _TexScaleZ;
					maximumY = 1 - _HighlightPower / _TexScaleZ; 
				}

				else if (i.norm.z == 1 || i.norm.z == -1) {
					tc.x *= _TexScaleX / _TileFactor;
					tc.y *= _TexScaleY / _TileFactor;

					minimumX = _HighlightPower / _TexScaleX;
					maximumX = 1 - _HighlightPower / _TexScaleX; 
					minimumY = _HighlightPower / _TexScaleY;
					maximumY = 1 - _HighlightPower / _TexScaleY; 
				}*/

				tc *= _TileFactor;

				// Set color. Takes pulse into account
				fixed4 col = tex2D(_MainTex, tc) * (_Color * (_PulseFactor + 0.15));

				// Set glow around edges of object. Takes pulse into account
				if(i.uv.x < minimumX || i.uv.x > maximumX)
					col = _HighlightColor * _PulseFactor;

				if(i.uv.y < minimumY || i.uv.y > maximumY)
					col = _HighlightColor * _PulseFactor;

				return col;
			}
			ENDCG
		}
	}
}