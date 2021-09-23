using System;
using System.Reflection;

namespace Pamaxie.Helpers
{
    public static class UnitTest
    {
        /// <summary>
        /// <see cref="bool"/> for checking if the environment is running through unit testing
        /// </summary>
        public static bool IsRunningFromUnitTests { get; }

        static UnitTest()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName != null && assembly.FullName.ToLowerInvariant().StartsWith("xunit"))
                {
                    IsRunningFromUnitTests = true;
                    break;
                }
            }
        }
    }
}