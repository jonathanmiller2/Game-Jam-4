Shader "GBAShader"
{
  Properties
  {
		//Cotton Candy
    _MainTex ("Texture", 2D) = "white" {}
    _Darkest ("Darkest", color) = (0, 0, 0)
    _Dark ("Dark", color) = (1, .33, 1)
    _Ligt ("Light", color) = (0.33, 1, 1)
    _Ligtest ("Lightest", color) = (1, 1, 1)
	
  }
  SubShader
  {
    // No culling or depth
    Cull Off ZWrite Off ZTest Always

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
    };

    struct v2f
    {
      float2 uv : TEXCOORD0;
      float4 vertex : SV_POSITION;
    };

    v2f vert (appdata v)
    {
      v2f o;
      o.vertex = UnityObjectToClipPos(v.vertex);
      o.uv = v.uv;
      return o;
    }

    sampler2D _MainTex;
    float4 _Darkest, _Dark, _Ligt, _Ligtest;

    float4 frag (v2f i) : SV_Target
    {
      float4 originalColor = tex2D(_MainTex, i.uv);

      float luma = dot(originalColor.rgb, float3(0.2126, 0.7152, 0.0722));
      float posterized = floor(luma * 4) / (4 - 1);
      float lumaTimesThree = posterized * 3.0;

      float darkest = saturate(lumaTimesThree);
      float4 color = lerp(_Darkest, _Dark, darkest);
	  
      float dark = saturate(lumaTimesThree - 1.0);
      color = lerp(color, _Dark, dark);
	  
	  float light = saturate(lumaTimesThree - 2.0);
      color = lerp(color, _Ligt, light);

      float lightest = saturate(lumaTimesThree - 5.0);
      color = lerp(color, _Ligtest, lightest);

      return color;
    }
    ENDCG
    }
  }
}
