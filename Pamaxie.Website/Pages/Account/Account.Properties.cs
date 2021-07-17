using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using Pamaxie.Database.Extensions.Data;
#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Account 
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private bool DisableDeleteData { get; set; } = false;
        private ProfileData? Profile { get; set; }
    }
}