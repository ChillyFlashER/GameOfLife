namespace GameOfLife
{
    using PCLStorage;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    
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
        public abstract void Clear();

        /// <summary>
        /// Steps the simulation forwards.
        /// </summary>
        public abstract void Step();

        /// <summary>
        /// Save current simulation state to file path.
        /// </summary>
        public virtual async void SaveAsync(string filePath)
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            IFile file;

            var fileResult = await rootFolder.CheckExistsAsync(filePath);
            if (fileResult == ExistenceCheckResult.FileExists)
            {
                file = await rootFolder.GetFileAsync(filePath);
                Debug.WriteLine(string.Format("Simulation state file override at '{0}'.", file.Path));
            }
            else
            {
                file = await rootFolder.CreateFileAsync(filePath, CreationCollisionOption.GenerateUniqueName);
                Debug.WriteLine(string.Format("Simulation state save file created at '{0}'.", file.Path));
            }

            SaveAsync(file);
        }

        /// <summary>
        /// Save current simulation state to file.
        /// </summary>
        public virtual async void SaveAsync(IFile file)
        {
            using (var stream = new BinaryWriter(await file.OpenAsync(FileAccess.ReadAndWrite)))
            {
                this.Write(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Write(BinaryWriter writer);

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Read(BinaryReader reader);
    }
}
