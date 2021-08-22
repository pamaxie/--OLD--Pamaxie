﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;

#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Account 
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private bool DisableDeleteData { get; set; }
        private ProfileData? Profile { get; set; }
    }
}