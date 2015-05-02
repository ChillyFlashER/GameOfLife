namespace GameOfLife
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    
    public class GridTest
    {
        const int size = 100;

        private Grid<bool> grid;

        public GridTest()
        {
            var rd = new Random(20150501);

            this.grid = new Grid<bool>(size, size);

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (rd.Next(0, 10) > 5)
                        this.grid.SetCell(x, y, true);
        }

        [Fact]
        public void to_buffer()
        {
            var list = new List<UnlimitedSimulation.Point>();

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (grid.GetCell(x, y))
                        list.Add(new UnlimitedSimulation.Point(x, y));
        }
    }
}
