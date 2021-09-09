using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pamaxie.MediaDetection
{
    internal static class FileTypeLocator
    {
        /// <summary>
        /// Used to Get the File types via Assembly Reflection
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<FileType> GetFileTypes() =>
            GetFileTypes(typeof(FileTypeLocator).GetTypeInfo().Assembly);

        /// <summary>
        /// Used to Get the File types via Assembly Reflection
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IEnumerable<FileType> GetFileTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly),
                    "Must send in an assembly to get the file types from");

            return assembly.GetTypes()
                .Where(t => typeof(FileType).IsAssignableFrom(t))
                .Where(t => !t.GetTypeInfo().IsAbstract)
                .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
                .Select(Activator.CreateInstance)
                .OfType<FileType>();
        }

        /// <summary>
        /// Used to Get the File types via Assembly Reflection
        /// </summary>
        /// <param name="assembly">The assembly to get</param>
        /// <param name="includeLocal">if the ones from this assembly should be included as well</param>
        /// <returns></returns>
        internal static IEnumerable<FileType> GetFileTypes(Assembly assembly, bool includeLocal)
        {
            IEnumerable<FileType> typesInAssembly = GetFileTypes(assembly);

            if (!includeLocal)
                return typesInAssembly;
            IEnumerable<FileType> typesThisAssembly = GetFileTypes();
            return typesInAssembly.Union(typesThisAssembly);

        }
    }
}