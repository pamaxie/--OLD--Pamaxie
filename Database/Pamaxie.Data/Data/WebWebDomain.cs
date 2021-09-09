using System;

namespace Pamaxie.Data
{
    public class WebWebDomain : IWebDomain
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DomainType DomainType { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime TTL { get; set; }
    }
}