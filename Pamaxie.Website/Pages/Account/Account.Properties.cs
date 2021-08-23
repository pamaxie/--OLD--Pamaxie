using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using Pamaxie.Data;

#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="Account"/>
    /// </summary>
    public partial class Account 
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        
        private bool DisableDeleteData { get; set; }
        private IPamaxieUser? User { get; set; }
    }
}