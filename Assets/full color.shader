
// Reference: 
// Flurgle 2017, Forum How do I get the Skybox Lighting settings value "Ambient Intensity" in the frag/vert?,
// accessed 1 Sep 2017, <https://forum.unity3d.com/threads/how-do-i-get-the-skybox-lighting-settings
//-value-ambient-intensity-in-the-frag-vert.454467/> 


Shader "Tim/full color"
{
	 Properties {
	 	// base color, white
        _Color("Main Color", Color) = (1,1,1,0.5)
    }
    SubShader {
        Pass {
        	// choosing the lightmode for some unity function
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"


            struct vertin {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };
 
            struct vertout {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float4 WorldPosition : TEXCOORD1;
                float4 color : COLOR;
            };

            // this is the vertex shader
            vertout vert(vertin i) {
                vertout o;
                float4 nor = float4(i.normal, 0.0);
                o.pos = UnityObjectToClipPos(i.vertex);
                o.WorldPosition = mul(unity_ObjectToWorld, i.vertex);
                o.normal = mul(unity_ObjectToWorld, nor).xyz;
                o.color = i.color;
                return o;
            }
         
 			fixed4 _Color;
            float4 _LightColor0;
            // this is the fragment shader
            fixed4 frag(vertout i) : COLOR {
           		// this part may contain code from Flurgle 2017
           		// reference is above and in readme file
                float3 ambient = unity_AmbientSky.xyz * _LightColor0.rgb * _Color;

                float3 thenormal = normalize(i.normal);
                // direction where light comes from 
                float3 thelight = normalize(_WorldSpaceLightPos0.xyz);
                // camera view 
                float3 camera = normalize(_WorldSpaceCameraPos - i.WorldPosition.xyz);
                // diffuse light
                float nl = dot(thenormal, thelight);
                float3 diffuse = _LightColor0.rgb * max(0.0, nl);

                // specular part of the shader 
                float3 specular;
                // the angle between the normal and the light 
                // this part may contain code from Flurgle 2017
           		// reference is above and in readme file
                if(dot(thenormal, thelight) < 0.0) {
          			// when sun falls
          			// no specular 
                    specular = float3(0.0, 0.0, 0.0);
                } else {
                	// this part may contain code from Flurgle 2017
           			// reference is above and in readme file
           			// specular is calculated by the reflection of the surface of the light and user view
                    specular = _LightColor0.rgb * _Color.rgb * pow(max(0.0, dot(reflect(-thelight, thenormal), camera)), 2);
                }
               // return the all effects
                float3 effect = ambient + diffuse + specular;
 
                return  i.color * float4(effect, 1) * _Color;
            }
           
            ENDCG
        }
    }
}