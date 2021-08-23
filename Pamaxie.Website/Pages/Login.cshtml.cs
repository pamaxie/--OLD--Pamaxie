using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pamaxie.Website.Pages
{
    /// <summary>
    /// Class for the Login ViewModel page
    /// </summary>
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Log in the user with a Google Account
        /// </summary>
        /// <param name="returnUrl">Page to return to</param>
        /// <returns><see cref="IActionResult"/></returns>
        public IActionResult OnGetAsync(string? returnUrl = null)
        {
            const string provider = "Google";
            // Request a redirect to the external login provider.
            AuthenticationProperties authenticationProperties = new()
            {
                RedirectUri = Url.Page("./Login", "Callback", new { returnUrl })
            };
            return new ChallengeResult(provider, authenticationProperties);
        }
        
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets the Google Account <see cref="ClaimsIdentity"/> after using Google's login page
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        public async Task<IActionResult> OnGetCallbackAsync()
        {
            // Get the information about the user from the external login provider
            ClaimsIdentity? googleUser = User.Identities.FirstOrDefault();
            if (googleUser is not {IsAuthenticated: true})
                return LocalRedirect("/");
            AuthenticationProperties authProperties = new()
            {
                IsPersistent = true,
                RedirectUri = Request.Host.Value
            };
            await HttpContext.SignInAsync( 
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(googleUser),
                authProperties);
            return LocalRedirect("/");
        }
    }
}
