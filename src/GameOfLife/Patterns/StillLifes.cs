namespace GameOfLife.Patterns
{
    /// <summary>
    /// Still Life; 
    /// </summary>
    public class Block : Pattern
    {
        /// <inheritdoc />
        public override int Width { get { return 4; } }

        /// <inheritdoc />
        public override int Height { get { return 4; } }

        /// <inheritdoc />
        public override void Create(LimitedSimulation simulation, int x, int y)
        {
            var pattern = new byte[4, 4]
            {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
            };

            for (int dx = 0; dx < 4; dx++)
            {
                for (int dy = 0; dy < 4; dy++)
                {
                    simulation.Grid.SetCell(x + dx, y + dy, pattern[dx, dy] == 1);
                }
            }
        }
    }

    /// <summary>
    /// Still Life; 
    /// </summary>
    public class Beehive : Pattern
    {
        /// <inheritdoc />
        public override int Width { get { return 6; } }

        /// <inheritdoc />
        public override int Height { get { return 5; } }

        /// <inheritdoc />
        public override void Create(LimitedSimulation simulation, int x, int y)
        {
            var pattern = new byte[5, 6]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 1, 0, 0, 1, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0 },
            };

            for (int dx = 0; dx < 6; dx++)
            {
                for (int dy = 0; dy < 5; dy++)
                {
                    simulation.Grid.SetCell(x + dx, y + dy, pattern[dx, dy] == 1);
                }
            }
        }
    }

    /// <summary>
    /// Still Life; 
    /// </summary>
    public class Loaf : Pattern
    {
        /// <inheritdoc />
        public override int Width { get { return 6; } }

        /// <inheritdoc />
        public override int Height { get { return 6; } }

        /// <inheritdoc />
        public override void Create(LimitedSimulation simulation, int x, int y)
        {
            var pattern = new byte[6, 6]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 1, 0, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0 },
            };

            for (int dx = 0; dx < 6; dx++)
            {
                for (int dy = 0; dy < 6; dy++)
                {
                    simulation.Grid.SetCell(x + dx, y + dy, pattern[dx, dy] == 1);
                }
            }
        }
    }

    /// <summary>
    /// Still Life; 
    /// </summary>
    public class Boat : Pattern
    {
        /// <inheritdoc />
        public override int Width { get { return 5; } }

        /// <inheritdoc />
        public override int Height { get { return 5; } }

        /// <inheritdoc />
        public override void Create(LimitedSimulation simulation, int x, int y)
        {
            var pattern = new byte[5, 5]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 1, 1, 0, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0 },
            };

            for (int dx = 0; dx < 6; dx++)
            {
                for (int dy = 0; dy < 6; dy++)
                {
                    simulation.Grid.SetCell(x + dx, y + dy, pattern[dx, dy] == 1);
                }
            }
        }
    }
}
