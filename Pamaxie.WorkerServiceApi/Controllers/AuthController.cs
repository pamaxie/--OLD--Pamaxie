using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;


namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Controller to handle application authentication
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly TokenGenerator _generator;


        public AuthController(TokenGenerator generator)
        {
            _generator = generator;
        }

        /// <summary>
        /// Signs in a user via Basic authentication and returns a token.
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for AppAuthCredentials</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthToken> LoginTask()
        {
            //TODO: Use basic auth here please, do not use a HTTPPost for login.
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(result))
                return BadRequest();

            PamaxieApplication appData = JsonConvert.DeserializeObject<PamaxieApplication>(result);

            if (string.IsNullOrWhiteSpace(appData?.Credentials.AuthorizationToken) || default == appData.UniqueKey)
                return Unauthorized();

            if (!appData.VerifyAuthentication())
                return Unauthorized();

            AuthToken token = _generator.CreateToken(appData.UniqueKey);
            return Ok(token);
        }

        /// <summary>
        /// Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("refresh")]
        public ActionResult<AuthToken> RefreshTask()
        {
            //TODO Not yet implemented
            StringValues token = Request.Headers["authorization"];
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Authentication token for refresh could not be found");

            string userId = TokenGenerator.GetUserKey(token);
            if (!UserDataServiceExtension.Exists(userId))
                return BadRequest("User does not exist");
            AuthToken newToken = _generator.CreateToken(userId);
            return Ok(newToken);
        }
    }
}