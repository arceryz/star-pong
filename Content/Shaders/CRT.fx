sampler2D ScreenTexture;

float Time;
float2 Shake;

float4 MainPS(float4 position: SV_Position, float2 texcoord: TEXCOORD0) : COLOR0
{
    float2 uv = texcoord + Shake;
    float4 pixel1 = tex2D(ScreenTexture, uv);
    
    float2 texel = float2(1.0f / 1280.0f, 1.0f / 720.0f);
    float4 pixel2 = tex2D(ScreenTexture, uv + texel);
    float4 pixel3 = tex2D(ScreenTexture, uv - texel*2);
    
    float4 color = (pixel1 + pixel2 + pixel3) * 0.4f;
    
    if ((position.y + Time * 6.0f) % 4 < 2)
    {
        color *= 0.5f + 0.2f * abs(sin(Time * 7.0f));
    }

    return color;
}

technique CRT
{
    pass P0
    {
        PixelShader = compile ps_3_0 MainPS();
    }
}