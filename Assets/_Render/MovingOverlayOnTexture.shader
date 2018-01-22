Shader "Custom/MovingOverlayOnTexture" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Rays texture", 2D) = "white" {}
		_SecondaryTex("Overlay Texture Color (RGB) Alpha (A)", 2D) = "white" {}
		_speed("Speed", Float) = 0.2
	}

		Category{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha One

			Cull Off Lighting Off ZWrite Off Fog{ Color(0,0,0,0) }

			BindChannels{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

			SubShader{
				Pass{
					CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

					uniform sampler2D _MainTex;
					uniform sampler2D _SecondaryTex;
					fixed4 _Color;
					float _speed;

					float4 frag(v2f_img i, v2f_img j) : COLOR{
						i.uv.x += (_Time*_speed);
						i.uv.y -= (_Time*_speed);
						return tex2D(_MainTex, j.uv) * tex2D(_SecondaryTex, i.uv) *_Color;
					}
					ENDCG
				}

			/*Pass{
				CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag

		#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				float _speed;

				float4 frag(v2f_img i) : COLOR{
					i.uv.x += sin(_Time*_speed);
					return tex2D(_MainTex, i.uv);
				}
					ENDCG
				}*/
				}
		}
}