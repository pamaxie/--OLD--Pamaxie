using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Base
{
    /// <summary>
    /// Exception that is thrown if a filetype is not supported by our methods
    /// </summary>
    public class InvalidFileException : Exception
    {
        public InvalidFileException() { }

        public InvalidFileException(string message) : base(message) { }

        public InvalidFileException(string message, Exception inner) : base (message, inner) { }
    }
}
