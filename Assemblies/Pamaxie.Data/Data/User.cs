using System.ComponentModel.DataAnnotations;

namespace Pamaxie.Data
{
    public class User
    {
        [Key] public long Id { get; set; }
        public string GoogleUserId { get; set; }
        public string Username { get; set; }
        [EmailAddress] public string Email { get; set; }
        public bool EmailVerified { get; set; } = false;
        public bool DeletedAccount { get; set; }
    }
}