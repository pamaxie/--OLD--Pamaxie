using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Client
{
    internal class UserInteraction : IUserInteraction
    {
        /// <inheritdoc/>
        public IPamaxieUser Get(string key)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieUser Create(IPamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(IPamaxieUser value, out IPamaxieUser createdValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieUser Update(IPamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(IPamaxieUser value, out IPamaxieUser updatedValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(IPamaxieUser value, out IPamaxieUser databaseValue)
        {
            throw new System.NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool Delete(IPamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool VerifyEmail(IPamaxieUser value)
        {
            throw new System.NotImplementedException();
        }
    }
}