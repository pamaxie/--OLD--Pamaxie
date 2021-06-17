using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pamaxie.Database.Sql.DataClasses
{
    /// <summary>
    ///     Data for Application specific things
    /// </summary>
    public class Application
    {
        [Key]
        public ulong ApplicationId { get; set; }
        [NotMapped]
        public string AppToken { get; set; }

        public ulong UserId { get; set; }
        public string AppTokenHash { get; set; }
    }
}