namespace Pamaxie.Data
{
    //TODO: this needs to be added to the database with the required adapter and hash spec (so we can seperate it from other things)
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