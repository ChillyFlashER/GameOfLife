﻿namespace GameOfLife
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Timers;

    using MessageBox = System.Windows.Forms.MessageBox;
    using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
    using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;

    public class Game1 : Game
    {
        public DrawableSimulation Simulation { get; private set; }

        public Stopwatch Stopwatch { get; private set; }
        public Timer Timer { get; private set; }

        private InputState inputState;
        private Userinterface userinterface;
        private Camera camera;
        private bool dragging;

        private Texture2D texture;

        public Game1()
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
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });

            Simulation = new DrawableSimulation(GraphicsDevice, 200, 100);
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

            Stopwatch = new Stopwatch();

            Timer = new Timer(100);
            Timer.Elapsed += (s, e) =>
            {
                Stopwatch.Restart();
                Simulation.Step();
                Stopwatch.Stop();

                Debug.WriteLine(string.Format("{3:N0} ({0}x{1}) cells in {2}ms", 
                    Simulation.Width, Simulation.Height, Stopwatch.ElapsedMilliseconds, Simulation.Width * Simulation.Height));
            };
            //Timer.Start();

            var spritesheetLocation = "Content/Spritesheet.png";
            if (!File.Exists(spritesheetLocation))
                MessageBox.Show(string.Format("Could not locate '{0}'.", spritesheetLocation), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            var spritesheet = Texture2D.FromStream(GraphicsDevice, File.Open(spritesheetLocation, FileMode.Open));
            userinterface = new Userinterface(spritesheet, this);

            dragging = false;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            inputState.Update();
            //camera.OnInput(inputState);
            
            var mousePos = inputState.Mouse.Position.ToVector2();
            var worldPos = Vector2.Transform(mousePos, Matrix.Invert(camera.View));

            if (!dragging)
            {
                userinterface.OnInput(inputState, worldPos);

                if (inputState.Mouse.LeftButton == ButtonState.Pressed)
                {
                    Simulation.SetCell(true);
                }

                if (inputState.Mouse.RightButton == ButtonState.Pressed)
                {
                    Simulation.SetCell(false);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 40, 40));

            Simulation.Draw(GraphicsDevice, camera.View);
            userinterface.Draw(Simulation.SpriteBatch, camera.View);

            //var spriteBatch = Simulation.SpriteBatch;

            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.View);
            
            //spriteBatch.Draw(texture, new Rectangle(Simulation.VisualWidth + 25, 0, 300, Simulation.VisualHeight), Color.Red);

            //for (int i = 0; i < 5; i++)
            //{
            //    spriteBatch.Draw(texture, new Rectangle(Simulation.VisualWidth + 30, 5 + (80 * i), 275, 75), Color.Green);
            //}

            //spriteBatch.Draw(texture, new Rectangle(0, Simulation.VisualHeight + 10, 300, 20), Color.Red);
            //spriteBatch.Draw(texture, new Rectangle(Simulation.VisualWidth - 200, Simulation.VisualHeight + 10, 200, 20), Color.Red);

            //spriteBatch.End();

            base.Draw(gameTime);
        }
        
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
}