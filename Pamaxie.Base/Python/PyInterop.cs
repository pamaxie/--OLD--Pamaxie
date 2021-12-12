using Microsoft.Win32;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace Pamaxie.Base.Python
{
    /// <summary>
    /// <inheritdoc cref="IPyInterop"/>
    /// </summary>
    public class PyInterop : Singleton<PyInterop>, IPyInterop
    {
        private static bool _hasPython;
        private static string _pythonVersion = "3.5";

        public PyInterop() : base()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _pythonVersion = "libpython3.8.so";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _pythonVersion = "python38.dll";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _pythonVersion = "libpython3.8.dylib";
            }
        }

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
        public IEnumerable<string> GetPythonFilePaths(string? sourceDirectory = null)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory))
            {
                if (System.Reflection.Assembly.GetEntryAssembly() == null)
                {
                    throw new ArgumentNullException(nameof(sourceDirectory));
                }

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
        /// <inheritdoc cref="IPyInterop.GetPythonFilePaths(string)"/>
        /// </summary>
        public IEnumerable<FileInfo?> GetPythonFiles(string? sourceDirectory = null)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory))
            {
                if (System.Reflection.Assembly.GetEntryAssembly() == null)
                {
                    throw new ArgumentNullException(nameof(sourceDirectory));
                }

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


            Runtime.PythonDLL = _pythonVersion;

            try
            {
                using (Py.GIL()){}

                return true;
            } 
            catch(TypeInitializationException)
            {
                //Probably the wrong python version or python is not installed?
                return false;
            }
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
