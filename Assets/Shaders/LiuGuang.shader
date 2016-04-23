Shader "Custom/LiuGuang" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FlowLightTex("Light Texture(A)", 2D) = "black" {}
		_UvPos("_UvPos", range(0.5, 1.5)) = 0.5				//uv offset
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		sampler2D _MainTex;
		sampler2D _FlowLightTex;
		float _UvPos;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			float2 uv = IN.uv_MainTex;
			uv.x /= 2;
			uv.x += _UvPos;
			
			float fLight = tex2D(_FlowLightTex, uv).a;
			o.Albedo = c.rgb + float3(fLight, fLight, fLight);
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
