namespace GameOfLife
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InfiniteGrid<T> : IGrid<T>
        where T : struct
    {
        /// <summary>
        /// 
        /// </summary>
        protected Point ChunkSize
        {
            get { return chunkSize; }
        }
        private Point chunkSize;

        /// <summary>
        /// 
        /// </summary>
        public int LoadedChunks
        {
            get { return values.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IEnumerable<KeyValuePair<Point, Grid<T>>> Values
        {
            get { return values; }
        }
        private Dictionary<Point, Grid<T>> values;

        /// <summary>
        /// 
        /// </summary>
        public InfiniteGrid()
            : this(100, 100)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public InfiniteGrid(int chunkSizeX, int chunkSizeY)
        {
            this.chunkSize = new Point(chunkSizeX, chunkSizeY);
            this.values = new Dictionary<Point, Grid<T>>();
        }

        /// <inheritdoc />
        public T? GetCell(int x, int y)
        {
            var position = this.WorldToChunk(x, y);

            var localGrid = this.GetChunk(position.Item1);
            if (localGrid != null)
            {
                return localGrid.GetCell(position.Item2.X, position.Item2.Y);
            }

            return null;
        }

        /// <inheritdoc />
        public bool SetCell(int x, int y, T value)
        {
            var position = this.WorldToChunk(x, y);

            var localGrid = this.GetChunk(position.Item1);
            if (localGrid != null)
            {
                localGrid.SetCell(position.Item2.X, position.Item2.Y, value);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryGetCell(int x, int y, out T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        protected Grid<T> GetChunk(Point point)
        {
            Grid<T> localGrid;
            if (values.TryGetValue(point, out localGrid))
            {
                return localGrid;
            }
            else
            {
                return values[point] = new Grid<T>(chunkSize.X, chunkSize.Y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Tuple<Point, Point> WorldToChunk(int x, int y)
        {
            return new Tuple<Point,Point>(
                new Point(x / chunkSize.X, y / chunkSize.Y),
                new Point(x % chunkSize.X, y % chunkSize.Y));
        }

        /// <summary>
        /// 
        /// </summary>
        protected struct Point
        {
            /// <summary>
            /// 
            /// </summary>
            public int X;

            /// <summary>
            /// 
            /// </summary>
            public int Y;

            /// <summary>
            /// 
            /// </summary>
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return string.Format("[{0}, {1}]", this.X, this.Y);
            }
        }
    }
}
