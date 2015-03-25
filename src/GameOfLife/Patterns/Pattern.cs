namespace GameOfLife.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Pattern
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract int Width { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Create(Simulation simulation, int x, int y); // TODO: Add rotation
    }
}
