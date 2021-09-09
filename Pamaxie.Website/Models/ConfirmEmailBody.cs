using System;
using Pamaxie.Data;

namespace Pamaxie.Website.Models
{
    /// <summary>
    /// Confirmation email body, used for the JWT Token
    /// </summary>
    public sealed class ConfirmEmailBody : IBody
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public EmailPurpose Purpose => EmailPurpose.EMAIL_CONFIRMATION;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime Expiration { get; set; } = DateTime.UtcNow.AddDays(10);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPamaxieUser User { get; }

        public ConfirmEmailBody(IPamaxieUser user)
        {
            User = user;
        }
    }
}