namespace Pamaxie.Data
{
    public interface IWebDomain : IDatabaseObject
    {
        /// <summary>
        /// The Domain that was recognized by our system
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// The Type of domain our system recognized it as
        /// </summary>
        public DomainType DomainType { get; set; }
    }
}