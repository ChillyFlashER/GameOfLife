namespace GameOfLife
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    public class Button
    {
        private Action execute;
        private Texture2D texture;
        private Rectangle rectangle;
        private Rectangle sourceRectangle;
        private Rectangle sourceRectangleMouseOver;
        private bool isMouseOver;

        public Button(Texture2D texture, Rectangle rectangle, Rectangle sourceRectangle, Rectangle sourceRectangleMouseOver, Action execute)
        {
            this.execute = execute;
            this.texture = texture;
            this.rectangle = rectangle;
            this.sourceRectangle = sourceRectangle;
            this.sourceRectangleMouseOver = sourceRectangleMouseOver;
            this.isMouseOver = false;
        }

        public void OnInput(InputState inputState, Vector2 mousePosition)
        {
            if (this.isMouseOver = rectangle.Contains(mousePosition))
            {
                if (inputState.IsNewMouseLeftDown() && execute != null)
                {
                    execute();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, !isMouseOver ? sourceRectangle : sourceRectangleMouseOver, Color.White);
        }
    }

    public class Userinterface
    {
        private bool playing; // TODO: Remove this
        private Button playButton;
        private Button pauseButton;
        private Button stepForwardButton;
        private Button clearButton;

        public Userinterface(Texture2D spritesheet, Game1 game)
        {
            playing = true;

            // TODO: Add tooltip

            playButton = new Button(spritesheet, new Rectangle(0, -50, 35, 35), new Rectangle(0, 0, 100, 100), new Rectangle(0, 100, 100, 100), () => game.Timer.Enabled = playing = true);
            pauseButton = new Button(spritesheet, new Rectangle(0, -50, 35, 35), new Rectangle(100, 0, 100, 100), new Rectangle(100, 100, 100, 100), () => game.Timer.Enabled = playing = false);

            stepForwardButton = new Button(spritesheet, new Rectangle(50, -50, 35, 35), new Rectangle(300, 0, 100, 100), new Rectangle(300, 100, 100, 100), () => game.Simulation.Step());

            clearButton = new Button(spritesheet, new Rectangle(100, -50, 35, 35), new Rectangle(0, 200, 100, 100), new Rectangle(0, 300, 100, 100), () => game.Simulation.Clear());
        }

        public void OnInput(InputState inputState, Vector2 worldPos)
        {
            if (playing)
            {
                pauseButton.OnInput(inputState, worldPos);
            }
            else
            {
                playButton.OnInput(inputState, worldPos);
            }

            stepForwardButton.OnInput(inputState, worldPos);
            clearButton.OnInput(inputState, worldPos);
        }

        public void Draw(SpriteBatch spriteBatch, Matrix view)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, view);

            if (playing)
            {
                pauseButton.Draw(spriteBatch);
            }
            else
            {
                playButton.Draw(spriteBatch);
            }

            stepForwardButton.Draw(spriteBatch);
            clearButton.Draw(spriteBatch);

            spriteBatch.End();
        }
    }

}
