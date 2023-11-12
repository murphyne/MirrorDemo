Shader "Unlit/Portal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // The material is completely non-transparent and is rendered at the same time as the other opaque geometry.
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            CGPROGRAM

            //define vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //include useful shader functions
            #include "UnityCG.cginc"

            //the object data that's put into the vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
            };

            //the data that's used to generate fragments and can be read by the fragment shader
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
            };

            // Texture and transforms of the texture.
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            // Vertex shader.
            v2f vert(const appdata v)
            {
                v2f o;
                // Convert the vertex positions from object space to clip space.
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            // Fragment shader.
            float4 frag(v2f i) : SV_TARGET
            {
                float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;

                // Flip the texture horizontally.
                // textureCoordinate.x = 1.0f - textureCoordinate.x;
                // return float4(textureCoordinate.xy,0,0);

                // Enable texture tiling and offset in material.
                // Not sure whether this must be in Portal shader.
                textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
                // return float4(textureCoordinate.xy,0,0);

                float4 col = tex2D(_MainTex, textureCoordinate);
                return col;
            }

            ENDCG
        }
    }
}
