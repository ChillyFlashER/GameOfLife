namespace GameOfLife
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Simulate Game Of Life.
    /// </summary>
    public class UnlimitedSimulation : Simulation
    {
        /// <summary>
        /// Gets the chunk size.
        /// </summary>
        public Point ChunkSize
        {
            get { return chunkSize; }
        }
        private Point chunkSize;

        /// <summary>
        /// Gets the chunks.
        /// </summary>
        public IEnumerable<KeyValuePair<Point, Grid<bool>>> Values
        {
            get { return values; }
        }
        private ImmutableDictionary<Point, Grid<bool>> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlimitedSimulation"/> class.
        /// </summary>
        public UnlimitedSimulation()
            : this(100, 100) 
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlimitedSimulation"/> class.
        /// </summary>
        public UnlimitedSimulation(int chunkSizeX, int chunkSizeY)
        {
            this.chunkSize = new Point(chunkSizeX, chunkSizeY);
            this.values = ImmutableDictionary.Create<Point, Grid<bool>>();
        }

        #region Methods

        /// <inheritdoc />
        public override void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task StepAsync()
        {
            var tasks = new List<Task>();

            foreach (var item in Values)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var grid = item.Value;
                    for (int x = 0; x < grid.Width; x++)
                    {
                        for (int y = 0; y < grid.Height; y++)
                        {
                            var cell = this.GetCell(item.Key.X + x, item.Key.Y + y);
                            var cellValue = cell.HasValue ? cell.Value : false;
                            var neighbours = this.Neighbours(x, y);
                            var newValue = (cellValue && neighbours >= 2 && neighbours <= 3) ||
                                (!cellValue && neighbours == 3);

                            grid.SetCell(x, y, newValue);
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private int Neighbours(int x, int y)
        {
            int neighbours = 0;
            int x1, y1;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    x1 = x + dx;
                    y1 = y + dy;

                    var cell = this.GetCell(x1, y1);
                    if (cell.HasValue && cell.Value)
                        neighbours++;
                }
            }

            return neighbours;
        }

        #endregion

        #region Grid Methods

        public bool? GetCell(int x, int y)
        {
            var position = this.WorldToChunk(x, y);

            var localGrid = this.GetChunk(position.Item1);
            if (localGrid != null)
            {
                return localGrid.GetCell(position.Item2.X, position.Item2.Y);
            }

            return null;
        }

        public bool SetCell(int x, int y, bool value)
        {
            var position = this.WorldToChunk(x, y);

            var localGrid = this.GetChunk(position.Item1);
            if (localGrid != null)
            {
                localGrid.SetCell(position.Item2.X, position.Item2.Y, value);
                OnChunkChanged(position.Item1);
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Grid<bool> GetChunk(Point point)
        {
            Grid<bool> localGrid;
            if (!values.TryGetValue(point, out localGrid))
            {
                values = values.Add(point, localGrid = new Grid<bool>(chunkSize.X, chunkSize.Y));
            }

            return localGrid;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Tuple<Point, Point> WorldToChunk(int x, int y)
        {
            return new Tuple<Point, Point>(
                new Point(x / chunkSize.X, y / chunkSize.Y),
                new Point(
                    ((x % chunkSize.X) + chunkSize.X) % chunkSize.X,
                    ((y % chunkSize.Y) + chunkSize.Y) % chunkSize.Y));
        }

        #endregion

        protected virtual void OnChunkChanged(Point chunkPositon)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public struct Point
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
