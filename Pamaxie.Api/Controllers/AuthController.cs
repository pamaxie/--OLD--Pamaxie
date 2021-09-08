using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Jwt;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Controller to handle application authentication
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
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
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            PamaxieApplication? appData = JsonConvert.DeserializeObject<PamaxieApplication>(result);

            if (string.IsNullOrEmpty(appData?.Credentials.AuthorizationToken) || default == appData.Key)
                return Unauthorized(ErrorHandler.UnAuthorized());

            if (!appData.VerifyAuthentication()) return Unauthorized(ErrorHandler.UnAuthorized());

            AuthToken token = _generator.CreateToken(appData.Key);
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
            if (string.IsNullOrEmpty(token))
                return BadRequest("Authentication token for refresh could not be found");

            var userId = TokenGenerator.GetUserKey(token);
            UserDataServiceExtension.Exists(userId);
            AuthToken newToken = _generator.CreateToken(userId);
            return Ok(newToken);
        }
    }
}