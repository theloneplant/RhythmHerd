Shader "Rhythm Herd/Circle"
{
    Properties
    {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_InnerColor ("Inner Color", Color) = (1, 1, 1, 1)
		_Radius ("Radius", Float) = 0.5
		_Thickness ("Thickness", Float) = 0.2
		_AntialiasWidth ("Antialias Width", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

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

			float4 _Color;
			float4 _InnerColor;
			float _Radius;
			float _Thickness;
			float _AntialiasWidth;

            fixed4 frag (v2f i) : SV_Target
            {
				float distance = length(i.uv - 0.5) * 2;
				distance = distance + 1 - _Radius;
				float inner = 1 - (1 - distance - _Thickness) / _AntialiasWidth;
				float outer = (1 - distance) / _AntialiasWidth;
				float circle = clamp(outer, 0, 1) * clamp(inner, 0, 1);
				return float4(1, 1, 1, outer) * lerp(_InnerColor, _Color, clamp(inner, 0, 1));
            }
            ENDCG
        }
    }
}
