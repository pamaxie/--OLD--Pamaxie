using Microsoft.AspNetCore.Http;
using MudBlazor;
using Pamaxie.Extensions.Sql;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Sql.Data;
using Pamaxie.Website.Authentication;
using System.Security.Claims;

namespace Pamaxie.Website.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISnackbar _snackbar;

        public UserService(IHttpContextAccessor httpContextAccessor, ISnackbar snackbar)
        {
            _httpContextAccessor = httpContextAccessor;
            _snackbar = snackbar;
            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            _snackbar.Configuration.PreventDuplicates = true;
            _snackbar.Configuration.VisibleStateDuration = 10000;
            _snackbar.Configuration.HideTransitionDuration = 500;
            _snackbar.Configuration.ShowTransitionDuration = 500;
        }
        
        /// <summary>
        /// Checks if the current logged-in user have their email verified.
        /// </summary>
        /// <returns></returns>
        public bool IsEmailOfCurrentUserVerified()
        {
            ClaimsPrincipal? claimUser = _httpContextAccessor.HttpContext?.User;
            if (claimUser == null)
                return false;
            ProfileData? profile = claimUser.GetGoogleAuthData(out _)?.GetProfileData();
            if (profile == null)
                return false;
            User user = UserExtensions.GetUser(profile.GoogleClaimUserId);
            return user is {EmailVerified: true};
        }

        /// <summary>
        /// Shows a Snackbar dialog telling the user to verify their email address.
        /// </summary>
        public void ShowVerifyEmailDialog()
        {
            _snackbar.Add("Please verify your email before using our service", Severity.Info, config =>
            {
                config.HideIcon = true;
                config.SnackbarVariant = Variant.Filled;
            });
        }
    }
}