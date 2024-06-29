Shader "Custom/Shelll"
{
    SubShader
    {
        Tags
        {
            "LightMode" = "ForwardBase"
        }

        Pass
        {

            Cull Off
            CGPROGRAM
            #pragma vertex vp
            #pragma fragment fp

            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"

            float hash(uint n)
            {
                n = (n << 13U) ^ n;
                n = n * (n * n * 15731U + 0x789221U) + 0x1376312589U;
                return float(n & uint(0x7fffffffU)) / float(0x7fffffff);
            }

            struct VertexData
            {
                float4 position : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            int _ShellIndex; // This is the current shell layer being operated on, it ranges from 0 -> _ShellCount 
            int _ShellCount; // This is the total number of shells, useful for normalizing the shell index
            float _ShellLength;
            // This is the amount of distance that the shells cover, if this is 1 then the shells will span across 1 world space unit
            float _Density; // This is the density of the strands, used for initializing the noise
            float _NoiseMin, _NoiseMax;
            // This is the range of possible hair lengths, which the hash then interpolates between 
            float _Thickness; // This is the thickness of the hair strand
            float _Attenuation;
            // This is the exponent on the shell height for lighting calculations to fake ambient occlusion (the lack of ambient light)
            float _OcclusionBias;
            // This is an additive constant on the ambient occlusion in order to make the lighting less harsh and maybe kind of fake in-scattering
            float _ShellDistanceAttenuation = 1;
            // This is the exponent on determining how far to push the shell outwards, which biases shells downwards or upwards towards the minimum/maximum distance covered
            float _Curvature;
            // This is the exponent on the physics displacement attenuation, a higher value controls how stiff the hair is
            float _DisplacementStrength; // The strength of the displacement (very complicated)
            float3 _ShellColor = float4(1, 1, 1, 1); // The color of the shells (very complicated)
            float3 _ShellDirection = float3(0.2,0.2,1);
            // The direction the shells are going to point towards, this is updated by the CPU each frame based on user input/movement


            v2f vp(VertexData v)
            {
                v2f i;
                float shellHeight = (float)_ShellIndex / (float)_ShellCount;
                shellHeight = pow(shellHeight, _ShellDistanceAttenuation);
                v.position.xyz += v.normal.xyz * _ShellLength * shellHeight;

                float k = pow(shellHeight, _Curvature);
                v.position.xyz += _ShellDirection *k * _DisplacementStrength;
                i.normal = normalize(UnityObjectToWorldNormal(v.normal));

                i.position = UnityObjectToClipPos(v.position);
                i.worldPos = mul(unity_ObjectToWorld, v.position + _ShellIndex);
                i.uv = v.uv;
                return i;
            }

            float4 fp(v2f i) : SV_TARGET
            {
                float2 newUV = i.uv * _Density;
                float2 localUV = frac(newUV) * 2 - 1;
                uint2 tid = newUV;
                uint seed = tid.x * 100 + tid.y;
                float shellIndex = _ShellIndex;
                float shellCount = _ShellCount;
                float rand = lerp(_NoiseMin, _NoiseMax, hash(seed));
                float h = shellIndex / shellCount;
                float localDistanceFromCenter = length(localUV);
                int outsideThickness = (localDistanceFromCenter) > (_Thickness * (rand - h));

                if (outsideThickness && _ShellIndex > 0) discard;

                float halfLambert = DotClamped(i.normal, _WorldSpaceLightPos0) * 0.5 + 0.5;
                halfLambert = halfLambert * halfLambert+0.4;

                float ambientOcclusion = pow(h, _Attenuation);
                ambientOcclusion += _OcclusionBias;
                ambientOcclusion = saturate( ambientOcclusion);
                

                return float4(_ShellColor * halfLambert * ambientOcclusion, 0);
            }
            ENDCG
        }
    }
}