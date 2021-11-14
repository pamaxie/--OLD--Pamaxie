using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Data
{
    public class SecurityQuestions : ISecurityQuestions
    {
        /// <inheritdoc/>
        public string BackupKey1 { get; set; }

        /// <inheritdoc/>
        public string BackupKey2 { get; set; }

        /// <inheritdoc/>
        public string BackupKey3 { get; set; }

        /// <inheritdoc/>
        public string RelatedUserId { get; set; }

        /// <inheritdoc/>
        public string UniqueKey { get; set; }

        /// <inheritdoc/>
        public DateTime TTL { get; set; }
    }
}
