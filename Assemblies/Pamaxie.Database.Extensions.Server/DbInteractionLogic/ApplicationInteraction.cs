﻿using System;
using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Server
{
    /// <inheritdoc/>
    internal class ApplicationInteraction : IApplicationInteraction
    {
        /// <inheritdoc/>
        public IPamaxieApplication Get(string key)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc/>
        public IEnumerable<IPamaxieApplication> GetFromUser(string userKey)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieApplication Create(IPamaxieApplication value)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool TryCreate(IPamaxieApplication value, out IPamaxieApplication createdValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IPamaxieApplication Update(IPamaxieApplication value)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool TryUpdate(IPamaxieApplication value, out IPamaxieApplication updatedValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(IPamaxieApplication value, out IPamaxieApplication databaseValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Delete(IPamaxieApplication value)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc/>
        public bool EnableOrDisable(IPamaxieApplication value)
        {
            throw new NotImplementedException();
        }
    }
}