using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Client
{
    public class AuthenticationInteraction : IAuthenticationInteraction
    {
        /// <inheritdoc/>
        public AppAuthCredentials Get(string key)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public AppAuthCredentials Create(AppAuthCredentials value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(AppAuthCredentials value, out AppAuthCredentials createdValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public AppAuthCredentials Update(AppAuthCredentials value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(AppAuthCredentials value, out AppAuthCredentials updatedValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(AppAuthCredentials value, out AppAuthCredentials databaseValue)
        {
            throw new System.NotImplementedException();
        }
    }
}