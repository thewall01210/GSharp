
struct VShader_Input
{
  float4 color;
};

float4 VShader(VShader_Input input : INPUT) : SV_POSITION
{
	return input.color;
}

float4 PShader(float4 color : SV_POSITION) : SV_Target
{
	return float4(color[0], color[1], color[2], 1.0f);
}