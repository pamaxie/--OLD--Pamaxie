using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
