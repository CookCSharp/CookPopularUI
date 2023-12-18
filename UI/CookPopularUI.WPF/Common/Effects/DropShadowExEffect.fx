sampler2D Texture1Sampler : register(S0);
float Angle : register(C0);
float Depth : register(C1);
float4 Color : register(C2);
float Opacity : register(C3);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 c = 0;
    float rad = Angle * 0.0174533f;
    float xOffset = cos(rad) * Depth;
    float yOffset = sin(rad) * Depth;

    uv.x += xOffset;
    uv.y += yOffset;
    c = tex2D(Texture1Sampler, uv);
    c.rgb = Color.rgb * c.a * Opacity;
    c.a = c.a * Opacity;
    return c;
}