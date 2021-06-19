using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pamaxie.Database.Sql.DataClasses
{
    public class User
    {
        [Key] 
        public long Id { get; set; }
        public string GoogleUserId { get; set; }
        public string Username { get; set; }
        
        [EmailAddress] 
        public string Email { get; set; }

        public bool DeletedAccount { get; set; }
    }
}