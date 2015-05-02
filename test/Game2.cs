namespace GameOfLife
{
    using GameOfLife.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Diagnostics;

    class Game2 : Game
    {
        public Stopwatch stopwatch;

#if NETFX_CORE
        public Windows.System.Threading.ThreadPoolTimer timer;
#else
        public System.Timers.Timer timer;
#endif

        private InputState inputState;
        private Camera camera;

        private DrawableUnlimitedSimulation simulation;

        public Game2()
        {
            var graphics = new GraphicsDeviceManager(this);

            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void LoadContent()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            inputState = new InputState();
            camera = new Camera(GraphicsDevice);
            camera.MinZoom = 0.2f;

            simulation = new DrawableUnlimitedSimulation(GraphicsDevice);
            simulation.VisualScale = 5;

            simulation.SetCell(0, 0, true);

            stopwatch = new Stopwatch();

            const float timerTick = 100;

#if NETFX_CORE
            timer = Windows.System.Threading.ThreadPoolTimer.CreatePeriodicTimer(
                e => OnTimer(), System.TimeSpan.FromMilliseconds(timerTick));
#else
            timer = new System.Timers.Timer(timerTick);
            timer.Elapsed += (s, e) => OnTimer();
            timer.Start();
#endif
        }

        async void OnTimer()
        {
            stopwatch.Restart();
            await simulation.StepAsync();
            stopwatch.Stop();

            Debug.WriteLine(string.Format("{0} ms", stopwatch.ElapsedMilliseconds), "Profile");
        }

        protected override void Update(GameTime gameTime)
        {
            inputState.Update();
            camera.OnInput((float)gameTime.ElapsedGameTime.TotalMilliseconds, inputState);

            var mousePos = inputState.Mouse.Position.ToVector2();
            var worldPos = Vector2.Transform(mousePos, Matrix.Invert(camera.View));

            if (inputState.Mouse.LeftButton == ButtonState.Pressed)
            {
                simulation.SetCell(true);
            }

            if (inputState.Mouse.RightButton == ButtonState.Pressed)
            {
                simulation.SetCell(false);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 40, 40));
            simulation.Draw(GraphicsDevice, camera.View);
        }
    }
}
