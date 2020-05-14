Shader " Custom/Rolling "
{ 
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Bend("Bend",Float) = 1
_Strength("Strength",Float) = 1
}

SubShader
{
Tags {"Queue"="Geometry" "IgnoreProjector"="true" "RenderType"="Opaque"}
Cull Off

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct v2f
{
half2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
};

sampler2D _MainTex;
float _Bend;
float _Strength;

v2f vert(appdata_full v)
{
v2f o;

//I don't know what should I write here for rolling ,please help me.
//v.vertex.y *= ???
v.vertex.y *= exp(_Bend+(v.vertex.x*_Strength)); // something like this but this hasn't twist effect
o.vertex = UnityObjectToClipPos(v.vertex);
o.texcoord = v.texcoord;
return o;
}

float4 frag (v2f i) : COLOR
{

float2 uv = i.texcoord.xy;
float4 tex = tex2D(_MainTex, uv);

return tex;
}
ENDCG
}
}
Fallback Off
}
