using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pamaxie.Website.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        // ReSharper disable once UnusedMember.Global
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
