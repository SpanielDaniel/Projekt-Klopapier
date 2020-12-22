Shader "MyShader/Silhouette"
{
    Properties
    {
        _Color("Farbe",Color) = (1,1,1,0.5)
        _Thickness("Rand Dicke",Float) = 1
    }

    SubShader 
    {
        Pass 
        {

            Tags{"Queue" = "Transparent"/* oder 3000 */ } // befehle an die unity engine
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            
            //------------------- start cg shader code  ---------------
            #pragma vertex VS
            #pragma fragment PS

            #include "UnityCG.cginc" // zusätzliche variabeln. zb lichtberechnung

            uniform float4 _Color;
            uniform float _Thickness;

            
            struct VS_OUT
            {
                float4 pos : SV_POSITION;
			    float3 normal : TEXCOORD0; // jokerwerte
                float3 viewDir : TEXCOORD1;
            };

            VS_OUT VS(float4 pos : POSITION,float3 normal : NORMAL)
            {
                VS_OUT output;
                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInverse = unity_WorldToObject;

                output.normal = normalize(
                    mul(float4(normal,0),modelMatrixInverse).xyz
                );

                output.viewDir = normalize(
                    _WorldSpaceCameraPos - mul(modelMatrix, pos).xyz
                );
                
                output.pos = UnityObjectToClipPos(pos);
                return output;
            }

            // für jeden vektor eine koordinaten
            float4 PS(VS_OUT input) : COLOR
            {
                float3 normalDir = normalize(input.normal);
                float3 viewDir = normalize(input.viewDir);

                float Opacity = min(1,_Color.a/abs(pow(dot(viewDir,normalDir),_Thickness)));

                //return float4
                return float4(_Color.rgb,Opacity);
            }
            
            //------------------- ende shader code  ---------------
            ENDCG
        }

        Pass // 2
        {
            // alpha belending, alpha werden zusammengerechnet
            // source : src -> erster alpha wert
            // destination : dest -> zweiter alpha wert
            ZWrite Off // zwingend notwendig
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            
            
            CGPROGRAM
            
            //------------------- start cg shader code  ---------------
            #pragma vertex VS
            #pragma fragment PS

            uniform float _Alpha; // float wert von 0 bis 1
            
            struct VS_OUT
            {
                float4 pos : SV_POSITION;
            };

            VS_OUT VS(float4 pos : POSITION)
            {
                VS_OUT output;
                output.pos = UnityObjectToClipPos(pos);
                return output;
            }

            // für jeden vektor eine koordinaten
            float4 PS(VS_OUT input) : COLOR
            {
                return float4(0.5,0,1,_Alpha);
            }
            
            //------------------- ende shader code  ---------------
            ENDCG
        }
    }
}