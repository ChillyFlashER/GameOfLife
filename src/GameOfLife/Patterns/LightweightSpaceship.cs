namespace GameOfLife.Patterns
{
    /// <summary>
    /// Spaceship; 
    /// </summary>
    public class LightweightSpaceship : Pattern
    {
        /// <inheritdoc />
        public override int Width { get { return 6; } }

        /// <inheritdoc />
        public override int Height { get { return 7; } }

        /// <inheritdoc />
        public override void Create(Simulation simulation, int x, int y)
        {
            var pattern = new byte[6,7]
            {
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 1, 0, 0, 0, 1, 0 },
                { 0, 0, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
            };

            for (int dx = 0; dx < 6; dx++)
            {
                for (int dy = 0; dy < 7; dy++)
                {
                    bool value = pattern[dx, dy] == 1;
                    simulation.SetCell(x + dx, y + dy, value);
                }
            }
        }
    }
}
