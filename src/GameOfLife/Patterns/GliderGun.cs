namespace GameOfLife.Patterns
{
    /// <summary>
    /// Structure; A single Gosper's Glider Gun creating "gliders".
    /// </summary>
    public class GliderGun : Pattern
    {
        /// <inheritdoc />
        public override int Width { get { return 0; } }

        /// <inheritdoc />
        public override int Height { get { return 0; } }

        /// <inheritdoc />
        public override void Create(LimitedSimulation simulation, int x, int y)
        {
            // TODO: Make it easier to create patterns and apply them

            simulation.Grid.SetCell(1, 6, true);
            simulation.Grid.SetCell(2, 6, true);
            simulation.Grid.SetCell(1, 7, true);
            simulation.Grid.SetCell(2, 7, true);

            simulation.Grid.SetCell(35, 4, true);
            simulation.Grid.SetCell(36, 4, true);
            simulation.Grid.SetCell(35, 5, true);
            simulation.Grid.SetCell(36, 5, true);

            simulation.Grid.SetCell(13, 4, true);
            simulation.Grid.SetCell(14, 4, true);
            simulation.Grid.SetCell(12, 5, true);
            simulation.Grid.SetCell(11, 6, true);
            simulation.Grid.SetCell(11, 7, true);
            simulation.Grid.SetCell(11, 8, true);
            simulation.Grid.SetCell(12, 9, true);
            simulation.Grid.SetCell(13, 10, true);
            simulation.Grid.SetCell(14, 10, true);

            simulation.Grid.SetCell(15, 7, true);

            simulation.Grid.SetCell(16, 5, true);
            simulation.Grid.SetCell(17, 6, true);
            simulation.Grid.SetCell(17, 7, true);
            simulation.Grid.SetCell(18, 7, true);
            simulation.Grid.SetCell(17, 8, true);
            simulation.Grid.SetCell(16, 9, true);

            simulation.Grid.SetCell(21, 4, true);
            simulation.Grid.SetCell(21, 5, true);
            simulation.Grid.SetCell(21, 6, true);
            simulation.Grid.SetCell(22, 4, true);
            simulation.Grid.SetCell(22, 5, true);
            simulation.Grid.SetCell(22, 6, true);
            simulation.Grid.SetCell(23, 3, true);
            simulation.Grid.SetCell(23, 7, true);

            simulation.Grid.SetCell(25, 2, true);
            simulation.Grid.SetCell(25, 3, true);
            simulation.Grid.SetCell(25, 7, true);
            simulation.Grid.SetCell(25, 8, true);
        }
    }
}
