namespace Pamaxie.Data
{
    public interface IWebDomain
    {
        public string Url { get; set; }
        public DomainType DomainType { get; set; }
    }
}