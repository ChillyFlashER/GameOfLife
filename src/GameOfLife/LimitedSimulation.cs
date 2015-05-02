namespace GameOfLife
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Limitied Simulate Game Of Life.
    /// </summary>
    public class LimitedSimulation : Simulation
    {
        /// <summary>
        /// Gets the width of the simulation in cells.
        /// </summary>
        public int Width
        {
            get { return this.Grid.Width; }
        }

        /// <summary>
        /// Gets the height of the simulation in cells.
        /// </summary>
        public int Height
        {
            get { return this.Grid.Height; }
        }

        /// <summary>
        /// Gets the grid that holds the simulation data.
        /// </summary>
        public Grid<bool> Grid { get; private set; }

        /// <summary>
        /// Gets or sets simulation should be done in parallel.
        /// </summary>
        public bool IsParallel { get; set; }

        private CancellationTokenSource taskCancelToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedSimulation"/> class with width and height of 100.
        /// </summary>
        public LimitedSimulation()
            : this(100, 100)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedSimulation"/> class.
        /// </summary>
        public LimitedSimulation(int width, int height)
        {
            this.Grid = new Grid<bool>(width, height);
            this.IsParallel = true;
        }

        /// <inheritdoc />
        public override void Clear()
        {
            if (taskCancelToken != null)
            {
                taskCancelToken.Cancel();
            }

            this.Grid = new Grid<bool>(this.Width, this.Height);
        }

        /// <inheritdoc />
        public override void Step()
        {
            if (taskCancelToken != null)
            {
                // TODO: Throw exeption?
                return; 
            }

            taskCancelToken = new CancellationTokenSource();

            Task<Grid<bool>>.Factory.StartNew(e =>
            {
                var newGrid = new Grid<bool>(this.Width, this.Height);

                if (IsParallel)
                {
                    var tasks = new List<Task>();

                    for (int i = 0; i < this.Grid.Width; i++)
                    {
                        tasks.Add(Task.Factory.StartNew(state =>
                        {
                            var x = (int)state;
                            for (int y = 0; y < this.Grid.Height; y++)
                            {
                                var cell = this.Grid[x, y];
                                var cellValue = cell.HasValue ? cell.Value : false;
                                var neighbours = this.Neighbours(x, y);
                                newGrid[x, y] = (cellValue && neighbours >= 2 && neighbours <= 3) ||
                                    (!cellValue && neighbours == 3);
                            }
                        }, i));
                    }

                    Task.WaitAll(tasks.ToArray());
                }
                else
                {
                    for (int x = 0; x < this.Grid.Width; x++)
                    {
                        for (int y = 0; y < this.Grid.Height; y++)
                        {
                            var cell = this.Grid[x, y];
                            var cellValue = cell.HasValue ? cell.Value : false;
                            var neighbours = this.Neighbours(x, y);
                            newGrid[x, y] = (cellValue && neighbours >= 2 && neighbours <= 3) ||
                                (!cellValue && neighbours == 3);
                        }
                    }
                }

                return newGrid;

            }, taskCancelToken).ContinueWith(r => 
            {
                if (r.IsCompleted)
                {
                    this.Grid = r.Result;
                }
            }).Wait();

            taskCancelToken.Dispose();
            taskCancelToken = null;
        }

        /// <summary>
        /// Returns all the neighbours of a point.
        /// </summary>
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
                    
                    if (x1 < 0)
                        x1 += this.Grid.Width;

                    if (x1 >= this.Grid.Width)
                        x1 -= this.Grid.Width;

                    if (y1 < 0)
                        y1 += this.Grid.Height;

                    if (y1 >= this.Grid.Height)
                        y1 -= this.Grid.Height;

                    var cell = this.Grid[x1, y1];
                    if (cell.HasValue && cell.Value)
                        neighbours++;
                }
            }

            return neighbours;
        }
    }
}
