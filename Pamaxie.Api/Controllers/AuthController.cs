using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Jwt;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Controller to handle <see cref="PamaxieApplication"/> authentication
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
        /// Signs in a <see cref="PamaxieApplication"/> via Basic authentication and returns a <see cref="AuthToken"/>.
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for <see cref="AppAuthCredentials"/></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthToken))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AuthToken> LoginTask(PamaxieApplication application)
        {
            //TODO: Use basic auth here please, do not use a HTTPPost for login.
            if (string.IsNullOrEmpty(application.Credentials.AuthorizationToken) || default == application.UniqueKey)
            {
                return Unauthorized();
            }

            if (!application.VerifyAuthentication())
            {
                return Unauthorized();
            }

            AuthToken token = _generator.CreateToken(application.UniqueKey);
            return Ok(token);
        }

        /// <summary>
        /// Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthToken))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AuthToken> RefreshTask()
        {
            //TODO Not yet implemented
            StringValues token = Request.Headers["authorization"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Authentication token for refresh could not be found");
            }

            string userId = TokenGenerator.GetUserKey(token);

            if (!UserDataServiceExtension.Exists(userId))
            {
                return BadRequest("Invalid token");
            }

            AuthToken newToken = _generator.CreateToken(userId);
            return Ok(newToken);
        }
    }
}