
struct VShader_Input
{
  float4 position : POSITION;
  float4 color : COLOR;
  float4 normal : NORMAL;
};

struct PShader_Input
{
  float4 position : SV_POSITION;
  float4 color : PCOLOR;
};

PShader_Input VShader(VShader_Input input)
{
  PShader_Input output = (PShader_Input)0;

  float light = dot(input.normal, normalize(float3(1.0, 1.0, 1.0)));

  output.position = input.position;
  output.color = input.color * clamp(light, 0.05, 1.0);

	return output;
}

float4 PShader(PShader_Input input) : SV_Target
{
	return float4(input.color[0], input.color[1], input.color[2], 1.0);
	// return float4(0.5, 0.5, 0.5, 1.0);
}
