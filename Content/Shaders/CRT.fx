sampler2D ScreenTexture;

float Time;
float2 Shake;

// Noise function.
float rand_1_05(in float2 uv)
{
    float2 noise = (frac(sin(dot(uv, float2(12.9898, 78.233) * 2.0)) * 43758.5453));
    return abs(noise.x + noise.y) * 0.5;
}

float2 crt(float2 coord, float bend)
{
	// put in symmetrical coords
    coord = (coord - 0.5) * 1.9;

    coord *= 1.1;

	// deform coords
    coord.x *= 1.0 + pow((abs(coord.y) / bend), 2.0);
    coord.y *= 1.0 + pow((abs(coord.x) / bend), 2.0);

	// transform back to 0.0 - 1.0 space
    coord = (coord / 2.0) + 0.5;

    return coord;
}

// Returns nearest of two values to x.
float nearest(float x, float a, float b)
{
    return (x - a < b - x ? a : b);
}

// Returns nearest point on the 0-1 border of uv coordinates.
float2 nearest_border(float2 vec)
{  
    float nx = nearest(vec.x, 0, 1);
    float ny = nearest(vec.y, 0, 1);
    float2 x_ny = float2(clamp(vec.x, 0, 1), ny);
    float2 nx_y = float2(nx, clamp(vec.y, 0, 1));
    return (distance(vec, x_ny) < distance(vec, nx_y)) ? x_ny : nx_y;
}

float4 MainPS(float4 position: SV_Position, float2 texcoord: TEXCOORD0) : COLOR0
{
    // The screen shake applies directly on top of the UV to give
    // the illusion of a moving camera.
    float2 texel = float2(1.0f / 1280.0f, 1.0f / 720.0f);
    float2 uv = texcoord + Shake;
    
    // Apply a crt screen deform and discard outside particles.
    // Draw a border smoothly around the edge.
    float2 test_uv = crt(uv, 3.2f);
    float2 clamped = nearest_border(test_uv);
    float dist = distance(clamped, test_uv);
    float3 border = float3(1,1,1) * 0.3f * pow(1.0f / (1.0f + dist), 256);
    if (test_uv.x < 0 || test_uv.x > 1 || test_uv.y < 0 || test_uv.y > 1)
    {
        return float4(border, 1);
    }
    
    // The scanline will shift the uv values on the x axis.
    // We can control the intensity, rate of decay and speed here.
    float posy = test_uv.y * 720;
    float scanline = 1 - ((720 - posy + 70.0f * Time) % 720) / 720.0f;
    uv.x += pow(scanline, 16) * texel.x * 14.0f;
    uv = crt(uv, 3.2f);

    // Blend the three pixels for a blurry effect.
    float3 pixel1 = tex2D(ScreenTexture, uv).rgb;
    float3 pixel2 = tex2D(ScreenTexture, uv + texel).rgb;
    float3 pixel3 = tex2D(ScreenTexture, uv - texel*2).rgb;
    float3 color = (pixel1 + pixel2 + pixel3) * 0.4f + border;
    
    // Apply the grid CRT effect.
    if ((720 - posy +Time * 6.0f) % 6 < 3)
    {
       color *= 0.7f + 0.2f * abs(sin(Time * 4.0f));
    }

    // Apply the scanline pass.
    color += 0.1f * pow(scanline, 16);
    
    // Apply some noise.
    color += (0.05f + 0.03f * sin(Time)) * rand_1_05(float2(Time + uv.y, uv.x - Time));
    
    // Tone mapping
    color = saturate(color);
    return float4(color, 1.0);
}

technique CRT
{
    pass P0
    {
        PixelShader = compile ps_3_0 MainPS();
    }
}