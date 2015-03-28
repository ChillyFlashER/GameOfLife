namespace GameOfLife
{
    using Xunit;
    
    public class SimulationTest
    {
        [Fact]
        public void still_life_works()
        {
            int[] points = { 2, 2, 2, 3, 3, 2, 3, 3 };
            var simulation = new LimitedSimulation();
            
            for (int i = 0; i < points.Length - 1; i += 2)
                simulation.Grid.SetCell(points[i], points[i + 1], true);

            for (int i = 0; i < points.Length - 1; i += 2)
                Assert.True(simulation.Grid.GetCell(points[i], points[i + 1]));

            simulation.Step();

            for (int i = 0; i < points.Length - 1; i += 2)
                Assert.True(simulation.Grid.GetCell(points[i], points[i + 1]));
        }
    }
}
