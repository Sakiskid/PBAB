//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.9.6                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Customs/TMP_SDF"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_TintRGBA_Color_1("_TintRGBA_Color_1", COLOR) = (1,1,1,0)
_ColorHSV_Hue_1("_ColorHSV_Hue_1", Range(0, 360)) = 180
_ColorHSV_Saturation_1("_ColorHSV_Saturation_1", Range(0, 2)) = 1
_ColorHSV_Brightness_1("_ColorHSV_Brightness_1", Range(0, 2)) = 1
AnimatedMouvementUV_X_1("AnimatedMouvementUV_X_1", Range(-1, 1)) = 0.25
AnimatedMouvementUV_Y_1("AnimatedMouvementUV_Y_1", Range(-1, 1)) = 0
AnimatedMouvementUV_Speed_1("AnimatedMouvementUV_Speed_1", Range(-1, 1)) = 0.254
_NewTex_1("NewTex_1(RGB)", 2D) = "white" { }
_PlasmaFX_Fade_1("_PlasmaFX_Fade_1", Range(0, 1)) = 0.604
_PlasmaFX_Speed_1("_PlasmaFX_Speed_1", Range(0, 1)) = 0.604
_OperationBlend_Fade_1("_OperationBlend_Fade_1", Range(0, 1)) = 0.636
_SpriteFade("SpriteFade", Range(0, 1)) = 1.0

// required for UI.Mask
[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
[HideInInspector]_Stencil("Stencil ID", Float) = 0
[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
[HideInInspector]_ColorMask("Color Mask", Float) = 15

}

SubShader
{

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off 

// required for UI.Mask
Stencil
{
Ref [_Stencil]
Comp [_StencilComp]
Pass [_StencilOp]
ReadMask [_StencilReadMask]
WriteMask [_StencilWriteMask]
}

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};

sampler2D _MainTex;
float _SpriteFade;
float4 _TintRGBA_Color_1;
float _ColorHSV_Hue_1;
float _ColorHSV_Saturation_1;
float _ColorHSV_Brightness_1;
float AnimatedMouvementUV_X_1;
float AnimatedMouvementUV_Y_1;
float AnimatedMouvementUV_Speed_1;
sampler2D _NewTex_1;
float _PlasmaFX_Fade_1;
float _PlasmaFX_Speed_1;
float _OperationBlend_Fade_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float4 TintRGBA(float4 txt, float4 color)
{
float3 tint = dot(txt.rgb, float3(.222, .707, .071));
tint.rgb *= color.rgb;
txt.rgb = lerp(txt.rgb,tint.rgb,color.a);
return txt;
}
inline float RBFXmod(float x,float modu)
{
return x - floor(x * (1.0 / modu)) * modu;
}

float3 RBFXrainbow(float t)
{
t= RBFXmod(t,1.0);
float tx = t * 8;
float r = clamp(tx - 4.0, 0.0, 1.0) + clamp(2.0 - tx, 0.0, 1.0);
float g = tx < 2.0 ? clamp(tx, 0.0, 1.0) : clamp(4.0 - tx, 0.0, 1.0);
float b = tx < 4.0 ? clamp(tx - 2.0, 0.0, 1.0) : clamp(6.0 - tx, 0.0, 1.0);
return float3(r, g, b);
}

float4 Plasma(float4 txt, float2 uv, float _Fade, float speed)
{
float _TimeX=_Time.y * speed;
float a = 1.1 + _TimeX * 2.25;
float b = 0.5 + _TimeX * 1.77;
float c = 8.4 + _TimeX * 1.58;
float d = 610 + _TimeX * 2.03;
float x1 = 2.0 * uv.x;
float n = sin(a + x1) + sin(b - x1) + sin(c + 2.0 * uv.y) + sin(d + 5.0 * uv.y);
n = RBFXmod(((5.0 + n) / 5.0), 1.0);
float4 nx=txt;
n += nx.r * 0.2 + nx.g * 0.4 + nx.b * 0.2;
float4 ret=float4(RBFXrainbow(n),txt.a);
return lerp(txt,ret,_Fade);
}
float4 ColorHSV(float4 RGBA, float HueShift, float Sat, float Val)
{

float4 RESULT = float4(RGBA);
float a1 = Val*Sat;
float a2 = HueShift*3.14159265 / 180;
float VSU = a1*cos(a2);
float VSW = a1*sin(a2);

RESULT.x = (.299*Val + .701*VSU + .168*VSW)*RGBA.x
+ (.587*Val - .587*VSU + .330*VSW)*RGBA.y
+ (.114*Val - .114*VSU - .497*VSW)*RGBA.z;

RESULT.y = (.299*Val - .299*VSU - .328*VSW)*RGBA.x
+ (.587*Val + .413*VSU + .035*VSW)*RGBA.y
+ (.114*Val - .114*VSU + .292*VSW)*RGBA.z;

RESULT.z = (.299*Val - .3*VSU + 1.25*VSW)*RGBA.x
+ (.587*Val - .588*VSU - 1.05*VSW)*RGBA.y
+ (.114*Val + .886*VSU - .203*VSW)*RGBA.z;

return RESULT;
}
float4 OperationBlend(float4 origin, float4 overlay, float blend)
{
float4 o = origin; 
o.a = overlay.a + origin.a * (1 - overlay.a);
o.rgb = (overlay.rgb * overlay.a + origin.rgb * origin.a * (1 - overlay.a)) * (o.a+0.0000001);
o.a = saturate(o.a);
o = lerp(origin, o, blend);
return o;
}
float2 AnimatedMouvementUV(float2 uv, float offsetx, float offsety, float speed)
{
speed *=_Time*50;
uv += float2(offsetx, offsety)*speed;
uv = fmod(uv,1);
return uv;
}
float4 frag (v2f i) : COLOR
{
float4 _MainTex_2 = tex2D(_MainTex, i.texcoord);
float4 TintRGBA_1 = TintRGBA(_MainTex_2,_TintRGBA_Color_1);
float4 _ColorHSV_1 = ColorHSV(TintRGBA_1,_ColorHSV_Hue_1,_ColorHSV_Saturation_1,_ColorHSV_Brightness_1);
float4 _MainTex_1 = tex2D(_MainTex, i.texcoord);
float2 AnimatedMouvementUV_1 = AnimatedMouvementUV(i.texcoord,AnimatedMouvementUV_X_1,AnimatedMouvementUV_Y_1,AnimatedMouvementUV_Speed_1);
float4 NewTex_1 = tex2D(_NewTex_1,AnimatedMouvementUV_1);
NewTex_1.a = _MainTex_1.a;
float4 _PlasmaFX_1 = Plasma(NewTex_1,i.texcoord,_PlasmaFX_Fade_1,_PlasmaFX_Speed_1);
float4 OperationBlend_1 = OperationBlend(_ColorHSV_1, _PlasmaFX_1, _OperationBlend_Fade_1); 
float4 FinalResult = OperationBlend_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}
