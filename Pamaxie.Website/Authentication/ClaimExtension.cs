using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pamaxie.Data;

namespace Pamaxie.Website.Authentication
{
    /// <summary>
    /// Extension method for <see cref="ClaimsPrincipal"/>
    /// </summary>
    public static class ClaimExtension
    {
        /// <summary>
        /// Gets the Google AppAuthCredentials claim via the claims principle
        /// </summary>
        /// <param name="principle"><see cref="ClaimsPrincipal"/> to get the google claims from</param>
        /// <param name="hasAccount">If the google user have a account on the website</param>
        /// <returns><see cref="PamaxieUser"/> that was created via the claim values. Is null if something went wrong.</returns>
        public static PamaxieUser? GetGoogleAuthData(this ClaimsPrincipal principle, out bool hasAccount)
        {
            hasAccount = false;
            if (principle.Identity == null)
                return null;

            if (!principle.Claims.Any())
                return null;

            List<Claim> claims = principle.Claims.ToList();
            if (claims.All(x => x.Issuer != "Google"))
                return null;

            PamaxieUser googleClaim = new PamaxieUser();
            foreach (Claim claim in claims.Where(x => x.Issuer == "Google"))
            {
                switch (claim.Type)
                {
                    case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier":
                        googleClaim.UniqueKey = claim.Value;
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
            IPamaxieUser pamaxieUser = UserDataServiceExtension.Get(googleClaim.UniqueKey);
            if (pamaxieUser is not { Deleted: false })
                return googleClaim;
            hasAccount = true;
            googleClaim.UniqueKey = pamaxieUser.UniqueKey;
            return googleClaim;
        }
    }
}