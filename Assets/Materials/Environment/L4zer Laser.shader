//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.9.6                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Customs/L4zer Laser"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
RotationUV_Rotation_1("RotationUV_Rotation_1", Range(-360, 360)) = 90
RotationUV_Rotation_PosX_1("RotationUV_Rotation_PosX_1", Range(-1, 2)) = 0.5
RotationUV_Rotation_PosY_1("RotationUV_Rotation_PosY_1", Range(-1, 2)) =0.5
RotationUV_Rotation_Speed_1("RotationUV_Rotation_Speed_1", Range(-8, 8)) =0
AnimatedOffsetUV_X_2("AnimatedOffsetUV_X_2", Range(-1, 1)) = 0
AnimatedOffsetUV_Y_2("AnimatedOffsetUV_Y_2", Range(-1, 1)) = 1
AnimatedOffsetUV_ZoomX_2("AnimatedOffsetUV_ZoomX_2", Range(1, 10)) = 1
AnimatedOffsetUV_ZoomY_2("AnimatedOffsetUV_ZoomY_2", Range(1, 10)) = 1
AnimatedOffsetUV_Speed_2("AnimatedOffsetUV_Speed_2", Range(-1, 1)) = 0.307
_NewTex_2("NewTex_2(RGB)", 2D) = "white" { }
_Destroyer_Value_1("_Destroyer_Value_1", Range(0, 1)) = 0.5
_Destroyer_Speed_1("_Destroyer_Speed_1", Range(0, 1)) =  0.5
_Darkness_Fade_1("_Darkness_Fade_1", Range(0, 1)) = 0
AnimatedOffsetUV_X_1("AnimatedOffsetUV_X_1", Range(-1, 1)) = 0
AnimatedOffsetUV_Y_1("AnimatedOffsetUV_Y_1", Range(-1, 1)) = 0.789
AnimatedOffsetUV_ZoomX_1("AnimatedOffsetUV_ZoomX_1", Range(1, 10)) = 1
AnimatedOffsetUV_ZoomY_1("AnimatedOffsetUV_ZoomY_1", Range(1, 10)) = 1
AnimatedOffsetUV_Speed_1("AnimatedOffsetUV_Speed_1", Range(-1, 1)) = 1
_NewTex_1("NewTex_1(RGB)", 2D) = "white" { }
KaleidoscopeUV_PosX_1("KaleidoscopeUV_PosX_1",  Range(-2, 2)) = 0.5
KaleidoscopeUV_PosY_1("KaleidoscopeUV_PosY_1",  Range(-2, 2)) = 0.5
KaleidoscopeUV_Number_1("KaleidoscopeUV_Number_1", Range(0, 6)) = 1.225
_Displacement_Value_1("_Displacement_Value_1", Range(-0.3, 0.3)) = 0.185
_Burn_Value_1("_Burn_Value_1", Range(0, 1)) = 0.5
_Burn_Speed_1("_Burn_Speed_1", Range(-8, 8)) = -3.114
_Add_Fade_1("_Add_Fade_1", Range(0, 4)) = 1
_ThresholdSmooth_Value_1("_ThresholdSmooth_Value_1", Range(-1, 2)) = 0.137
_ThresholdSmooth_Smooth_1("_ThresholdSmooth_Smooth_1", Range(0, 1)) = 0.8
_TintRGBA_Color_1("_TintRGBA_Color_1", COLOR) = (1,0,0,1)
_Add_Fade_2("_Add_Fade_2", Range(0, 4)) = 2.093
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
float RotationUV_Rotation_1;
float RotationUV_Rotation_PosX_1;
float RotationUV_Rotation_PosY_1;
float RotationUV_Rotation_Speed_1;
float AnimatedOffsetUV_X_2;
float AnimatedOffsetUV_Y_2;
float AnimatedOffsetUV_ZoomX_2;
float AnimatedOffsetUV_ZoomY_2;
float AnimatedOffsetUV_Speed_2;
sampler2D _NewTex_2;
float _Destroyer_Value_1;
float _Destroyer_Speed_1;
float _Darkness_Fade_1;
float AnimatedOffsetUV_X_1;
float AnimatedOffsetUV_Y_1;
float AnimatedOffsetUV_ZoomX_1;
float AnimatedOffsetUV_ZoomY_1;
float AnimatedOffsetUV_Speed_1;
sampler2D _NewTex_1;
float KaleidoscopeUV_PosX_1;
float KaleidoscopeUV_PosY_1;
float KaleidoscopeUV_Number_1;
float _Displacement_Value_1;
float _Burn_Value_1;
float _Burn_Speed_1;
float _Add_Fade_1;
float _ThresholdSmooth_Value_1;
float _ThresholdSmooth_Smooth_1;
float4 _TintRGBA_Color_1;
float _Add_Fade_2;

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
float2 RotationUV(float2 uv, float rot, float posx, float posy, float speed)
{
rot=rot+(_Time*speed*360);
uv = uv - float2(posx, posy);
float angle = rot * 0.01744444;
float sinX = sin(angle);
float cosX = cos(angle);
float2x2 rotationMatrix = float2x2(cosX, -sinX, sinX, cosX);
uv = mul(uv, rotationMatrix) + float2(posx, posy);
return uv;
}
float4 TintRGBA(float4 txt, float4 color)
{
float3 tint = dot(txt.rgb, float3(.222, .707, .071));
tint.rgb *= color.rgb;
txt.rgb = lerp(txt.rgb,tint.rgb,color.a);
return txt;
}
float4 Darkness(float4 txt, float value)
{
txt.rgb -= value;
return saturate(txt);
}
float DSFXr (float2 c, float seed)
{
return frac(43.*sin(c.x+7.*c.y)*seed);
}

