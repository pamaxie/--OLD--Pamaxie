using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Pamaxie.Api.Data;
using Pamaxie.Database.Extensions.Server;
using Pamaxie.Jwt;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Api Controller for handling all authentication interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class AuthController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly TokenGenerator _generator;
        private readonly DatabaseService _dbService;

        public AuthController(TokenGenerator generator, DatabaseService dbService)
        {
            _generator = generator;
            _dbService = dbService;
        }

        /// <summary>
        /// Signs in a user via Basic authentication and returns a token.
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for Authentication</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthToken> LoginTask()
        {
            //TODO Not yet implemented


            return Accepted("Success");
        }

        /// <summary>
        /// Creates a new Api User, needs to be unauthorized
        /// </summary>
        /// <returns><see cref="string"/> Success?</returns>
        [AllowAnonymous]
        [HttpPost("createUser")]
        public ActionResult<string> CreateUserTask()
        {
            //TODO Not yet implemented
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            return Created("https://pamaxie.com/auth/", string.Empty);
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
                return Unauthorized("Invalid authorization token");

            var userId = TokenGenerator.GetUserKey(token);
            if (_dbService.Users.Exists(userId)) return Unauthorized("Invalid authorization token");
            AuthToken newToken = _generator.CreateToken(userId);
            return Ok(newToken);
        }
    }
}