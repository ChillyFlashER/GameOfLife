namespace GameOfLife.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class DrawableInfiniteGrid : InfiniteGrid<bool>
    {
        public float VisualScale
        {
            get { return visualScale; }
            set { visualScale = value; }
        }
        private float visualScale = 10;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        private SpriteBatch spriteBatch;

        private Texture2D blankTexture;
        private Matrix viewMatrix;

        public DrawableInfiniteGrid(GraphicsDevice graphics)
        {
            this.spriteBatch = new SpriteBatch(graphics);

            this.blankTexture = new Texture2D(graphics, 1, 1);
            this.blankTexture.SetData<Color>(new Color[] { Color.White });
        }

        public bool SetCell(bool value)
        {
            // TODO: Negative numbers

            var mousePos = Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2();

            var worldPos = Vector2.Transform(mousePos, Matrix.Invert(viewMatrix));
            var localPos = (worldPos / VisualScale).ToPoint();

            return this.SetCell(localPos.X, localPos.Y, value);
        }

        public void Draw(GraphicsDevice graphics, Matrix? view = null)
        {
            viewMatrix = view ?? Matrix.Identity;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, viewMatrix);

            foreach (var item in Values)
            {
                DrawChunk(item.Key, item.Value);
            }

            spriteBatch.End();
        }

        private void DrawChunk(Point point, Grid<bool> grid)
        {
            float chunkX = point.X * ChunkSize.X * VisualScale;
            float chunkY = point.Y * ChunkSize.Y * VisualScale;

            spriteBatch.Draw(blankTexture, new Rectangle(
                (int)chunkX, (int)chunkY, 
                (int)(ChunkSize.X * VisualScale), 
                (int)(ChunkSize.Y * VisualScale)), Color.Green);


            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    var cell = grid.GetCell(x, y);
                    if (cell.HasValue && cell.Value)
                    {
                        spriteBatch.Draw(blankTexture, new Rectangle(
                            (int)(chunkX + (x * VisualScale)),
                            (int)(chunkY + (y * VisualScale)), 
                            (int)VisualScale, 
                            (int)VisualScale), Color.Black);
                    }
                }
            }
        }
    }
}
