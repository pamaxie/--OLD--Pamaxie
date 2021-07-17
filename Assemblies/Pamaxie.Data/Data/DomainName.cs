using System.ComponentModel.DataAnnotations;

namespace Pamaxie.Data
{
    public class DomainName : IDomainName
    {
        [Key] public long Id { get; set; }
        public string Url { get; set; }
    }
}