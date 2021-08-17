using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Test.Pamaxie.Website
{
    internal static class TestGoogleClaimData
    {
        private static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build().GetSection("GoogleClaim");

        internal static readonly Claim[] GoogleUserPrincipleClaims = {
            new(ClaimTypes.NameIdentifier, Configuration.GetValue<string>("NameIdentifier"),
                ClaimValueTypes.String, "Google"),
            new(ClaimTypes.Name, Configuration.GetValue<string>("Name"),
                ClaimValueTypes.String, "Google"),
            new(ClaimTypes.GivenName, Configuration.GetValue<string>("GivenName"),
                ClaimValueTypes.String, "Google"),
            new(ClaimTypes.Email, Configuration.GetValue<string>("Email"),
                ClaimValueTypes.String, "Google"),
            new("urn:google:image", Configuration.GetValue<string>("Image"),
                ClaimValueTypes.String, "Google")
        };
    }
}