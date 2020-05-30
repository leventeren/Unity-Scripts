Shader "Toon/Lit Environment Detail" {
    Properties {
       
        [Header(Main Texture)]
        _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Base (RGB)", 2D) = "black" {}
        _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
         [Space]
        [Header(Rim Lighting)]
        _RimColor ("Rim Color", Color) = (0.5,0.5,0.5,1)
        _RimPower("Rim Power", Range(0,50)) = 20
         [Space]
        [Header(Extra Worldspace Texture)]
        _ExtraTex ("Extra Texture (RGB)", 2D) = "black" {}
        _ExtraTex2 ("Extra Texture 2 (RGB)", 2D) = "black" {}
        _ExtraColor ("Extra Color", Color) = (0.5,0.5,0.5,1)
        _Scale("Extra Scale", Range(0,2)) = 1
        _Scale2("Extra Scale 2", Range(0,2)) = 1
        _TextureOpacity("Texture Opacity", Range(-6,6)) = 1
         [Space]
        [Header(TopTexture)]
        [Toggle(TOP)] _TOP("Top Texture", Float) = 0
        _TColor ("Top Color", Color) = (0.5,0.5,0.5,1)
        _TopSpread("Top Spread", Range(0.01,2)) = 1
        _TopScale("Top Scale", Range(0,2)) = 1
        _NoiseScale("Noise Scale", Range(0,2)) = 1
        _Noise ("Noise Texture", 2D) = "gray" {}
        _TopTex ("Top Texture (RGB)", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (0.5,0.5,0.5,1)
        _EdgeWidth("Edge Width", Range(0,2)) = 1
       
       
    }
 
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off
 
        CGPROGRAM
        #pragma surface surf ToonRamp  
        #pragma shader_feature TOP //
        sampler2D _Ramp;
 
        // custom lighting function that uses a texture ramp based
        // on angle between light direction and normal
        #pragma lighting ToonRamp exclude_path:prepass
        inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten) {
            #ifndef USING_DIRECTIONAL_LIGHT
            lightDir = normalize(lightDir);
            #endif
 
            half d = dot (s.Normal, lightDir)*0.5 + 0.5;
            half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
 
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
            c.a = 0;
            return c;
        }
 
        sampler2D _MainTex, _ExtraTex, _TopTex, _Noise, _ExtraTex2;
        float4 _Color, _RimColor, _TColor, _EdgeColor,_ExtraColor;
        float _Scale,_TextureOpacity,_RimPower;
        float _TopSpread,_TopScale, _NoiseScale, _EdgeWidth, _Scale2;
 
        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float3 worldNormal; // world normal built-in value
            float3 worldPos; // world position built-in value
            float3 viewDir;
 
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
           
            float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
 
            // triplanar
            float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));
            // triplanar for extra texture for x, y, z sides
            float3 xm = tex2D(_ExtraTex, IN.worldPos.zy * _Scale);
            float3 zm = tex2D(_ExtraTex, IN.worldPos.xy * _Scale);
            float3 ym = tex2D(_ExtraTex, IN.worldPos.zx * _Scale);
 
            // lerped together all sides for extra texture
            float3 extratexture = zm;
            extratexture = lerp(extratexture, xm, blendNormal.x);
            extratexture = lerp(extratexture, ym, blendNormal.y);
 
            // extratex second
            float3 xmm = tex2D(_ExtraTex2, IN.worldPos.zy  * _Scale2);
            float3 zmm = tex2D(_ExtraTex2, IN.worldPos.xy * _Scale2);
            float3 ymm = tex2D(_ExtraTex2, IN.worldPos.zx * _Scale2);
 
            // lerped together all sides for extra texture 2
            float3 extratexture2 = zmm;
            extratexture2 = lerp(extratexture2, xmm, blendNormal.x);
            extratexture2 = lerp(extratexture2, ymm, blendNormal.y);
 
            extratexture = (extratexture2 * extratexture);
 
            // rim light
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = (_RimColor.rgb ) * pow(rim, _RimPower);
 
            // triplanar for top texture, x,y,z sides
            float3 x = tex2D(_TopTex, IN.worldPos.zy * _TopScale);
            float3 y = tex2D(_TopTex, IN.worldPos.zx * _TopScale);
            float3 z = tex2D(_TopTex, IN.worldPos.xy * _TopScale);
 
        // lerped together all sides for top texture
            float3 toptexture = z;
            toptexture = lerp(toptexture, x, blendNormal.x);
            toptexture = lerp(toptexture, y, blendNormal.y);
 
        // normal noise triplanar for x, y, z sides
            float3 xn = tex2D(_Noise, IN.worldPos.zy * _NoiseScale);
            float3 yn = tex2D(_Noise, IN.worldPos.zx * _NoiseScale);
            float3 zn = tex2D(_Noise, IN.worldPos.xy * _NoiseScale);
 
        // lerped together all sides for noise texture
            float3 noisetexture = zn;
            noisetexture = lerp(noisetexture, xn, blendNormal.x);
            noisetexture = lerp(noisetexture, yn, blendNormal.y);
            noisetexture *= 2;
 
        float worldNormalDotNoise = dot(o.Normal * noisetexture, saturate(IN.worldNormal.y));
 
        // if dot product is higher than the top spread slider, multiplied by triplanar mapped top texture
        float3 topTextureResult = step(_TopSpread + _EdgeWidth, worldNormalDotNoise) * toptexture;
        float3 topTextureEdgeResult = (step(_TopSpread , worldNormalDotNoise) * step(worldNormalDotNoise, _TopSpread + _EdgeWidth)) * _EdgeColor;  
        float3 extraTexOpacity =  extratexture * _TextureOpacity;
 
            o.Albedo = lerp(c.rgb, float3(0,0,0),extraTexOpacity) + (extraTexOpacity* _ExtraColor * c.rgb );
#if TOP
            o.Albedo = (step( worldNormalDotNoise, _TopSpread) * ((lerp(c.rgb, float3(0,0,0), (extraTexOpacity)) ) + (extraTexOpacity * _ExtraColor* c.rgb))) + (topTextureResult * _TColor) + topTextureEdgeResult;
#endif         
            o.Alpha = c.a;
        }
 
        ENDCG
    }
 
    Fallback "Diffuse"
}
