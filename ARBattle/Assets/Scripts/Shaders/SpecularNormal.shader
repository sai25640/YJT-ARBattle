// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Yhf/SpecularNormal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("顶点颜色",Color) = (1.0,1.0,1.0,1.0)
		_SpecularColor("高光颜色",Color) = (1.0,1.0,1.0,1.0)
		_SpecularScope("高光范围",Float) = 10
	}
	SubShader {
		Tags { "LightMode"="ForwardBase" "Queue" = "Transparent" }
		Lighting on
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex main_v
			#pragma fragment main_f
			
			#include "UnityCG.cginc"
			
			uniform float4 _LightColor0;
			uniform sampler2D _MainTex;
			uniform float4 _Color;
			uniform float4 _SpecularColor;
			uniform float _SpecularScope;
			
			struct VertexOUT
			{
				float4 pos : SV_POSITION;
				half2 tex  : TEXCOORD0;
				float3 DiffColor : TEXCOORD1;
				float3 SpecColor : TEXCOORD2;
			};
			
			VertexOUT main_v(in appdata_base IN)
			{
				VertexOUT S;
				float4x4 fa = unity_ObjectToWorld;
				float4x4 fb = unity_WorldToObject;
				
				float3 normaldir =normalize(mul(float4(IN.normal,0.0f),fb).xyz);
				float3 Fc = normalize(_WorldSpaceCameraPos - mul(fa,IN.vertex).xyz);
				float3 Lightdir;
				float Diffdir;
				
				if(_WorldSpaceLightPos0.w == 0.0f)
				{
					Diffdir = 2.0f;
					Lightdir = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					float3 V = _WorldSpaceLightPos0.xyz - mul(fa,IN.vertex).xyz;
					float Distance = length(V);
					Diffdir = 1.0f / Distance;
					Lightdir = normalize(V);
				}
				
				float3 EnvironmentColor = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
				
				float3 diff = Diffdir * _LightColor0.rgb * _Color.rgb * max(0.0f,dot(normaldir, Lightdir));
				
				float3 SpecRef;
				if(dot(normaldir, Lightdir) < 0.0f)
				{
					SpecRef = float3(0.0f,0.0f,0.0f);
				}
				else
				{
					SpecRef = Diffdir * _LightColor0.rgb * _SpecularColor.rgb *
					 pow(max(0.0f,dot(reflect(-Lightdir, normaldir),Fc)),_SpecularScope);
				}
				S.pos = UnityObjectToClipPos(IN.vertex);
				S.tex = (half2)IN.texcoord.xy;
				S.DiffColor = diff;
				S.SpecColor = SpecRef;
				return S;
			}
			float4 main_f(in VertexOUT OUT) : COLOR
			{
				float4 col = tex2D(_MainTex,OUT.tex);
				if(col.a > 0.5f)
				{
					col.rgb *= OUT.DiffColor;
					col.rgb += OUT.SpecColor;
				}
				else
				{
					discard;
				}
				return col;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
