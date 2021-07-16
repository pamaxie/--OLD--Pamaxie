using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Database.Sql.DataClasses;
using Pamaxie.Extensions;
using System.IO;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;

namespace Pamaxie.Api.Controllers
{
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
        ///     Signs in a user via Basic authentication and returns a token.
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for Authentication</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthToken> LoginTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            Application appData = JsonConvert.DeserializeObject<Application>(result);

            if (string.IsNullOrEmpty(appData?.AppToken) || default == appData.ApplicationId)
                return Unauthorized(ErrorHandler.UnAuthorized());

            if (!appData.VerifyAuth()) return Unauthorized(ErrorHandler.UnAuthorized());

            AuthToken token = _generator.CreateToken(appData.ApplicationId.ToString());
            if (token == null) return StatusCode(500);

            return Ok(token);
        }

        /// <summary>
        ///     Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("refresh")]
        public ActionResult<AuthToken> RefreshTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            
            if (string.IsNullOrEmpty(result))  return BadRequest(ErrorHandler.BadData());
            Application appData;
            try
            {
                appData = JsonConvert.DeserializeObject<Application>(result);
            }
            catch
            {
                return StatusCode(400);
            }
            
            if (default == appData?.ApplicationId) return BadRequest(ErrorHandler.UnAuthorized());

            string userId = appData.ApplicationId.ToString();
            AuthToken token = _generator.CreateToken(userId);

            if (token == null) return StatusCode(500);
            return Ok(token);
        }
    }
}