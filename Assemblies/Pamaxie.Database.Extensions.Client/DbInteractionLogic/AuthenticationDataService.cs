using System;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    internal class AuthenticationDataService : IAuthenticationDataService
    {
        /// <inheritdoc/>
        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public AppAuthCredentials Create(AppAuthCredentials value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(AppAuthCredentials value, out AppAuthCredentials createdValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public AppAuthCredentials Update(AppAuthCredentials value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(AppAuthCredentials value, out AppAuthCredentials updatedValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(AppAuthCredentials value, out AppAuthCredentials databaseValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Delete(AppAuthCredentials value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool VerifyAuthentication(AppAuthCredentials value)
        {
            throw new NotImplementedException();
        }
    }
}