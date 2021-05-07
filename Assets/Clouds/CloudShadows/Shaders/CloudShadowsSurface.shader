Shader "Custom/CloudShadowsSurface" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[Normal]
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
		[Header(Opacity)]
		_CloudsColor ("Color", Color) = (0.5,0.5,0.5,1.0)
		[Header(Textures and Size)]
		_ShadowTex ("Shadow Texture", 2D) = "black" { }
		_NoiseTex ("Displacement Texture", 2D) = "black" { }
		[Header(Movement)]
		_SpeedX ("SpeedX", Float) = 1
		_SpeedY ("SpeedY", Float) = 1
		_Offset ("Offset", Float) = 1
		[Header(Height)]
		_Height ("Cutoff Height", Float) = 1.0
		_HeightBlend ("Height Edge Blend", Range(0,10)) = 10.0
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		half _Glossiness;
		half _Metallic;
		sampler2D _ShadowTex;
		sampler2D _NoiseTex;
		float4 _ShadowTex_ST;
		float4 _NoiseTex_ST;
		fixed4 _CloudsColor;
		half _SpeedX;
		half _SpeedY;
		half _Offset;
		half _Height;
		half _HeightBlend;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_ShadowTex;
			float2 uv_NoiseTex;
			float3 worldPos;
		};
		
		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
        }
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)*_Color;
			fixed4 noise = tex2D (_NoiseTex, float2(IN.worldPos.x*_NoiseTex_ST.x+frac(_Time.x*_SpeedX*_Offset),IN.worldPos.z*_NoiseTex_ST.y+frac(_Time.x*_SpeedY*_Offset)));
			fixed4 shadowtexture = tex2D (_ShadowTex, float2(IN.worldPos.x*_ShadowTex_ST.x+frac(_Time.x*_SpeedX),IN.worldPos.z*_ShadowTex_ST.y+frac(_Time.x*_SpeedY))+noise)+_CloudsColor-_CloudsColor.a;
			
			// height blending
			half height = IN.worldPos.y-_Height;
			half heightblend = saturate(height/-_HeightBlend);
			if (IN.worldPos.y<=_Height)
				shadowtexture.a = lerp(shadowtexture.a,0.0,heightblend);
				
			c = lerp(c,shadowtexture,shadowtexture.a*_CloudsColor.a); 
			 
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
