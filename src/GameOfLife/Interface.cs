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
        /// <returns></returns>
        T GetCell(int x, int y);

        /// <summary>
        /// Try get cell at position.
        /// </summary>
        /// <returns></returns>
        bool TryGetCell(int x, int y, out T value);

        /// <summary>
        /// Sets cell at position.
        /// </summary>
        void SetCell(int x, int y, T value);
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
