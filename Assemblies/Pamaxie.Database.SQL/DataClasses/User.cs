using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pamaxie.Database.Sql.DataClasses
{
    public class User
    {
        [Key] 
        public ulong Id { get; set; }
        public string Username { get; set; }
        
        [EmailAddress] 
        public string Email { get; set; }

        [Phone] 
        public string PhoneNumber { get; set; }
    }
}