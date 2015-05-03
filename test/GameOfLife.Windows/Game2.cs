namespace GameOfLife
{
	using SharpDX;
	using SharpDX.Direct3D;
	using SharpDX.Direct3D11;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows.Forms;
	using Buffer = SharpDX.Direct3D11.Buffer;
	using Color = SharpDX.Color;

	public class Partition
	{
		public BufferDescription Description;
		public Buffer Buffer;
		public VertexBufferBinding Binding;

		private RectVertex[] data;
		private bool changed;

		public Partition(SharpDX.Direct3D11.Device device, int bufferSize)
		{
			this.data = new RectVertex[bufferSize];
			this.changed = true;

			this.Description = new BufferDescription(Utilities.SizeOf<RectVertex>() * bufferSize, ResourceUsage.Dynamic, 
				BindFlags.VertexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
			this.Buffer = Buffer.Create(device, this.data, this.Description);
			this.Binding = new VertexBufferBinding(this.Buffer, Utilities.SizeOf<RectVertex>(), 0);
		}

		public void SetData(RectVertex[] vertices)
		{
			this.changed = true;
			this.data = vertices;
		}

		public void Draw(DeviceContext context)
		{
			if (this.changed)
			{
				DataStream stream;

				var dataBox = context.MapSubresource(this.Buffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out stream);
				stream.WriteRange(data);

				context.UnmapSubresource(this.Buffer, 0);
				stream.Dispose();

				this.changed = false;
			}

			context.InputAssembler.SetVertexBuffers(0, this.Binding);
			context.Draw(data.Length, 0);
		}
	}

	public class Game2 : Game
	{
		/// <summary> Cell size in pixels </summary>
		const float cell_size = 5;
		/// <summary> Simulation width </summary>
		const int width = 124;

		private MultiCellEffect effect;
		private Buffer contantBuffer;

		private List<RectVertex> rlist = new List<RectVertex>();
		private Partition partition;

		private LimitedSimulation simulation;
		private Stopwatch stopwatch;

		private Vector2 position = Vector2.Zero;
		private Vector2 startMouse = Vector2.Zero;
		private Vector2 startPosition = Vector2.Zero;
		private Vector2 pixelSize;

		protected override void LoadContent()
		{
			this.pixelSize = ConvertPixelToClip(Vector2.One);

			this.effect = new MultiCellEffect(this.Device);

			this.simulation = new LimitedSimulation(width, width);
			this.stopwatch = new Stopwatch();

			// Create a random grid
			var rd = new Random();
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < width; y++)
				{
					int index = (y * width) + x;
					if (rd.Next(0, 10) > 5)
					{
						this.simulation.Grid.SetCell(x, y, true);
					}
				}
			}

			// Create graphcis partition (grid to graphics)
			this.partition = new Partition(Device, width * width);
			UpdateSimulationGraphics();

			// TODO: Matrix
			contantBuffer = new Buffer(Device, Utilities.SizeOf<Vector4>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

			Form.MouseDown += Form_MouseDown;
			Form.MouseMove += Form_MouseMove;
			Form.KeyDown += Form_KeyDown;
		}

		void Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				stopwatch.Restart();
				simulation.Step();
				stopwatch.Stop();

				Debug.WriteLine(string.Format("Simulation: {0} ms, {1} ticks", stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks));

				UpdateSimulationGraphics();
			}
		}

		void UpdateSimulationGraphics()
		{
			rlist.Clear();

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < width; y++)
				{
					if (simulation.Grid.GetCell(x, y))
					{
						var rect = new RectVertex();
						rect.pos = new Vector4(ConvertPixelToClip(new Vector2(cell_size * x, cell_size * y)), 0.0f, 1.0f);
						rect.siz = ConvertPixelToClip(new Vector2(cell_size, cell_size));
						rect.col = new Vector4(0, 0, 0, 1);
						rlist.Add(rect);
					}
				}
			}

			partition.SetData(rlist.ToArray());
		}

		void Form_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				startMouse.X = e.X;
				startMouse.Y = e.Y;
				startPosition.X = position.X;
				startPosition.Y = position.Y;
			}
			else if (e.Button == MouseButtons.Left)
			{
				// TODO: Mouse raycast
			}
		}

		void Form_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				position = new Vector2(startPosition.X + (e.X - startMouse.X) * pixelSize.X,
									   startPosition.Y - (e.Y - startMouse.Y) * pixelSize.Y);
			}
		}

		protected override void Draw(DeviceContext context, float elapsedTime)
		{
			context.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList; // TODO: I should move this to the effect too \?
			context.VertexShader.SetConstantBuffer(0, contantBuffer);
			context.Rasterizer.SetViewport(0, 0, Form.ClientSize.Width, Form.ClientSize.Height);
			context.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);

			// Apply effect
			this.effect.Apply(context);

			// Clear screen
			context.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
			context.ClearRenderTargetView(RenderTargetView, Color.CornflowerBlue);

			// Set camera position
			Vector4 pos = new Vector4(position, 0, 0);
			context.UpdateSubresource(ref pos, contantBuffer);

			// Draw grid
			this.partition.Draw(context);
		}

		static void Main(string[] args)
		{
			using (var game = new Game2())
			{
				game.Run();
			}
		}
	}
}
