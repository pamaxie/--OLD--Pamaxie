
namespace Pamaxie.Base.Python
{
    internal class TestPyInterop
    {
        /// <summary>
        /// This should basically run everything once, so we can validate everything works properly :P
        /// </summary>
        internal TestPyInterop() 
        {
            PyInterop.Instance.HasPython();
            PyInterop.Instance.GetPythonFiles();
        }

    }
}
