Shader "Joel/DiffuseAlpha"
{
    Properties
    {
        _Color ("Color Tint", Color) = (1,1,1,1)
        _MainTex ("Color (RGB) Alpha (A)", 2D) = "white"
    }
 
    Category
    {
        ZWrite Off
        Cull Back
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "Queue" = "Transparent"}
 
        SubShader
        {
            Pass
            {
                SetTexture [_MainTex]
                {
                    ConstantColor [_Color]
                    Combine Texture * Constant
                }
            }
        } 
    }
}