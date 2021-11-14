using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Data
{
    public interface ISecurityQuestions : IDatabaseObject
    {
        /// <summary>
        /// This holds the first of the three backup keys required to find the user data and regain access
        /// </summary>
        string BackupKey1 { get; set; }

        /// <summary>
        /// This holds the second of the three backup keys required to find the user data and regain access
        /// </summary>
        string BackupKey2 { get; set; }

        /// <summary>
        /// This holds the third of the three backup keys required to find the user data and regain access
        /// </summary>
        string BackupKey3 { get; set; }

        /// <summary>
        /// This holds the user id of the user that is related to all these backup keys
        /// </summary>
        string RelatedUserId { get; set; }
    }
}
