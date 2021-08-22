using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.InteractionObjects
{
    /// <summary>
    /// Defines the behavior of <see cref="IAuthenticationInteraction"/>
    /// </summary>
    public interface IAuthenticationInteraction : IDatabaseInteraction<AppAuthCredentials>
    {
        
    }
}