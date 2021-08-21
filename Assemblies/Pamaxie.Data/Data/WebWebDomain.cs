using System;

namespace Pamaxie.Data
{
    public class WebWebDomain : IWebDomain, IDatabaseObject
    {
        public string Key { get; set; }
        public DateTime TTL { get; set; }
        public string Url { get; set; }
        public DomainType DomainType { get; set; }
    }
}