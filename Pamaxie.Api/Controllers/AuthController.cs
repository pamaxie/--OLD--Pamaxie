using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Pamaxie.Database.Sql.DataClasses;
using Pamaxie.Extensions;
using System.IO;

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
        /// Signs in a user via Basic authentication and returns a token.
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for Authentication</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthToken> LoginTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            Application? appData = JsonConvert.DeserializeObject<Application>(result);

            if (string.IsNullOrEmpty(appData?.AppToken) || default == appData.ApplicationId)
                return Unauthorized(ErrorHandler.UnAuthorized());

            if (!appData.VerifyAuth()) return Unauthorized(ErrorHandler.UnAuthorized());

            AuthToken token = _generator.CreateToken(appData.ApplicationId.ToString());

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
            
            Application? appData = JsonConvert.DeserializeObject<Application>(result);
            
            if (string.IsNullOrEmpty(appData?.AppToken) || default == appData.ApplicationId)
                return Unauthorized(ErrorHandler.UnAuthorized());

            string userId = appData.ApplicationId.ToString();
            AuthToken token = _generator.CreateToken(userId);

            return Ok(token);
        }
    }
}