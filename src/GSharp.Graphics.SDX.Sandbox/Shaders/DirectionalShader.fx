﻿float4 VShader(float4 position : POSITION) : SV_POSITION
{
	return position;
}

float4 PShader(float4 position : SV_POSITION) : SV_Target
{
	return float4(0.5f, 0.2f, 0.7f, 1.0f);
}