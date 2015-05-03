namespace GameOfLife
{
	using SharpDX;
	using SharpDX.D3DCompiler;
	using SharpDX.Direct3D11;
	using SharpDX.DXGI;

	/// <summary>
	/// Vertex data.
	/// </summary>
	public struct RectVertex
	{
		public Vector4 pos;
		public Vector2 siz; // TODO: I could remove size and place it in the constant buffer
		public Vector4 col;
	}

	public sealed class MultiCellEffect
	{
		/// <summary> Gets the shader layout. </summary>
		public InputLayout Layout { get; private set; }

		/// <summary> Gets the vertex shader. </summary>
		public VertexShader VertexShader { get; private set; }

		/// <summary> Gets the geometry shader </summary>
		public GeometryShader GeometryShader { get; private set; }

		/// <summary> Gets the pixel shader. </summary>
		public PixelShader PixelShader { get; private set; }

		public MultiCellEffect(SharpDX.Direct3D11.Device device)
		{
			// Compile Vertex Shader
			var bytecode = CustomEffect.VertexShaderByteCode;

			this.VertexShader = new VertexShader(device, bytecode);
			this.Layout = new InputLayout(device, ShaderSignature.GetInputSignature(bytecode), new[] 
			{ 
				new InputElement("ANCHOR", 0, Format.R32G32B32A32_Float, 0, 0), 
				new InputElement("DIMENSIONS", 0, Format.R32G32_Float, 16, 0), 
				new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0) 
			});

			// Compile Geometry Shader
			bytecode = CustomEffect.GeometryShaderByteCode;
			this.GeometryShader = new GeometryShader(device, bytecode);

			// Compile Pixel Shader
			bytecode = CustomEffect.PixelShaderByteCode;
			this.PixelShader = new PixelShader(device, bytecode);
		}

		public void Apply(DeviceContext context)
		{
			// TODO: I should apply the constant buffer here too

			context.InputAssembler.InputLayout = this.Layout;
			context.VertexShader.Set(this.VertexShader);
			context.GeometryShader.Set(this.GeometryShader);
			context.PixelShader.Set(this.PixelShader);
		}
	}
}
