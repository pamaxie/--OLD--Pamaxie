using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="MainLayout"/>
    /// </summary>
    public partial class MainLayout
    {
        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
    }
}