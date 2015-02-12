
struct VShader_Input
{
  float4 position : POSITION;
};

struct PShader_Input
{
  float4 color : COLOR;
};

float4 VShader(VShader_Input input : VINPUT) : SV_POSITION
{
	return input.position;
}

float4 PShader(PShader_Input input) : SV_Target
{
	return float4(input.color[0], input.color[1], input.color[2], 1.0f);
}