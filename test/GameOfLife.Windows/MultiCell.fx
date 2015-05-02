
float4 position;
//float4x4 world;

struct RECT_INPUT
{
	float4 pos : ANCHOR;
	float2 siz : DIMENSIONS;
	float4 col : COLOR;
};

struct PS_INPUT
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
};

RECT_INPUT VS(RECT_INPUT input)
{
	//input.pos = mul(input.pos, world);
	input.pos += position;
	return input;
}

[maxvertexcount(4)]
void GS(point RECT_INPUT rect[1], inout TriangleStream<PS_INPUT> triStream)
{
	PS_INPUT v;

	//bottom left
	v.pos = float4(rect[0].pos[0], rect[0].pos[1] - rect[0].siz[1], 0, 1);
	v.col = rect[0].col;
	triStream.Append(v);

	//top left
	v.pos = float4(rect[0].pos[0], rect[0].pos[1], 0, 1);
	v.col = rect[0].col;
	triStream.Append(v);

	//bottom right
	v.pos = float4(rect[0].pos[0] + rect[0].siz[0], rect[0].pos[1] - rect[0].siz[1], 0, 1);
	v.col = rect[0].col;
	triStream.Append(v);

	//top right
	v.pos = float4(rect[0].pos[0] + rect[0].siz[0], rect[0].pos[1], 0, 1);
	v.col = rect[0].col;
	triStream.Append(v);
}

float4 PS(PS_INPUT input) : SV_Target
{
	return input.col;
}
