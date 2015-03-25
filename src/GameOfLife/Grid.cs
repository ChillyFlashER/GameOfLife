namespace GameOfLife
{
    using System;

    /// <summary>
    /// Generic 2D flat array.
    /// </summary>
    /// <typeparam name="T">Container Value</typeparam>
    public class Grid<T>
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
        public T this[int x, int y]
        {
            get { return this.At(x, y); }
            set { this.Set(x, y, value); }
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

        /// <summary>
        /// Returns value at position.
        /// </summary>
        public virtual T At(int x, int y)
        {
            if (x < 0 || x > this.Width)
                throw new ArgumentOutOfRangeException("x is not in range of width.");

            if (y < 0 || y > this.Height)
                throw new ArgumentOutOfRangeException("y is not in range of height.");

            return this.values[(y * this.Width) + x];
        }

        /// <summary>
        /// Sets value at position.
        /// </summary>
        public virtual void Set(int x, int y, T value)
        {
            if (x < 0 || x > this.Width)
                throw new ArgumentOutOfRangeException("x is not in range of width.");

            if (y < 0 || y > this.Height)
                throw new ArgumentOutOfRangeException("y is not in range of height.");

            this.values[(y * Width) + x] = value;
        }
    }
}
