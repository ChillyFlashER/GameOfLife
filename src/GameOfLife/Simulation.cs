namespace GameOfLife
{
    using PCLStorage;
    using System.IO;
    
    /// <summary>
    /// Simulate Game Of Life.
    /// </summary>
    /// <remarks>
    /// Rules of Game Of Life:
    /// Any live cell with fewer than two live neighbours dies, as if caused by under-population.
    /// Any live cell with two or three live neighbours lives on to the next generation.
    /// Any live cell with more than three live neighbours dies, as if by overcrowding.
    /// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
    /// </remarks>
    public abstract class Simulation
    {
        /// <summary>
        /// Clear simulation.
        /// </summary>
        public abstract void Clear();                   // TODO: Make async

        /// <summary>
        /// Steps the simulation forwards.
        /// </summary>
        public abstract void Step();                    // TODO: Make async

        /// <summary>
        /// Steps the simulation backwards.
        /// </summary>
        public abstract void StepBack();                // TODO: Make async

        /// <summary>
        /// Save current simulation state to file path.
        /// </summary>
        public virtual async void SaveAsync(string filePath)
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            
            var file = await rootFolder.GetFileAsync(filePath);
            if (file == null)
            {
                file = await rootFolder.CreateFileAsync(filePath, CreationCollisionOption.GenerateUniqueName);
            }

            SaveAsync(file);
        }

        /// <summary>
        /// Save current simulation state to file.
        /// </summary>
        public abstract async void SaveAsync(IFile file);
    }
}
