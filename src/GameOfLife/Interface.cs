namespace GameOfLife
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGrid<T> 
        where T : struct
    {
        /// <summary>
        /// Gets cell at position.
        /// </summary>
        T? GetCell(int x, int y);

        /// <summary>
        /// Sets cell at position.
        /// </summary>
        bool SetCell(int x, int y, T value);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISimulation
    {
        /// <summary>
        /// 
        /// </summary>
        void Step();
    }
}