float DSFXn (float2 p, float seed)
{
float2 i = floor(p), w = p-i, j = float2 (1.,0.);
w = w*w*(3.-w-w);
return lerp(lerp(DSFXr(i, seed), DSFXr(i+j, seed), w.x), lerp(DSFXr(i+j.yx, seed), DSFXr(i+1., seed), w.x), w.y);
}

float DSFXa (float2 p, float seed)
{
float m = 0., f = 2.;
for ( int i=0; i<9; i++ ){ m += DSFXn(f*p, seed)/f; f+=f; }
return m;
}

float4 DestroyerFX(float4 txt, float2 uv, float value, float seed, float HDR)
{
float t = frac(value*0.9999);
float4 c = smoothstep(t / 1.2, t + .1, DSFXa(3.5*uv, seed));
c = txt*c;
c.r = lerp(c.r, c.r*120.0*(1 - c.a), value);
c.g = lerp(c.g, c.g*40.0*(1 - c.a), value);
c.b = lerp(c.b, c.b*5.0*(1 - c.a) , value);
c.rgb = lerp(saturate(c.rgb),c.rgb,HDR);
return c;
}
float BFXr (float2 c, float seed)
{
return frac(43.*sin(c.x+7.*c.y)* seed);
}

float BFXn (float2 p, float seed)
{
float2 i = floor(p), w = p-i, j = float2 (1.,0.);
w = w*w*(3.-w-w);
return lerp(lerp(BFXr(i, seed), BFXr(i+j, seed), w.x), lerp(BFXr(i+j.yx, seed), BFXr(i+1., seed), w.x), w.y);
}

float BFXa (float2 p, float seed)
{
float m = 0., f = 2.;
for ( int i=0; i<9; i++ ){ m += BFXn(f*p, seed)/f; f+=f; }
return m;
}

