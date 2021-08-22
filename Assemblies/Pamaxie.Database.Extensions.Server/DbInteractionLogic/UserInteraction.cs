using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Pamaxie.Database.Extensions.Server
{
    public class UserInteraction : IUserInteraction
    {
        /// <inheritdoc/>
        public PamaxieUser Get(string key)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public PamaxieUser Create(PamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(PamaxieUser value, out PamaxieUser createdValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public PamaxieUser Update(PamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(PamaxieUser value, out PamaxieUser updatedValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(PamaxieUser value, out PamaxieUser databaseValue)
        {
            throw new System.NotImplementedException();
        }
    }
}