using System;
using Pamaxie.Data;

namespace Pamaxie.Website.Models
{
    /// <summary>
    /// JWT Token body
    /// </summary>
    public interface IBody
    {
        /// <summary>
        /// The purpose of the email
        /// </summary>
        public EmailPurpose Purpose { get; }

        /// <summary>
        /// Expiration date of the email
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// <inheritdoc cref="PamaxieUser"/>
        /// </summary>
        public IPamaxieUser User { get; }
    }
}