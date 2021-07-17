using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    public class LogoutModel : PageModel
    {
        public string ReturnUrl { get; private set; }
        public async Task<IActionResult> OnGetAsync(
            string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Clear the existing external cookie
            try
            {
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception)
            {
                // ignored
            }

            return LocalRedirect("/");
        }
    }
}
