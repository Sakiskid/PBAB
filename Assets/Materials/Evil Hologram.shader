//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.9.6                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Customs/Evil Hologram"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_Hologram_Value_1("_Hologram_Value_1", Range(0, 1)) = 1
_Hologram_Speed_1("_Hologram_Speed_1", Range(0, 4)) = 1
_OutlineEmpty_Size_1("_OutlineEmpty_Size_1", Range(1, 16)) = 16.0
_OutlineEmpty_Color_1("_OutlineEmpty_Color_1", COLOR) = (1,1,1,1)
_MaskAlpha_Fade_1("_MaskAlpha_Fade_1", Range(0, 1)) = 0
__CompressionFX_Value_1("__CompressionFX_Value_1", Range(1, 16)) = 16
_OperationBlend_Fade_3("_OperationBlend_Fade_3", Range(0, 1)) = 1
_TurnGold_Speed_1("_TurnGold_Speed_1", Range(-8, 8)) = 1
_OperationBlend_Fade_1("_OperationBlend_Fade_1", Range(0, 1)) = 0.336
AnimatedOffsetUV_X_1("AnimatedOffsetUV_X_1", Range(-1, 1)) = 0.215
AnimatedOffsetUV_Y_1("AnimatedOffsetUV_Y_1", Range(-1, 1)) = 0
AnimatedOffsetUV_ZoomX_1("AnimatedOffsetUV_ZoomX_1", Range(1, 10)) = 1
AnimatedOffsetUV_ZoomY_1("AnimatedOffsetUV_ZoomY_1", Range(1, 10)) = 1
AnimatedOffsetUV_Speed_1("AnimatedOffsetUV_Speed_1", Range(-1, 1)) = 0.797
_Generate_Fire_PosX_1("_Generate_Fire_PosX_1", Range(-1, 2)) = -0.214
_Generate_Fire_PosY_1("_Generate_Fire_PosY_1", Range(-1, 2)) = -0.107
_Generate_Fire_Precision_1("_Generate_Fire_Precision_1", Range(0, 1)) = 0.023
_Generate_Fire_Smooth_1("_Generate_Fire_Smooth_1", Range(0, 1)) = 0.777
_Generate_Fire_Speed_1("_Generate_Fire_Speed_1", Range(-2, 2)) = 1.107
_Add_Fade_1("_Add_Fade_1", Range(0, 4)) = 0.214
_OperationBlend_Fade_2("_OperationBlend_Fade_2", Range(0, 1)) = 1
_TintRGBA_Color_1("_TintRGBA_Color_1", COLOR) = (1,0,0,1)
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
float _Hologram_Value_1;
float _Hologram_Speed_1;
float _OutlineEmpty_Size_1;
float4 _OutlineEmpty_Color_1;
float _MaskAlpha_Fade_1;
float __CompressionFX_Value_1;
float _OperationBlend_Fade_3;
float _TurnGold_Speed_1;
float _OperationBlend_Fade_1;
float AnimatedOffsetUV_X_1;
float AnimatedOffsetUV_Y_1;
float AnimatedOffsetUV_ZoomX_1;
float AnimatedOffsetUV_ZoomY_1;
float AnimatedOffsetUV_Speed_1;
float _Generate_Fire_PosX_1;
float _Generate_Fire_PosY_1;
float _Generate_Fire_Precision_1;
float _Generate_Fire_Smooth_1;
float _Generate_Fire_Speed_1;
float _Add_Fade_1;
float _OperationBlend_Fade_2;
float4 _TintRGBA_Color_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float2 AnimatedOffsetUV(float2 uv, float offsetx, float offsety, float zoomx, float zoomy, float speed)
{
speed *=_Time*25;
uv += float2(offsetx*speed, offsety*speed);
uv = fmod(uv * float2(zoomx, zoomy), 1);
return uv;
}
float CMPFXrng2(float2 seed)
{
return frac(sin(dot(seed * floor(50 + (_Time + 0.1) * 12.), float2(127.1, 311.7))) * 43758.5453123);
}

float CMPFXrng(float seed)
{
return CMPFXrng2(float2(seed, 1.0));
}

