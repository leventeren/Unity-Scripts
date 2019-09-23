Shader "AlphaShader"
{
  Properties
  {
    _Distance ("Distance", Float) = 0
    _FadeStartDistance ("FadeStartDistance", Float) = 8
    _FadeCompleteDistance ("FadeCompleteDistance", Float) = 3
  }

  SubShader
  {
    Pass
    {
      AlphaToMask On

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      float _Distance;
      float _FadeStartDistance;
      float _FadeCompleteDistance;

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

      fixed4 frag (v2f i) : SV_Target
      {
        if (_Distance > _FadeStartDistance) {
          return fixed4(1, 1, 1, 1); // no change
        }
        if (_Distance > _FadeCompleteDistance) {
          return fixed4(1, 1, 1, (_Distance - _FadeCompleteDistance) / (_FadeStartDistance - _FadeCompleteDistance)); // fading out
        }
          return fixed4(1, 1, 1, 0); // faded out. Opacity = 0
      }
      ENDCG
    }
  }
}

