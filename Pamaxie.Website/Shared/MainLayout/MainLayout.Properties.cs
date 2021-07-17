using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Diagnostics.CodeAnalysis;
#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class MainLayout
    {
        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
    }
}