Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color ("Color", Color) = (0.4,0.4,0.4,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emission ("Emission", Range (0,1)) = 0.5
		
		_LineWidth("LineWidth", Range (0,0.1)) = 0.05
		_LineColor ("LineColor", Color) = (1,1,1,1)

    }
    SubShader
{
    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
    LOD 100

    //1. 기본 Pass
    Cull back
    ZTest lEqual
	//Zwrite off

    stencil
    {
        //ref 20
        comp equal

        pass IncrSat
        //zfail replace
    }

    CGPROGRAM
    #pragma surface surf Lambert
    #pragma target 3.0

    struct Input
    {
        float2 uv_MainTex;
    };

    sampler2D _MainTex;
    fixed4 _Color;
    fixed _Emission;

    void surf (Input IN, inout SurfaceOutput o) //표면 색상
    {
        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
        o.Emission = tex2D (_MainTex, IN.uv_MainTex);
        o.Alpha = c.a;
    }
    ENDCG


	//2. 외곽선 Pass
	Cull front
	ZTest notEqual
	//Zwrite off

	stencil
	{
	    //ref 20
	    //comp less

	    //pass decrsat
	    //zfail IncrSat
	}

	CGPROGRAM
	#pragma surface surf NoLight vertex:vert noshadow noambient
	#pragma target 3.0

	float _LineWidth;
	fixed4 _LineColor;

	void vert(inout appdata_full v) //정점 제어
	{
	    //vertex를 normal 방향으로 확장
	    v.vertex.xyz += v.normal.xyz * _LineWidth;
	}

	struct Input
	{
	    //Input에 값을 받지 않으면 오류 -> 의미없는 값 넣음
	    float2 uv_MainTex;
	};

	sampler2D _MainTex;
	fixed4 _Color;
	fixed _Emission;

	void surf (Input IN, inout SurfaceOutput o)
	{
	    fixed4 c = _Color;
	    o.Albedo = c.rgb;
	    o.Alpha = c.a;
	}

	float4 LightingNoLight (SurfaceOutput s, float3 lightDir, float atten)
	{
	    return _LineColor;
	}
	ENDCG
    }
    FallBack "Diffuse"
}
