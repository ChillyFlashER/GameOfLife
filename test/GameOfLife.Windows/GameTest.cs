namespace GameOfLife
{
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using System;
    using System.Windows.Forms;
    using Buffer = SharpDX.Direct3D11.Buffer;
    using Color = SharpDX.Color;
    using MapFlags = SharpDX.Direct3D11.MapFlags;

    public class SharpDXCubes : DeferredGame
    {
        const int MaxNumberOfCubes = 64;
        const float viewZ = 5.0f;

        private Matrix View, Projection, viewProj;

        private InputLayout layout;
        private VertexShader vertexShader;
        private PixelShader pixelShader;
        private Buffer contantBuffer, verticesBuffer;

        protected override void LoadContent()
        {
            var bytecode = ShaderBytecode.CompileFromFile("MultiCube.fx", "VS", "vs_4_0");
            vertexShader = new VertexShader(Device, bytecode);

            layout = new InputLayout(Device, ShaderSignature.GetInputSignature(bytecode), new[] 
            { 
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0), 
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0) 
            });

            bytecode.Dispose();

            bytecode = ShaderBytecode.CompileFromFile("MultiCube.fx", "PS", "ps_4_0");
            pixelShader = new PixelShader(Device, bytecode);
            bytecode.Dispose();

            verticesBuffer = Buffer.Create(Device, BindFlags.VertexBuffer, new[] 
            { 
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), // Front 
                new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), 
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), 
 
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), // BACK 
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), 
 
                new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), // Top 
                new Vector4(-1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), 
 
                new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), // Bottom 
                new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4(-1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), 
                new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), 
 
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), // Left 
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), 
                new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), 
 
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), // Right 
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), 
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), 
            });

            contantBuffer = new Buffer(Device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            this.View = Matrix.LookAtLH(new Vector3(0, 0, -viewZ), new Vector3(0, 0, 0), Vector3.UnitY);
            this.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, Form.ClientSize.Width / (float)Form.ClientSize.Height, 0.1f, 100.0f);
            this.viewProj = Matrix.Multiply(View, Projection);
        }

        protected override void Update(float elapsedTime)
        {

        }

        protected override void SetPartitionContext(int contextIndex, DeviceContext context)
        {
            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(verticesBuffer, Utilities.SizeOf<Vector4>() * 2, 0));
            context.VertexShader.SetConstantBuffer(0, contantBuffer);
            context.VertexShader.Set(vertexShader);
            context.Rasterizer.SetViewport(0, 0, Form.ClientSize.Width, Form.ClientSize.Height);
            context.PixelShader.Set(pixelShader);
            context.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
        }

        protected override CommandList DrawPartition(int contextIndex, DeviceContext context, float elapsedTime)
        {
            if (contextIndex == 0)
            {
                context.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
                context.ClearRenderTargetView(RenderTargetView, Color.CornflowerBlue);
            }

            int deltaCube = MaxNumberOfCubes / Threads;
            int nextStartingRow = deltaCube * contextIndex;

            int toRow = nextStartingRow + deltaCube;
            if (toRow > MaxNumberOfCubes)
                toRow = MaxNumberOfCubes;

            int count = MaxNumberOfCubes;
            float divCubes = (float)count / (viewZ - 1);

            var rotateMatrix = Matrix.Scaling(1.0f / count)
                * Matrix.RotationX(elapsedTime) * Matrix.RotationY(elapsedTime * 2) * Matrix.RotationZ(elapsedTime * .7f);

            for (int y = nextStartingRow; y < toRow; y++)
            {
                for (int x = 0; x < count; x++)
                {
                    rotateMatrix.M41 = (x + .5f - count * .5f) / divCubes;
                    rotateMatrix.M42 = (y + .5f - count * .5f) / divCubes;

                    Matrix worldViewProj;
                    Matrix.Multiply(ref rotateMatrix, ref viewProj, out worldViewProj);
                    worldViewProj.Transpose();

                    context.UpdateSubresource(ref worldViewProj, contantBuffer);

                    context.Draw(36, 0);
                }
            }

            return context.FinishCommandList(false);
        }
    }
}
