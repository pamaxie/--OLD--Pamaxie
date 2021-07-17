using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class RedirectToLogin
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

    }
}