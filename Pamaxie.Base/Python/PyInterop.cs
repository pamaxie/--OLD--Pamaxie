using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Base.Python
{
    /// <summary>
    /// <inheritdoc cref="IPyInterop"/>
    /// </summary>
    internal class PyInterop : IPyInterop
    {
        private bool _hasPython;
        private const string _pythonVersion = "3";

        /// <summary>
        /// <inheritdoc cref="IPyInterop.CanRun(string)"/>
        /// </summary>
        public bool CanRun(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            FileInfo file = new FileInfo(fileName);

            return this.CanRun(file);
        }

        /// <summary>
        /// <inheritdoc cref="IPyInterop.CanRun(FileInfo)"/>
        /// </summary>
        public bool CanRun(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException();
            }

            //Valides the file is written specifically for pamaxie.
            if (file.Extension != "pampy")
            {
                throw new InvalidFileException(string.Format("{0} is not a supported extension for Python interop. Please use a .pampy file.", file.Extension));
            }

            return true;
        }

        /// <summary>
        /// <inheritdoc cref="IPyInterop.GetPythonFilePaths(string)"/>
        /// </summary>
        public IEnumerable<string> GetPythonFilePaths(string sourceDirectory)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory))
            {
                sourceDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
            }

            var files = Directory.GetFiles(sourceDirectory, "*.pampy");
            
            if (!files.Any())
            {
                yield return String.Empty;
            }

            foreach(var file in files)
            {
                yield return file;
            }
        }

        /// <summary>
        /// <inheritdoc cref="IPyInterop.GetPythonFiles(string)"/>
        /// </summary>
        public IEnumerable<FileInfo?> GetPythonFiles(string sourceDirectory)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory))
            {
                sourceDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
            }

            var files = Directory.GetFiles(sourceDirectory, "*.pampy");

            if (!files.Any())
            {
                yield return null;
            }

            foreach (var file in files)
            {
                yield return new FileInfo(file);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IPyInterop.HasPython"/>
        /// </summary>
        public bool HasPython()
        {
            //Omit check if we already detected an installation before
            if (_hasPython)
            {
                return true;
            }

            //Tries to run the python process.
            ProcessStartInfo pythonProcess = new();
            pythonProcess.FileName = @"cmd.exe"; // Specify exe name.
            pythonProcess.Arguments = "python --version";
            pythonProcess.UseShellExecute = false;
            pythonProcess.RedirectStandardError = true;
            pythonProcess.CreateNoWindow = true;

            using Process process = Process.Start(pythonProcess);
            if (process == null)
            {
                return false;
            }

            using StreamReader reader = process.StandardError;
            var result = reader.ReadToEnd();
            reader.Close();
            process.Close();
            return string.IsNullOrWhiteSpace(result);
        }
    }
}
