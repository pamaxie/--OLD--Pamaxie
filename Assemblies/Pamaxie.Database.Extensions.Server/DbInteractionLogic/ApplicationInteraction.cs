using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Server
{
    internal class ApplicationInteraction : IApplicationInteraction
    {
        /// <inheritdoc/>
        public IPamaxieApplication Get(string key)
        {
            throw new System.NotImplementedException();
        }
        
        /// <inheritdoc/>
        public IEnumerable<IPamaxieApplication> GetFromUser(string userKey)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieApplication Create(IPamaxieApplication value)
        {
            throw new System.NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool TryCreate(IPamaxieApplication value, out IPamaxieApplication createdValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieApplication Update(IPamaxieApplication value)
        {
            throw new System.NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool TryUpdate(IPamaxieApplication value, out IPamaxieApplication updatedValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(IPamaxieApplication value, out IPamaxieApplication databaseValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Delete(IPamaxieApplication value)
        {
            throw new System.NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool EnableOrDisable(IPamaxieApplication value)
        {
            throw new System.NotImplementedException();
        }
    }
}