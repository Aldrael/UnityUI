Shader "Mobile/Particles/Outlined Sprite" {
Properties {
_EmisColor ("Emissive Color", Color) = (.2,.2,.2,0)
_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
Blend SrcAlpha OneMinusSrcAlpha
Cull Off ZWrite Off Fog { Color (0,0,0,0) }

Lighting Off

Material { Emission [_EmisColor] }
ColorMaterial AmbientAndDiffuse

SubShader {
Pass {

AlphaTest Equal 1
SetTexture [_MainTex] 
{	
combine texture * primary
}
}

Pass {

ZTest Less
AlphaTest NotEqual 1

SetTexture [_MainTex] 
{	
combine texture * primary
}
SetTexture [_MainTex] 
{
constantColor [_EmisColor]
combine previous * constant
}
}

}
}
}