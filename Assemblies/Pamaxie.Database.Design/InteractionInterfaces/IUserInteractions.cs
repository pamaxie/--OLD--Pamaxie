using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.InteractionObjects
{
    /// <summary>
    /// Defines the behavior of <see cref="IUserInteraction"/>
    /// </summary>
    public interface IUserInteraction : IDatabaseInteraction<PamaxieUser>
    {
        
    }
}