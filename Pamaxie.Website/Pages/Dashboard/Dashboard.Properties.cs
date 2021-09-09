using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using Pamaxie.Data;
using Pamaxie.Website.Services;

#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="Dashboard"/>
    /// </summary>
    public partial class Dashboard
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private UserService UserService { get; set; }

        private List<PamaxieApplication> Applications { get; set; } = new List<PamaxieApplication>();
        private PamaxieApplication? NewApplication { get; set; } = null;
        private PamaxieUser? User { get; set; }
        private bool AcceptedTerms { get; set; }
        private bool AcceptedTos { get; set; }
        private MudTextField<string> PwField1 { get; set; }
    }
}