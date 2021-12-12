namespace Pamaxie.Base.Python
{
    /// <summary>
    /// Class used to interop with our python scripts for machine learning
    /// </summary>
    public interface IPyInterop
    {
        /// <summary>
        /// Gets all Python files in the folder that are accepted to be run by pamaxies interop
        /// <param name="sourceDirectory">The source directory to search for files in</param>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FileInfo?> GetPythonFiles(string? sourceDirectory = null);

        /// <summary>
        /// Returns fully qualified file path for the detected files that we can interop with
        /// </summary>
        /// <param name="sourceDirectory">The source directory to search for files in</param>
        /// <returns></returns>
        public IEnumerable<string> GetPythonFilePaths(string? sourceDirectory = null);

        /// <summary>
        /// Validates we can run a given file. This basically ensures that the python compiles
        /// and is compatible with our output interpreters
        /// </summary>
        /// <param name="fileName">The name and path of the file that should be validated</param>
        /// <returns><see cref="bool"/> if we can run the file</returns>
        public bool CanRun(string fileName);

        /// <summary>
        /// Validates we can run a given file. This basically ensures that the python compiles
        /// and is compatible with our output interpreters
        /// </summary>
        /// <param name="file">The file that should be validated</param>
        /// <returns></returns>
        public bool CanRun(FileInfo file);

        /// <summary>
        /// Validates the python interpreter is up and running
        /// </summary>
        /// <returns><see cref="bool"/> that says if python can be run on this system</returns>
        public bool HasPython();
    }
}