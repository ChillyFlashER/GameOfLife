namespace GameOfLife
{
    using System;

    /// <summary>
    /// Generic 2D flat array.
    /// </summary>
    /// <typeparam name="T">Container Value</typeparam>
    public class Grid<T> : IGrid<T>
        where T : struct
    {
        /// <summary>
        /// Gets the width of the array.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the array.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets value at position.
        /// </summary>
        public T? this[int x, int y]
        {
            get { return this.GetCell(x, y); }
            set { this.SetCell(x, y, value.Value); }
        }

        private T[] values;

        /// <summary>
        /// Initializes a new <see cref="Grid<T>" /> with width and height of 100.
        /// </summary>
        public Grid()
            : this(100, 100)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="Grid<T>" /> class.
        /// </summary>
        public Grid(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.values = new T[this.Width * this.Height];
        }

        /// <inheritdoc />
        public T GetCell(int x, int y)
        {
            return this.values[(y * this.Width) + x];
        }

        /// <inheritdoc />
        public bool TryGetCell(int x, int y, out T value)
        {
            if (x < 0 || x >= this.Width)
            {
                value = default(T);
                return false;
            }

            if (y < 0 || y >= this.Height)
            {
                value = default(T);
                return false;
            }

            value = this.values[(y * this.Width) + x];
            return true;
        }

        /// <inheritdoc />
        public void SetCell(int x, int y, T value)
        {
            this.values[(y * Width) + x] = value;
        }
    }
}
