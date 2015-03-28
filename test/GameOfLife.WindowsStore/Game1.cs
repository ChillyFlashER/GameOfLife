namespace GameOfLife
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Diagnostics;
    using Windows.UI.Xaml;

    public class Game1 : Game
    {
        public DrawableSimulation Simulation { get; private set; }

        //private DispatcherTimer timer;

        private InputState inputState;
        private Camera camera;

        private Texture2D texture;

        public Game1()
        {
            var graphics = new GraphicsDeviceManager(this);

            graphics.SynchronizeWithVerticalRetrace = false;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;

            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void LoadContent()
        {
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });

            Simulation = new DrawableSimulation(GraphicsDevice);
            Simulation.VisualScale = 6;


            // TODO: Improve patterns
            var gliderGun = new GameOfLife.Patterns.GliderGun();
            gliderGun.Create(Simulation, 0, 0);

            var lightweightSpaceship = new GameOfLife.Patterns.LightweightSpaceship();
            lightweightSpaceship.Create(Simulation, 100, 40);


            inputState = new InputState();

            var viewport = GraphicsDevice.Viewport;
            camera = new Camera(GraphicsDevice);
            camera.Position = new Vector2(viewport.Width / 2 - 25, viewport.Height / 2 - 75);

            //timer = new DispatcherTimer();
            //timer.Tick += (s, e) => Simulation.Step();
            //timer.Interval = System.TimeSpan.FromMilliseconds(100);
            //timer.Start();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            inputState.Update();

            if (inputState.IsNewKeyDown(Keys.N))
            {
                Simulation.Step();
            }

            if (inputState.Mouse.LeftButton == ButtonState.Pressed)
            {
                Simulation.SetCell(true);
            }

            if (inputState.Mouse.RightButton == ButtonState.Pressed)
            {
                Simulation.SetCell(false);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 40, 40));
            Simulation.Draw(GraphicsDevice, camera.View);
            base.Draw(gameTime);
        }
        
        static void Main()
        {
            var factory = new MonoGame.Framework.GameFrameworkViewSource<Game1>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }
    }
}
