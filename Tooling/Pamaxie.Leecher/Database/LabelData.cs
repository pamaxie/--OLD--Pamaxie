using System.ComponentModel.DataAnnotations;

namespace Pamaxie.Leecher.Database
{
    public class LabelData
    {
        [Key]
        public long Id { get; set; }

        public string Url { get; set; }
        public string Content { get; set; }
        public string UrlType { get; set; }
    }
}
