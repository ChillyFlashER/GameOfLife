namespace GameOfLife
{
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using SharpDX.Windows;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading.Tasks;
    using Device = SharpDX.Direct3D11.Device;

    public abstract class BaseGame : IDisposable
    {
        /// <summary> Gets the Graphics Device. </summary>
        public Device Device { get; private set; }

        /// <summary> Gets the Swap Chain. </summary>
        public SwapChain SwapChain { get; private set; }

        /// <summary> Gets the Window. </summary>
        public RenderForm Form { get; private set; }

        /// <summary> Gets the Render Target View. </summary>
        public RenderTargetView RenderTargetView { get; private set; }

        /// <summary> Gets the Depth Stencil View. </summary>
        public DepthStencilView DepthStencilView { get; private set; }

        private Stopwatch clock;

        #region Helper Methods

        public Vector2 ConvertPixelToClip(Vector2 pixelPosition)
        {
            return new Vector2(pixelPosition.X / Form.ClientSize.Width, pixelPosition.Y / Form.ClientSize.Height) * 2;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected abstract void LoadContent();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Update(float elapsedTime)
        {

        }

        protected abstract void OnInitialize();
        protected abstract void OnDraw(float elapsedTime);

        public virtual void Run()
        {
            // Create Form
            Form = new RenderForm(this.ToString());
            Form.AllowUserResizing = false;
            Form.ClientSize = new Size(1280, 780);

            // Swap Chain Desc
            var swapChainDesc = new SwapChainDescription()
            {
                BufferCount = 2,
                ModeDescription =
                    new ModeDescription(Form.ClientSize.Width, Form.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = Form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain 
            Device device;
            SwapChain swapChain;
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDesc, out device, out swapChain);

            // Swap with properties
            this.Device = device;
            this.SwapChain = swapChain;

            // Check if driver is supporting natively CommandList
            bool supportConcurentResources, supportCommandList;
            Device.CheckThreadingSupport(out supportConcurentResources, out supportCommandList);

            // Ignore all windows events 
            var factory = SwapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(Form.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer 
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(SwapChain, 0);
            this.RenderTargetView = new RenderTargetView(Device, backBuffer);

            this.LoadContent();

            // Create Depth Buffer & View 
            var depthBuffer = new Texture2D(Device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = Form.ClientSize.Width,
                Height = Form.ClientSize.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            this.DepthStencilView = new DepthStencilView(Device, depthBuffer);

            this.OnInitialize();

            this.clock = new Stopwatch();
            this.clock.Start();

            RenderLoop.Run(Form, () =>
            {
                this.Update(this.clock.ElapsedMilliseconds / 1000.0f);
                this.OnDraw(this.clock.ElapsedMilliseconds / 1000.0f);
                swapChain.Present(0, PresentFlags.None);
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO: Dispose
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString();
        }
    }

    /// <summary>
    /// Game that runs normally.
    /// </summary>
    public abstract class Game : BaseGame
    {
        /// <summary>
        /// 
        /// </summary>
        protected abstract void Draw(DeviceContext context, float elapsedTime);

        /// <inheritdoc />
        protected override void OnInitialize()
        {

        }

        /// <inheritdoc />
        protected override void OnDraw(float elapsedTime)
        {
            this.Draw(Device.ImmediateContext, elapsedTime);
        }
    }

    /// <summary>
    /// Game that runs drawing deferred.
    /// </summary>
    public abstract class DeferredGame : BaseGame
    {
        /// <summary> Gets the thread count. </summary>
        public int Threads
        {
            get { return threadCount; } // TODO: Set
        }
        private int threadCount = 4;

        private DeviceContext[] contexts;
        private DeviceContext[] preContexts;
        private CommandList[] commandLists;

        /// <summary>
        /// 
        /// </summary>
        protected abstract void SetPartitionContext(int contextIndex, DeviceContext context);

        /// <summary>
        /// 
        /// </summary>
        protected abstract CommandList DrawPartition(int contextIndex, DeviceContext context, float elapsedTime);

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            const int MaxNumberOfThreads = 4;

            this.preContexts = new DeviceContext[MaxNumberOfThreads];
            for (int i = 0; i < this.preContexts.Length; i++)
                this.preContexts[i] = new DeviceContext(Device);

            this.contexts = new DeviceContext[MaxNumberOfThreads];
            this.contexts[0] = Device.ImmediateContext;

            this.commandLists = new CommandList[MaxNumberOfThreads];
        }

        /// <inheritdoc />
        protected override void OnDraw(float elapsedTime)
        {
            Array.Copy(preContexts, contexts, contexts.Length);
            for (int i = 0; i < threadCount; i++)
            {
                var contextIndex = i;
                var context = this.contexts[i];

                this.SetPartitionContext(contextIndex, context);
            }

            var tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                var contextIndex = i;
                var context = this.contexts[i];

                tasks[i] = new Task(() => commandLists[contextIndex] = this.DrawPartition(contextIndex, context, elapsedTime));
                tasks[i].Start();
            }

            Task.WaitAll(tasks);

            for (int i = 0; i < threadCount; i++)
            {
                var commandList = commandLists[i];

                Device.ImmediateContext.ExecuteCommandList(commandList, false);

                commandList.Dispose();
                commandLists[i] = null;
            }
        }
    }
}
