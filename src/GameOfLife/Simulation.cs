using System.Threading.Tasks;
namespace GameOfLife
{
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
        /// Initializes a new instance of the <see cref="Simulation"/> class. With width and height of 100.
        /// </summary>
        public Simulation()
            : this(100, 100)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulation"/> class.
        /// </summary>
        public Simulation(int width, int height)
        {
            this.Grid = new Grid<bool>(width, height);
        }

        /// <summary>
        /// Clear simulation.
        /// </summary>
        public void Clear()
        {
            // TODO: If Step() is processing there might be an issue.
            this.Grid = new Grid<bool>(this.Grid.Width, this.Grid.Height);
        }

        /// <summary>
        /// Set cell at position.
        /// </summary>
        public bool SetCell(int x, int y, bool value)
        {
            if (x < 0 || x > this.Width)
                return false;

            if (y < 0 || y > this.Height)
                return false;

            this.Grid[x, y] = value;
            return true;
        }

        /// <summary>
        /// Steps the simulation forward.
        /// </summary>
        public void Step()
        {
            var newGrid = new Grid<bool>(this.Grid.Width, this.Grid.Height);

            Parallel.For(0, this.Grid.Width, x =>
            {
                //Parallel.For(0, this.Grid.Height, y =>
                for (int y = 0; y < this.Grid.Height; y++)
                {
                    var neighbours = this.Neighbours(x, y);
                    newGrid[x, y] = (this.Grid[x, y] && neighbours >= 2 && neighbours <= 3) ||
                        (!this.Grid[x, y] && neighbours == 3);
                }//);
            });

            //for (int x = 0; x < this.Grid.Width; x++)
            //{
            //    for (int y = 0; y < this.Grid.Height; y++)
            //    {
            //        var neighbours = this.Neighbours(x, y);
            //        newGrid[x, y] = (this.Grid[x, y] && neighbours >= 2 && neighbours <= 3) || 
            //            (!this.Grid[x, y] && neighbours == 3);
            //    }
            //}

            this.Grid = newGrid;
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

                    if (this.Grid[x1, y1])
                        neighbours++;
                }
            }

            return neighbours;
        }
    }
}
