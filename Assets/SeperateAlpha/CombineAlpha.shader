Shader "Custom/CombineAlpha" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_AlphaTex ("Appha (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags {"Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			
			struct a2v
			{
				fixed4 pos : POSITION;
				fixed2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			
			v2f vert(a2v input)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
				o.uv = input.uv;
				return o;
			}
			
			half4 frag(v2f o) : COLOR
			{
				half4 col;
				col.rgb = tex2D(_MainTex, o.uv).rgb;
				col.a = tex2D(_AlphaTex, o.uv).b;
				return col;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
