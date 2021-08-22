using Pamaxie.Data;
using Pamaxie.Database.Extensions;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Test.Base
{
    /// <inheritdoc/>
    public class MockApplicationInteraction : IApplicationInteraction
    {
        /// <inheritdoc/>
        public PamaxieApplication Get(string key)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public PamaxieApplication Create(PamaxieApplication value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(PamaxieApplication value, out PamaxieApplication createdValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public PamaxieApplication Update(PamaxieApplication value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(PamaxieApplication value, out PamaxieApplication updatedValue)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(PamaxieApplication value, out PamaxieApplication databaseValue)
        {
            throw new System.NotImplementedException();
        }
    }
}