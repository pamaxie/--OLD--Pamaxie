using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;

namespace Pamaxie.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
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
            return Ok("Success");
        }

        /// <summary>
        /// Creates a new Api User
        /// </summary>
        /// <returns><see cref="string"/> Success?</returns>
        [Authorize]
        [HttpPost("createUser")]
        public ActionResult<string> CreateUserTask()
        {
            return Ok("Success");
        }


        /// <summary>
        /// Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("refresh")]
        public ActionResult<AuthToken> RefreshTask()
        {
            return Ok("Success");
        }
    }
}