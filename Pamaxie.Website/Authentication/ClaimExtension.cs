using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pamaxie.Database.Sql.DataClasses;
using Pamaxie.Extensions;
using Pamaxie.Website.Authentication.Data;

namespace Pamaxie.Website.Authentication
{
    internal static class ClaimExtension
    {
        /// <summary>
        /// Gets the Google Authentication claim via the claims principle
        /// </summary>
        /// <param name="principle"><see cref="ClaimsPrincipal"/> to get the google claims from</param>
        /// <param name="hasAccount"></param>
        /// <returns><see cref="GoogleAuthData"/> that was created via the claim values. Is <see cref="null"/> if something went wrong.</returns>
        internal static GoogleAuthData GetGoogleAuthData(this ClaimsPrincipal principle, out bool hasAccount)
        {
            hasAccount = false;
            if (principle.Identity == null)
                return null;

            if (!principle.Claims.Any())
                return null;

            List<Claim> claims = principle.Claims.ToList();
            if (claims.All(x => x.Issuer != "Google"))
                return null;

            GoogleAuthData googleClaim = new();
            foreach(Claim claim in claims.Where(x => x.Issuer == "Google"))
            {
                switch (claim.Type)
                {
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier":
                        googleClaim.GoogleClaimUserId = claim.Value;
                        break;
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name":
                        googleClaim.UserName = claim.Value;
                        break;
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname":
                        googleClaim.FirstName = claim.Value;
                        break;
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname":
                        googleClaim.LastName = claim.Value;
                        break;
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                        googleClaim.EmailAddress = claim.Value;
                        break;
                    case "urn:google:image":
                        googleClaim.ProfilePictureAddress = claim.Value;
                        break;
                }
            }

            //Check if user exists if yes get their id if no create one!
            User user = UserExtensions.GetUser(googleClaim.GoogleClaimUserId);
            if (user is not {DeletedAccount: false}) return googleClaim;
            hasAccount = true;
            googleClaim.Id = user.Id;
            return googleClaim;
        }
    }
}
