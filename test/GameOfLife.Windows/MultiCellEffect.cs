namespace GameOfLife
{
	using SharpDX;
	using SharpDX.D3DCompiler;
	using SharpDX.Direct3D11;
	using SharpDX.DXGI;

	public struct RectVertex
	{
		public Vector4 pos;
		public Vector2 siz; // TODO: I could remove the size 
		public Vector4 col;
	}

	public sealed class MultiCellEffect
	{
		/// <summary> </summary>
		public InputLayout Layout { get; private set; }

		/// <summary> </summary>
		public VertexShader VertexShader { get; private set; }

		/// <summary> </summary>
		public GeometryShader GeometryShader { get; private set; }

		/// <summary> </summary>
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
	}

}
