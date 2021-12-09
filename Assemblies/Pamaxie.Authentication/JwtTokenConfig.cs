using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Authentication
{
    public class JwtTokenConfig
    {
        /// <summary>
        /// Secret of the Authenication Settings
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Timeout of the Authenication settings
        /// </summary>
        public int ExpiresInMinutes { get; set; }
    }
}
