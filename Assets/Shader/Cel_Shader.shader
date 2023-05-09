Shader "Unlit/Cel_Shader"
{
    Properties
    {
    [HDR]
        _Colour ("Colour", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        
    [HDR]
    _AmbientColour ("Ambient Colour", Color) = (0.4, 0.4,0.4,1)
    
    [HDR]
    _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
    _Glossiness("Glossiness", Float) = 32

    [HDR]
    _RimColor("Rim Color", Color) = (1,1,1,1)
    _RimAmount("Rim Amount", Range(0, 1)) = 0.716
    _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags 
        {
            "LightMode"     = "ForwardBase"
            "PassFlags"     = "OnlyDirectional"
            "RenderType"    = "Transparent" 
        }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile
            // make fog work
            #pragma multi_compile_fog
            

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;

                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AmbientColour;
            float _Glossiness;
            float4 _SpecularColor;
            float4 _Colour;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float NDotL = dot(_WorldSpaceLightPos0, normal);
                
                float3 viewDir = normalize(i.viewDir);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);

                float shadow = SHADOW_ATTENUATION(i);
                //float lightIntensity = smoothstep(0, 0.01, NDotL);
                float lightIntensity = smoothstep(0, 0.01, NDotL * shadow);
                float4 light = lightIntensity * _LightColor0;

                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
                
                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor;

                float4 rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(NDotL, _RimThreshold);
                rimIntensity = smoothstep (_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                
                // sample the texture
                fixed4 _tex = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return _Colour * _tex * (_AmbientColour + light + specular + rimIntensity);
               
            }

            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