float4 BurnFX(float4 txt, float2 uv, float value, float seed, float HDR)
{
float t = frac(value*0.9999);
float4 c = smoothstep(t / 1.2, t + .1, BFXa(3.5*uv, seed));
c = txt*c;
c.r = lerp(c.r, c.r*15.0*(1 - c.a), value);
c.g = lerp(c.g, c.g*10.0*(1 - c.a), value);
c.b = lerp(c.b, c.b*5.0*(1 - c.a), value);
c.rgb += txt.rgb*value;
c.rgb = lerp(saturate(c.rgb),c.rgb,HDR);
return c;
}
float4 ThresholdSmooth(float4 txt, float value, float smooth)
{
float l = (txt.x + txt.y + txt.z) * 0.33;
txt.rgb = smoothstep(value, value + smooth, l);
return txt;
}
float4 DisplacementUV(float2 uv,sampler2D source,float x, float y, float value)
{
return tex2D(source,lerp(uv,uv+float2(x,y),value));
}
float2 KaleidoscopeUV(float2 uv, float posx, float posy, float number)
{
uv = uv - float2(posx, posy);
float r = length(uv);
float a = abs(atan2(uv.y, uv.x));
float sides = number;
float tau = 3.1416;
a = fmod(a, tau / sides);
a = abs(a - tau / sides / 2.);
uv = r * float2(cos(a), sin(a));
return uv;
}
float4 frag (v2f i) : COLOR
{
float2 RotationUV_1 = RotationUV(i.texcoord,RotationUV_Rotation_1,RotationUV_Rotation_PosX_1,RotationUV_Rotation_PosY_1,RotationUV_Rotation_Speed_1);
float2 AnimatedOffsetUV_2 = AnimatedOffsetUV(RotationUV_1,AnimatedOffsetUV_X_2,AnimatedOffsetUV_Y_2,AnimatedOffsetUV_ZoomX_2,AnimatedOffsetUV_ZoomY_2,AnimatedOffsetUV_Speed_2);
float4 NewTex_2 = tex2D(_NewTex_2,AnimatedOffsetUV_2);
float4 _Destroyer_1 = DestroyerFX(NewTex_2,AnimatedOffsetUV_2,_Destroyer_Value_1,_Destroyer_Speed_1,0);
float4 Darkness_1 = Darkness(_Destroyer_1,_Darkness_Fade_1);
float2 AnimatedOffsetUV_1 = AnimatedOffsetUV(RotationUV_1,AnimatedOffsetUV_X_1,AnimatedOffsetUV_Y_1,AnimatedOffsetUV_ZoomX_1,AnimatedOffsetUV_ZoomY_1,AnimatedOffsetUV_Speed_1);
float4 NewTex_1 = tex2D(_NewTex_1,AnimatedOffsetUV_1);
float2 KaleidoscopeUV_1 = KaleidoscopeUV(AnimatedOffsetUV_1,KaleidoscopeUV_PosX_1,KaleidoscopeUV_PosY_1,KaleidoscopeUV_Number_1);
float4 _Displacement_1 = DisplacementUV(KaleidoscopeUV_1,_MainTex,NewTex_1.r*NewTex_1.a,NewTex_1.g*NewTex_1.a,_Displacement_Value_1);
float4 _Burn_1 = BurnFX(_Displacement_1,KaleidoscopeUV_1,_Burn_Value_1,_Burn_Speed_1,0);
_Burn_1.a = NewTex_1.a;
NewTex_1 = lerp(NewTex_1,NewTex_1*NewTex_1.a + _Burn_1*_Burn_1.a,_Add_Fade_1);
float4 _ThresholdSmooth_1 = ThresholdSmooth(NewTex_1,_ThresholdSmooth_Value_1,_ThresholdSmooth_Smooth_1);
float4 TintRGBA_1 = TintRGBA(_ThresholdSmooth_1,_TintRGBA_Color_1);
Darkness_1 = lerp(Darkness_1,Darkness_1*Darkness_1.a + TintRGBA_1*TintRGBA_1.a,_Add_Fade_2);
float4 FinalResult = Darkness_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}
