using System;

namespace Pamaxie.Data
{
    public sealed class WebWebDomain : IWebDomain
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
        public string UniqueKey { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime TTL { get; set; }
    }
}