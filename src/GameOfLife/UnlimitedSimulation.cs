namespace GameOfLife
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Simulate Game Of Life.
    /// </summary>
    public class UnlimitedSimulation : Simulation
    {
        /// <summary>
        /// Gets the grid that holds the simulation data.
        /// </summary>
        public InfiniteGrid<bool> Grid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlimitedSimulation"/> class.
        /// </summary>
        public UnlimitedSimulation()
        {

        }

        /// <inheritdoc />
        public override void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Step()
        {
            var newGrid = new InfiniteGrid<bool>();

            // TODO: Moar Parallel!

            var tasks = new List<Task>();

            foreach (var item in Grid.Values.ToArray())
            {
                var grid = item.Value;
                for (int i = 0; i < grid.Width; i++)
                {
                    tasks.Add(Task.Factory.StartNew(state =>
                    {
                        var x = (int)state;
                        for (int y = 0; y < grid.Height; y++)
                        {
                            var cell = this.Grid.GetCell(item.Key.X + x, item.Key.Y + y);
                            var cellValue = cell.HasValue ? cell.Value : false;
                            var neighbours = this.Neighbours(x, y);
                            var newValue = (cellValue && neighbours >= 2 && neighbours <= 3) ||
                                (!cellValue && neighbours == 3);

                            newGrid.SetCell(x, y, newValue);
                        }
                    }, i));
                }
            }

            Task.WaitAll(tasks.ToArray());

            this.Grid = newGrid;
        }

        /// <inheritdoc />
        protected override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException("Is this how I want to implement it?");
        }

        /// <inheritdoc />
        protected override void Read(BinaryReader reader)
        {
            throw new NotImplementedException("Is this how I want to implement it?");
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

                    var cell = this.Grid.GetCell(x1, y1);
                    if (cell.HasValue && cell.Value)
                        neighbours++;
                }
            }

            return neighbours;
        }
    }
}
