namespace GameOfLife
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;

    /// <summary>
    /// Simple Camera.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Gets the view matrix.
        /// </summary>
        public Matrix View
        {
            get { return view; }
        }

        /// <summary>
        /// Gets or sets the camera position.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; UpdateView(); }
        }
        private Vector2 position;

        /// <summary>
        /// Gets or sets the camera zoom.
        /// </summary>
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; UpdateView(); }
        }
        private float zoom = 1.0f;

        /// <summary>
        /// Gets or sets the camera min zoom.
        /// </summary>
        public float MinZoom
        {
            get { return minZoom; }
            set { minZoom = value; }
        }
        private float minZoom;

        /// <summary>
        /// Gets or sets the camera max zoom.
        /// </summary>
        public float MaxZoom
        {
            get { return maxZoom; }
            set { maxZoom = value; }
        }
        private float maxZoom;

        private Matrix view;
        private GraphicsDevice graphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera" /> class.
        /// </summary>
        public Camera(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            this.position = Vector2.Zero;

            this.MinZoom = 0.5f;
            this.MaxZoom = 1.0f;
            this.Zoom = 1.0f;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateView()
        {
            var viewport = graphics.Viewport;
            this.view = Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(-position.X + (viewport.Width / 2), -position.Y + (viewport.Height / 2), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnInput(InputState inputState)
        {
            var move = Vector2.Zero;

            if (inputState.IsKeyDown(Keys.W)) move.Y--;
            if (inputState.IsKeyDown(Keys.S)) move.Y++;
            if (inputState.IsKeyDown(Keys.A)) move.X--;
            if (inputState.IsKeyDown(Keys.D)) move.X++;

            if (move != Vector2.Zero)
            {
                this.Position += move;
            }

            if (inputState.ScrollWheelChanged)
            {
                Zoom += inputState.ScrollWheelDelta * 0.001f;
                Zoom = MathHelper.Clamp(Zoom, MinZoom, MaxZoom);
            }
        }

        /// <summary>
        /// Transform world position to screen position.
        /// </summary>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, View);
        }

        /// <summary>
        /// Transform screen position to world position.
        /// </summary>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(View));
        }
    }
}
