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

        public DrawableInfiniteGrid(GraphicsDevice graphics)
        {
            this.spriteBatch = new SpriteBatch(graphics);

            this.blankTexture = new Texture2D(graphics, 1, 1);
            this.blankTexture.SetData<Color>(new Color[] { Color.White });
        }

        public void Draw(GraphicsDevice graphics, Matrix? view = null)
        {
            var viewMatrix = view ?? Matrix.Identity;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, viewMatrix);

            foreach (var item in Values)
            {
                DrawChunk(item.Key, item.Value);
            }

            spriteBatch.End();
        }

        private void DrawChunk(Point point, Grid<bool> grid)
        {
            spriteBatch.Draw(blankTexture, new Rectangle(
                (int)((point.X * ChunkSize.X) * VisualScale), 
                (int)((point.Y * ChunkSize.Y) * VisualScale), 
                (int)(ChunkSize.X * VisualScale), 
                (int)(ChunkSize.Y * VisualScale)), Color.Green);
        }
    }
}