float4 CompressionFX(float2 uv, sampler2D source,float Value)
{
float2 blockS = floor(uv * float2(24., 19.))*4.0;
float2 blockL = floor(uv * float2(38., 14.))*4.0;
float r = CMPFXrng2(uv);
float lineNoise = pow(CMPFXrng2(blockS), 3.0) *Value* pow(CMPFXrng2(blockL), 3.0);
float4 col1 = tex2D(source, uv + float2(lineNoise * 0.02 * CMPFXrng(2.0), 0));
float4 result = float4(float3(col1.x, col1.y, col1.z), 1.0);
result.a = col1.a;
return result;
}
float4 OutLineEmpty(float2 uv,sampler2D source, float value, float4 color)
{

value*=0.01;
float4 mainColor = tex2D(source, uv + float2(-value, value))
+ tex2D(source, uv + float2(value, -value))
+ tex2D(source, uv + float2(value, value))
+ tex2D(source, uv - float2(value, value));

mainColor.rgb = color;
float4 addcolor = tex2D(source, uv);
if (mainColor.a > 0.40) { mainColor = color; }
if (addcolor.a > 0.40) { mainColor.a = 0; }
return mainColor;
}
inline float Holo1mod(float x,float modu)
{
return x - floor(x * (1.0 / modu)) * modu;
}

inline float Holo1noise(sampler2D source,float2 p)
{
float _TimeX = _Time.y;
float sample = tex2D(source,float2(.2,0.2*cos(_TimeX))*_TimeX*8. + p*1.).x;
sample *= sample;
return sample;
}

inline float Holo1onOff(float a, float b, float c)
{
float _TimeX = _Time.y;
return step(c, sin(_TimeX + a*cos(_TimeX*b)));
}

