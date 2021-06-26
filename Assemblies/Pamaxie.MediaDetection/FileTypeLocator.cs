using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pamaxie.MediaDetection
{
    public static class FileTypeLocator
    {
        public static IEnumerable<FileType> GetFileTypes()
        {
            return GetFileTypes(typeof(FileTypeLocator).GetTypeInfo().Assembly);
        }
        
        public static IEnumerable<FileType> GetFileTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly),
                    "Must send in an assembly to get the file types from");
            }

            return assembly.GetTypes()
                .Where(t => typeof(FileType).IsAssignableFrom(t))
                .Where(t => !t.GetTypeInfo().IsAbstract)
                .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
                .Select(t => Activator.CreateInstance(t))
                .OfType<FileType>();
        }

        public static IEnumerable<FileType> GetFileTypes(Assembly assembly, bool includeDefaults)
        {
            var typesInAssembly = GetFileTypes(assembly);

            if (!includeDefaults)
            {
                return typesInAssembly;
            }
            else
            {
                var typesThisAssembly = GetFileTypes();
                return typesInAssembly.Union(typesThisAssembly);
            }
        }
    }
}