using System;

namespace Pamaxie.Data
{
    /// <summary>
    ///     Data for Application specific things
    /// </summary>
    public sealed class PamaxieApplication : IPamaxieApplication
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime TTL { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public AppAuthCredentials Credentials { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string OwnerKey { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime LastAuth { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool RateLimited { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Deleted { get; set; }
    }
}