float4 Hologram(float2 uv, sampler2D source, float value, float speed)
{
float alpha = tex2D(source, uv).a;
float _TimeX = _Time.y * speed;
float2 look = uv;
float window = 1. / (1. + 20.*(look.y - Holo1mod(_TimeX / 4., 1.))*(look.y - Holo1mod(_TimeX / 4., 1.)));
look.x = look.x + sin(look.y*30. + _TimeX) / (50.*value)*Holo1onOff(4., 4., .3)*(1. + cos(_TimeX*80.))*window;
float vShift = .4*Holo1onOff(2., 3., .9)*(sin(_TimeX)*sin(_TimeX*20.) + (0.5 + 0.1*sin(_TimeX*20.)*cos(_TimeX)));
look.y = Holo1mod(look.y + vShift, 1.);
float4 video = float4(0, 0, 0, 0);
float4 videox = tex2D(source, look);
video.r = tex2D(source, look - float2(.05, 0.)*Holo1onOff(2., 1.5, .9)).r;
video.g = videox.g;
video.b = tex2D(source, look + float2(.05, 0.)*Holo1onOff(2., 1.5, .9)).b;
video.a = videox.a;
video = video;
float vigAmt = 3. + .3*sin(_TimeX + 5.*cos(_TimeX*5.));
float vignette = (1. - vigAmt*(uv.y - .5)*(uv.y - .5))*(1. - vigAmt*(uv.x - .5)*(uv.x - .5));
float noi = Holo1noise(source,uv*float2(0.5, 1.) + float2(6., 3.))*value * 3;
float y = Holo1mod(uv.y*4. + _TimeX / 2. + sin(_TimeX + sin(_TimeX*0.63)), 1.);
float start = .5;
float end = .6;
float inside = step(start, y) - step(end, y);
float fact = (y - start) / (end - start)*inside;
float f1 = (1. - fact) * inside;
video += f1*noi;
video += Holo1noise(source,uv*2.) / 2.;
video.r *= vignette;
video *= (12. + Holo1mod(uv.y*30. + _TimeX, 1.)) / 13.;
video.a = video.a + (frac(sin(dot(uv.xy*_TimeX, float2(12.9898, 78.233))) * 43758.5453))*.5;
video.a = (video.a*.3)*alpha*vignette * 2;
video.a *=1.2;
video.a *= 1.2;
video = lerp(tex2D(source, uv), video, value);
return video;
}
float4 TintRGBA(float4 txt, float4 color)
{
float3 tint = dot(txt.rgb, float3(.222, .707, .071));
tint.rgb *= color.rgb;
txt.rgb = lerp(txt.rgb,tint.rgb,color.a);
return txt;
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
float Generate_Fire_hash2D(float2 x)
{
return frac(sin(dot(x, float2(13.454, 7.405)))*12.3043);
}

float Generate_Fire_voronoi2D(float2 uv, float precision)
{
float2 fl = floor(uv);
float2 fr = frac(uv);
float res = 1.0;
for (int j = -1; j <= 1; j++)
{
for (int i = -1; i <= 1; i++)
{
float2 p = float2(i, j);
float h = Generate_Fire_hash2D(fl + p);
float2 vp = p - fr + h;
float d = dot(vp, vp);
res += 1.0 / pow(d, 8.0);
}
}
return pow(1.0 / res, precision);
}

float4 Generate_Fire(float2 uv, float posX, float posY, float precision, float smooth, float speed, float black)
{
uv += float2(posX, posY);
float t = _Time*60*speed;
float up0 = Generate_Fire_voronoi2D(uv * float2(6.0, 4.0) + float2(0, -t), precision);
float up1 = 0.5 + Generate_Fire_voronoi2D(uv * float2(6.0, 4.0) + float2(42, -t ) + 30.0, precision);
float finalMask = up0 * up1  + (1.0 - uv.y);
finalMask += (1.0 - uv.y)* 0.5;
finalMask *= 0.7 - abs(uv.x - 0.5);
float4 result = smoothstep(smooth, 0.95, finalMask);
result.a = saturate(result.a + black);
return result;
}
float4 ColorTurnGold(float2 uv, sampler2D txt, float speed)
{
float4 txt1=tex2D(txt,uv);
float lum = dot(txt1.rgb, float3 (0.2126, 0.2152, 0.4722));
float3 metal = float3(lum,lum,lum);
metal.r = lum * pow(1.46*lum, 4.0);
metal.g = lum * pow(1.46*lum, 4.0);
metal.b = lum * pow(0.86*lum, 4.0);
float2 tuv = uv;
uv *= 2.5;
float time = (_Time/4)*speed;
float a = time * 50;
float n = sin(a + 2.0 * uv.x) + sin(a - 2.0 * uv.x) + sin(a + 2.0 * uv.y) + sin(a + 5.0 * uv.y);
n = fmod(((5.0 + n) / 5.0), 1.0);
n += tex2D(txt, tuv).r * 0.21 + tex2D(txt, tuv).g * 0.4 + tex2D(txt, tuv).b * 0.2;
n=fmod(n,1.0);
float tx = n * 6.0;
float r = clamp(tx - 2.0, 0.0, 1.0) + clamp(2.0 - tx, 0.0, 1.0);
float4 sortie=float4(1.0, 1.0, 1.0,r);
sortie.rgb=metal.rgb+(1-sortie.a);
sortie.rgb=sortie.rgb/2+dot(sortie.rgb, float3 (0.1126, 0.4552, 0.1722));
sortie.rgb-=float3(0.0,0.1,0.45);
sortie.rg+=0.025;
sortie.a=txt1.a;
return sortie; 
}
float4 frag (v2f i) : COLOR
{
float4 _Hologram_1 = Hologram(i.texcoord,_MainTex,_Hologram_Value_1,_Hologram_Speed_1);
float4 _OutlineEmpty_1 = OutLineEmpty(i.texcoord,_MainTex,_OutlineEmpty_Size_1,_OutlineEmpty_Color_1);
float4 MaskAlpha_1=_Hologram_1;
MaskAlpha_1.a = lerp(_OutlineEmpty_1.a, 1 - _OutlineEmpty_1.a,_MaskAlpha_Fade_1);
float4 _CompressionFX_1 = CompressionFX(i.texcoord,_MainTex,__CompressionFX_Value_1);
float4 OperationBlend_3 = OperationBlend(MaskAlpha_1, _CompressionFX_1, _OperationBlend_Fade_3); 
float4 _MainTex_1 = tex2D(_MainTex, i.texcoord);
float4 _TurnGold_1 = ColorTurnGold(i.texcoord,_MainTex,_TurnGold_Speed_1);
float4 OperationBlend_1 = OperationBlend(_MainTex_1, _TurnGold_1, _OperationBlend_Fade_1); 
float2 AnimatedOffsetUV_1 = AnimatedOffsetUV(i.texcoord,AnimatedOffsetUV_X_1,AnimatedOffsetUV_Y_1,AnimatedOffsetUV_ZoomX_1,AnimatedOffsetUV_ZoomY_1,AnimatedOffsetUV_Speed_1);
float4 _Generate_Fire_1 = Generate_Fire(AnimatedOffsetUV_1,_Generate_Fire_PosX_1,_Generate_Fire_PosY_1,_Generate_Fire_Precision_1,_Generate_Fire_Smooth_1,_Generate_Fire_Speed_1,0);
OperationBlend_1 = lerp(OperationBlend_1,OperationBlend_1*OperationBlend_1.a + _Generate_Fire_1*_Generate_Fire_1.a,_Add_Fade_1 * OperationBlend_1.a);
float4 OperationBlend_2 = OperationBlend(OperationBlend_3, OperationBlend_1, _OperationBlend_Fade_2); 
float4 TintRGBA_1 = TintRGBA(OperationBlend_2,_TintRGBA_Color_1);
float4 FinalResult = TintRGBA_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}
