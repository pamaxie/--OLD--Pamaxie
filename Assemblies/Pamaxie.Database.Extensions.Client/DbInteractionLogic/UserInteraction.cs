using System;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    internal class UserInteraction : IUserInteraction
    {
        /// <inheritdoc/>
        public IPamaxieUser Get(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieUser Create(IPamaxieUser value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(IPamaxieUser value, out IPamaxieUser createdValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieUser Update(IPamaxieUser value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(IPamaxieUser value, out IPamaxieUser updatedValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(IPamaxieUser value, out IPamaxieUser databaseValue)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool Delete(IPamaxieUser value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool VerifyEmail(IPamaxieUser value)
        {
            throw new NotImplementedException();
        }
    }
}