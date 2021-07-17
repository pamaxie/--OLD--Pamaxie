using System.ComponentModel.DataAnnotations;

namespace Pamaxie.Data
{
    public interface IDomainName
    {
        [Key] public long Id { get; set; }
        public string Url { get; set; }
    }
}