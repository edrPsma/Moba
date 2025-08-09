Shader "Unlit/Additive-Diffuse"
{
	Properties{
		_MainTex("Base layer (RGB)", 2D) = "white" {}
		_ScrollX("Base layer scroll speed X", Float) = 1.0
		_ScrollY("Base layer scroll speed Y", Float) = 0.0
		_RotationStart("Rotation Start", Float) = 0
		_RotationX("Base layer rotation center Y", Float) = 0.5
		_RotationY("Base layer rotation center X", Float) = 0.5
		_Rotation("Base layer rotation speed", Float) = 1.0

		_DetailTex("2nd layer (RGB)", 2D) = "white" {}
		_ScrollX2("2nd layer scroll speed X", Float) = 1.0
		_ScrollY2("2nd layer scroll speed Y", Float) = 0.0
		_RotationStart2("Rotation Start", Float) = 0
		_RotationX2("2nd layer rotation center Y", Float) = 0.5
		_RotationY2("2nd layer rotation center X", Float) = 0.5
		_Rotation2("2nd layer rotation speed", Float) = 1.0

		_Color("Color", Color) = (1,1,1,1)
		_MMultiplier("Multiplier", Float) = 2.0
		_IsGrey("IsGrey", Range(0,1)) = 0
	}

		SubShader{
			Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                //"PreviewType" = "Plane"
		        //"CurvedWorldTag"="Particles/Additive"
			    //"CurvedWorldNoneRemoveableKeywords"=""
			    //"CurvedWorldAvailableOptions"="V_CW_RANGE_FADE;"
            }

			Blend SrcAlpha One
			Cull Off
			Lighting Off ZWrite Off Fog { Mode Off }
			LOD 100

			CGINCLUDE
			#include "UnityCG.cginc"

            //CurvedWorld
            //#include "Assets/VacuumShaders/Curved World/Shaders/cginc/CurvedWorld_Base.cginc"
            //#include "Assets/VacuumShaders/Curved World/Shaders/cginc/CurvedWorld_RangeFade.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ScrollX;
			float _ScrollY;
			float _RotationStart;
			float _RotationX;
			float _RotationY;
			float _Rotation;

			sampler2D _DetailTex;
			float4 _DetailTex_ST;
			float _ScrollX2;
			float _ScrollY2;
			float _RotationStart2;
			float _RotationX2;
			float _RotationY2;
			float _Rotation2;

			float4 _Color;
			float _MMultiplier;
			half _IsGrey;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				fixed4 color : TEXCOORD1;

                UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO

				//Curved World Distance Fade
				//CURVEDWORLD_RANGE_FADE_COORDINATE(4)
			};

			float2 CalcUV(float2 uv, float tx, float ty, float rstart, float rx, float ry, float r)
			{
				float s = sin(r*_Time + rstart);
				float c = cos(r*_Time + rstart);

				float2x2 m = {c, -s, s, c};

				uv -= float2(rx, ry);
				uv = mul(uv, m);
				uv += float2(rx, ry);

				uv += frac(float2(tx, ty) * _Time);

				return uv;
			}

			v2f vert(appdata_full v)
			{
				v2f o;
                //CurveWorld
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				//Curved World Distance Fade
				//CURVEDWORLD_RANGE_FADE_SETUP(o, v.vertex)

				//V_CW_TransformPoint(v.vertex);

                o.pos = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);


				//o.pos = UnityObjectToClipPos(v.vertex);

				// base layer
				o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
				o.uv.xy = CalcUV(o.uv.xy, _ScrollX, _ScrollY, _RotationStart, _RotationX, _RotationY, _Rotation);

				// 2nd layer
				o.uv.zw = TRANSFORM_TEX(v.texcoord.xy,_DetailTex);
				o.uv.zw = CalcUV(o.uv.zw, _ScrollX2, _ScrollY2, _RotationStart2, _RotationX2, _RotationY2, _Rotation2);

				o.color = _MMultiplier * _Color * v.color;

				if (_IsGrey > 0)
				{
					half grey = dot(o.color, half3(0.299, 0.597, 0.114));
					o.color = half4(grey, grey, grey, 1);
				}



				return o;
			}
			ENDCG


			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				fixed4 frag(v2f i) : COLOR
				{
					fixed4 c = tex2D(_MainTex, i.uv.xy) * tex2D(_DetailTex, i.uv.zw) * i.color;

					if (_IsGrey > 0)
					{
						half grey = dot(c.xyz, half3(0.299, 0.597, 0.114));
						c.xyz = half3(grey, grey, grey);
					}

					return c;
				}
				ENDCG
				}
		}
}
