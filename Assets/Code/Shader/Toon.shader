Shader "SAE/Toon " // so fäng jede datei an
{
      // cel shader
     // Blinnn Phong
     // ambient + diffuse + specular
     // diffuse = lichtfarbe zusammen mit materialfarbe oin abhängigkeit der lichtrichtung
    Properties
    {
        _Color("Color Value", Color) = (1,1,1,0.5)
        _UnlitColor("Unlit Color Value", Color) = (0.6,0.6,0.6,0.5)

        _OutlineColor("Outline Color Value", Color) = (0,0,0,0.5)
        _LitOutlineThikness("L Outline Thickness", Range(0,1)) = 0.1
        _UnlitOutlineThikness("U Outline Thickness", Range(0,1)) = 0.4
        _DiffuseThreshhold("DiffThresh",Range(0,1)) = 0.1
        

        _SpecColor("Spec Color Value", Color) = (1,1,1,1)
        _Shiny("Shinyness", float) = 8
    }

    SubShader 
    {
        Pass // 1
        {
            Tags{"LightMode" = "ForwardBase"}

            CGPROGRAM
            
            //------------------- start cg shader code  ---------------
            #pragma vertex VS
            #pragma fragment PS

            #include "UnityCG.cginc" 
            uniform float4 _LightColor0;

            
            uniform float4 _Color;
            uniform float4 _UnlitColor;
            uniform float4 _OutlineColor;
            uniform float _LitOutlineThikness;
            uniform float _UnlitOutlineThikness;
            uniform float _DiffuseThreshhold;
            uniform float4 _SpecColor;
            uniform float _Shiny;

            struct VS_IN
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

            
            struct VS_OUT
            {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };

            
            
            VS_OUT VS(VS_IN input)
            {
                VS_OUT output;

                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInvers = unity_WorldToObject;

                output.normalDir = normalize(
                    mul(float4(input.normal,0), modelMatrixInvers).xyz
                ); // für dx11

                // = UnityObjectToWorldNormal(input.normal) -> gibt es in dx11 nicht
                // -new- vorberechnung
                output.posWorld = mul(modelMatrix,input.pos);
                output.pos = UnityObjectToClipPos(input.pos);
                
                return output;
            }

            // für jeden vektor eine koordinaten
            float4 PS(VS_OUT input) : COLOR
            {
                float3 normalDir = normalize(input.normalDir);
                
                float3 viewDir = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
                float3 lightDir;
                float attenuation;


                
                if(_WorldSpaceLightPos0.w == 0) // Directional light
                {
                    attenuation = 1;
                    lightDir = normalize(_WorldSpaceLightPos0.xyz);
                }
                else                             // point oder spott light
                {
                    float3 vertexToLight = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
                    
                    attenuation = 1 / length(vertexToLight); // diese ist eine linear oder mit pow um es realistischer zu machen
                    lightDir = normalize(vertexToLight);
                }

                // DEFAULT: UNLIT
                float4 fragColor = _UnlitColor.rgba;
                
                // low prio: diffuse
                float c = attenuation * max(0,dot(normalDir, lightDir));
                
                if( c >= _DiffuseThreshhold )
                {
                    fragColor = _LightColor0.rgba * _Color.rgba;
                }

                // hight prio : outline

                float4 lerpss = lerp(_UnlitOutlineThikness, _LitOutlineThikness, max(0,dot(normalDir, lightDir)));
                float4 a = dot(viewDir,normalDir);
                
                if ( all(a < lerpss))
                {
                     fragColor = _LightColor0.rgba * _OutlineColor.rgba;
                }


                // highest prio: highlights

                
                a = dot(normalDir,lightDir);
                float b = attenuation * pow(max(0,dot(reflect(-lightDir,normalDir),viewDir)), _Shiny);
                
                if( all(a >  0)
                 && all(b> 0.5)
                 ) // light on wrong side
                {
               
                     fragColor = _SpecColor.a * _LightColor0.rgba * _SpecColor.rgba
                    + (1- _SpecColor.a) * fragColor; 
                }
                return fragColor; // letzter alpha wert
            }
            
            //------------------- ende shader code  ---------------
            ENDCG
        }

        Pass // 2
        {
            Tags{"LightMode" = "ForwardAdd"}
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            
            //------------------- start cg shader code  ---------------
            #pragma vertex VS
            #pragma fragment PS

            #include "UnityCG.cginc" 
            uniform float4 _LightColor0;

            
            uniform float4 _Color;
            uniform float4 _UnlitColor;
            uniform float4 _OutlineColor;
            uniform float4 _LitOutlineThikness;
            uniform float _UnlitOutlineThikness;
            uniform float _DiffuseThreshhold;
            uniform float4 _SpecColor;
            uniform float _Shiny;

            struct VS_IN
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

            
            struct VS_OUT
            {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };

            
            
            VS_OUT VS(VS_IN input)
            {
                VS_OUT output;

                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInvers = unity_WorldToObject;

                output.normalDir = normalize(
                    mul(float4(input.normal,0), modelMatrixInvers).xyz
                ); // für dx11

                // = UnityObjectToWorldNormal(input.normal) -> gibt es in dx11 nicht
                // -new- vorberechnung
                output.posWorld = mul(modelMatrix,input.pos);
                output.pos = UnityObjectToClipPos(input.pos);
                
                return output;
            }

            // für jeden vektor eine koordinaten
            float4 PS(VS_OUT input) : COLOR
            {
                float3 normalDir = normalize(input.normalDir);
                
                float3 viewDir = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
                float3 lightDir;
                float attenuation;


                
                if(_WorldSpaceLightPos0.w == 0) // Directional light
                {
                    attenuation = 1;
                    lightDir = normalize(_WorldSpaceLightPos0.xyz);
                }
                else                             // point oder spott light
                {
                    float3 vertexToLight = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
                    
                    attenuation = 1 / length(vertexToLight); // diese ist eine linear oder mit pow um es realistischer zu machen
                    lightDir = normalize(vertexToLight);
                }

                // DEFAULT: UNLIT
                float3 fragColor = float4(0,0,0,0);
                
               

                // highest prio: highlights
                
                if(dot(normalDir,lightDir) >  0
                 && attenuation * pow(max(0,dot(reflect(-lightDir,normalDir),viewDir)), _Shiny) > 0.5
                 ) // light on wrong side
                {
               
                     fragColor = _LightColor0.rgb * _SpecColor;
                }
                
                return float4(fragColor.rgb,0.5); // letzter alpha wert
            }
            
            //------------------- ende shader code  ---------------
            ENDCG
        }
    }
}