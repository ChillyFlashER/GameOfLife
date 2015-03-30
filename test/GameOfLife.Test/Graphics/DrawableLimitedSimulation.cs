namespace GameOfLife
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    /// <summary>
    /// Drawable overlay for <see cref="LimitedSimulation" />.
    /// </summary>
    public class DrawableSimulation : LimitedSimulation
    {
        /// <summary>
        /// Gets the spritebatch used to draw.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Gets or sets the visual scale.
        /// </summary>
        public float VisualScale
        {
            get { return visualScale; }
            set { visualScale = value; }
        }
        private float visualScale = 10;

        private Texture2D blankTexture;
        private RenderTarget2D backgroundTexture;
        private int backgroundCells = 10;

        private Matrix viewMatrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableSimulation"/> class.
        /// </summary>
        public DrawableSimulation(GraphicsDevice graphics)
        {
            this.spriteBatch = new SpriteBatch(graphics);

            this.blankTexture = new Texture2D(graphics, 1, 1);
            this.blankTexture.SetData<Color>(new Color[] { Color.White });

            this.CreateBackground(graphics);
        }

        /// <summary>
        /// Set cell where mouse is, taking view matrix from Draw(...).
        /// </summary>
        public bool SetCell(bool value)
        {
            var mousePos = Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2();

            var worldPos = Vector2.Transform(mousePos, Matrix.Invert(viewMatrix));
            var localPos = (worldPos / VisualScale).ToPoint();

            if (localPos.X > 0 && localPos.X < this.Grid.Width &&
                localPos.Y > 0 && localPos.Y < this.Grid.Height)
            {
                return Grid.SetCell(localPos.X, localPos.Y, value);
            }

            return false;
        }

        /// <summary>
        /// Draws the simulation.
        /// </summary>
        public void Draw(GraphicsDevice graphics, Matrix? view = null)
        {
            viewMatrix = view ?? Matrix.Identity;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, viewMatrix);

            for (int x = 0; x < (this.Grid.Width / backgroundCells); x++)
            {
                for (int y = 0; y < (this.Grid.Height / backgroundCells); y++)
                {
                    var size = (int)(backgroundCells * VisualScale);
                    spriteBatch.Draw(backgroundTexture, new Rectangle(size * x, size * y, size, size), Color.White);
                }
            }

            for (int x = 0; x < this.Grid.Width; x++)
            {
                for (int y = 0; y < this.Grid.Height; y++)
                {
                    var cell = this.Grid.GetCell(x, y);
                    if (cell.HasValue && cell.Value)
                    { // I really dont need to check HasValue
                        spriteBatch.Draw(blankTexture, new Rectangle((int)(x * VisualScale), (int)(y * VisualScale), (int)VisualScale, (int)VisualScale), Color.Black);
                    }
                }
            }

            spriteBatch.End();
        }

        private void CreateBackground(GraphicsDevice graphics)
        {
            var size = (int)(backgroundCells * VisualScale);

            this.backgroundTexture = new RenderTarget2D(graphics, size, size);

            graphics.SetRenderTarget(this.backgroundTexture);
            graphics.Clear(Color.Transparent);

            spriteBatch.Begin();

            int color = 0;
            for (int x = 0; x < backgroundCells; x++)
            {
                for (int y = 0; y < backgroundCells; y++)
                {
                    color = (color + 1) % 2;

                    var rect = new Rectangle((int)(x * VisualScale), (int)(y * VisualScale), (int)VisualScale, (int)VisualScale);
                    spriteBatch.Draw(blankTexture, rect, color == 0 ? Color.Gray : Color.LightGray);
                }
                color++;
            }

            spriteBatch.End();

            graphics.SetRenderTarget(null);
        }
    }
}
