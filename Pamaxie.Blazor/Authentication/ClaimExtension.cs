using Pamaxie.Blazor.Authentication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pamaxie.Blazor.Authentication
{
    public static class ClaimExtension
    {
        /// <summary>
        /// Gets the Google Authentication claim via the claims principle
        /// </summary>
        /// <param name="principle"><see cref="ClaimsPrincipal"/> to get the google claims from</param>
        /// <returns><see cref="GoogleAuthData"/> that was created via the claim values. Is <see cref="null"/> if something went wrong.</returns>
        public static GoogleAuthData GetGoogleAuthData(this ClaimsPrincipal principle)
        {
            if (principle.Identity == null)
                return null;

            if (!principle.Claims.Any())
                return null;

            var claims = principle.Claims.ToList();
            if (!claims.Any(x => x.Issuer == "Google"))
                return null;

            var googleClaim = new GoogleAuthData();
            foreach(var claim in claims.Where(x => x.Issuer == "Google"))
            {
                if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                {
                    googleClaim.UserId = claim.Value;
                }else if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                {
                    googleClaim.UserName = claim.Value;
                }else if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
                {
                    googleClaim.FirstName = claim.Value;
                }
                else if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")
                {
                    googleClaim.LastName = claim.Value;
                }
                else if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                {
                    googleClaim.EmailAddress = claim.Value;
                }
                else if (claim.Type == "urn:google:image")
                {
                    googleClaim.ProfilePictureAddress = claim.Value;
                }
            }
            return googleClaim;
        }
    }
}
