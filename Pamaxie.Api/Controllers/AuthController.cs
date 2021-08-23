using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
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
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            PamaxieApplication? appData = JsonConvert.DeserializeObject<PamaxieApplication>(result);

            if (string.IsNullOrEmpty(appData?.Credentials.AuthorizationToken) || default == appData.Key)
                return Unauthorized(ErrorHandler.UnAuthorized());

            if (!appData.Credentials.VerifyAuthentication()) return Unauthorized(ErrorHandler.UnAuthorized());

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
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            
            if (string.IsNullOrEmpty(result))  return BadRequest(ErrorHandler.BadData());
            PamaxieApplication? appData;
            try
            {
                appData = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            }
            catch
            {
                return StatusCode(400);
            }
            
            if (default == appData?.Key) return BadRequest(ErrorHandler.UnAuthorized());

            string userId = appData.Key;
            AuthToken token = _generator.CreateToken(userId);
            return Ok(token);
        }
    }
}