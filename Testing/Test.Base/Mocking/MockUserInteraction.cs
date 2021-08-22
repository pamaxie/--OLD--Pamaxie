using Pamaxie.Data;
using Pamaxie.Database.Extensions;
using Pamaxie.Database.Extensions.InteractionObjects;

namespace Test.Base
{
    public class MockUserInteraction : IUserInteraction
    {
        
        public PamaxieApplication Get(string key)
        {
            throw new System.NotImplementedException();
        }

        public PamaxieUser Create(PamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryCreate(PamaxieUser value, out PamaxieUser createdValue)
        {
            throw new System.NotImplementedException();
        }

        public PamaxieUser Update(PamaxieUser value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryUpdate(PamaxieUser value, out PamaxieUser updatedValue)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateOrCreate(PamaxieUser value, out PamaxieUser databaseValue)
        {
            throw new System.NotImplementedException();
        }
    }
}