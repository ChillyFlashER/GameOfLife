namespace GameOfLife
{
    using System.Threading.Tasks;
    
    /// <summary>
    /// Simulate Game Of Life.
    /// </summary>
    /// <remarks>
    /// Any live cell with fewer than two live neighbours dies, as if caused by under-population.
    /// Any live cell with two or three live neighbours lives on to the next generation.
    /// Any live cell with more than three live neighbours dies, as if by overcrowding.
    /// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
    /// </remarks>
    public class Simulation
    {
        /// <summary>
        /// Gets the grid that holds the simulation data.
        /// </summary>
        public Grid<bool> Grid { get; private set; }

        // The default size of Grid<T> is 100
        private int size = 100; // TODO: Remove

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulation"/> class.
        /// </summary>
        public Simulation()
        {
            this.Grid = new Grid<bool>();
        }

        /// <summary>
        /// Clear simulation.
        /// </summary>
        public void Clear()
        {
            // TODO: If Step() is processing there might be an issue.
            this.Grid = new Grid<bool>();
        }

        /// <summary>
        /// Steps the simulation forward.
        /// </summary>
        public void Step()
        {
            var newGrid = new Grid<bool>();

            Parallel.For(0, size, x =>
            {
                for (int y = 0; y < size; y++)
                {
                    var cell = this.Grid[x, y];
                    var cellValue = cell.HasValue ? cell.Value : false;
                    var neighbours = this.Neighbours(x, y);
                    newGrid[x, y] = (cellValue && neighbours >= 2 && neighbours <= 3) ||
                        (!cellValue && neighbours == 3);
                }
            });

            this.Grid = newGrid;
        }

        /// <summary>
        /// Returns all the neighbours of a point.
        /// </summary>
        private int Neighbours(int x, int y)
        {
            // TODO: Make this independant of Grid<T> class.

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
