using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Website.Authentication;
using Pamaxie.Website.Models;

namespace Pamaxie.Website.Services
{
    /// <summary>
    /// A service for User interactions on the website
    /// </summary>
    public sealed class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISnackbar? _snackbar;

        private readonly string _secret;

        public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISnackbar? snackbar)
        {
            IConfigurationSection jwtTokenSection = configuration.GetSection("JwtToken");
            _secret = jwtTokenSection.GetValue<string>("Secret");
            _httpContextAccessor = httpContextAccessor;
            _snackbar = snackbar;
            if (_snackbar == null)
                return;
            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            _snackbar.Configuration.PreventDuplicates = true;
            _snackbar.Configuration.VisibleStateDuration = 10000;
            _snackbar.Configuration.HideTransitionDuration = 500;
            _snackbar.Configuration.ShowTransitionDuration = 500;
        }

        /// <summary>
        /// Checks if the current logged-in user have their email verified.
        /// </summary>
        /// <returns>If the email of the current user is verified</returns>
        public bool IsEmailOfCurrentUserVerified()
        {
            ClaimsPrincipal? claimUser = _httpContextAccessor.HttpContext?.User;
            if (claimUser is null)
                return false;
            PamaxieUser? googleUser = claimUser.GetGoogleAuthData(out _);
            if (googleUser == null)
                return false;
            PamaxieUser pamaxieUser = UserDataServiceExtension.Get(googleUser.UniqueKey);
            return pamaxieUser is { EmailVerified: true };
        }

        /// <summary>
        /// Generates a token for EmailConfirmation for a User
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Email Confirmation Token</returns>
        public string GenerateEmailConfirmationToken(PamaxieUser user)
        {
            IBody body = new ConfirmEmailBody(user);
            return JsonWebToken.Encode(body, _secret);
        }

        /// <summary>
        /// Confirms the email of a user from a GoogleUserId included in the token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Bool that show if the user's email is confirmed or not</returns>
        public bool ConfirmEmail(string token)
        {
            ConfirmEmailBody? body = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;
            if (body?.Purpose is not EmailPurpose.EMAIL_CONFIRMATION)
                return false;
            PamaxieUser pamaxieUser = UserDataServiceExtension.Get(body.User.UniqueKey);
            return pamaxieUser.EmailAddress == body.User.EmailAddress && body.User.VerifyEmail();
        }

        /// <summary>
        /// Shows a Snackbar dialog telling the user to verify their email address.
        /// </summary>
        public void ShowVerifyEmailDialog()
        {
            _snackbar?.Add("Please verify your email before using our service", Severity.Info, config =>
            {
                config.HideIcon = true;
                config.SnackbarVariant = Variant.Filled;
            });
        }
    }
}