Shader "Custom/EdgeGlow_surf" 
{

	Properties 
	{
		_Color ("Color", Color) = (0,0,0,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_HighlightColor ("Highlight", Color) = (1,1,1,1)
		_HighlightPower ("Highlight Power", Range(0.0,0.5)) = 0.05
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader 
	{
		Tags 
		{ 
			"DisableBatching"="True"
			"RenderType"="Opaque" 
		}

		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _HighlightColor;
		float _HighlightPower;
		half _Glossiness;
		half _Metallic;

		struct Input 
		{
			float2 uv_MainTex;
			float3 localPos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.localPos = v.vertex.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float limit = 0.5;
			_HighlightPower = limit - _HighlightPower;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			if((IN.localPos.x > _HighlightPower || IN.localPos.x < -_HighlightPower) &&
				(IN.localPos.x != limit && IN.localPos.x != -limit))
			{
				o.Albedo = _HighlightColor.rgb;
				o.Alpha = _HighlightColor.a;
			}

			if((IN.localPos.y > _HighlightPower || IN.localPos.y < -_HighlightPower) &&
				(IN.localPos.y != limit && IN.localPos.y != -limit))
			{
				o.Albedo = _HighlightColor.rgb;
				o.Alpha = _HighlightColor.a;
			}

			if((IN.localPos.z > _HighlightPower || IN.localPos.z < -_HighlightPower) &&
				(IN.localPos.z != limit && IN.localPos.z != -limit))
			{
				o.Albedo = _HighlightColor.rgb;
				o.Alpha = _HighlightColor.a;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
