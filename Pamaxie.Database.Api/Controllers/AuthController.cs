using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
        private readonly TokenGenerator _generator;
        private readonly DatabaseService _dbService;

        /// <summary>
        /// Constructor for <see cref="AuthController"/>
        /// </summary>
        /// <param name="generator">Token generator</param>
        /// <param name="dbService">Database Service</param>
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
        [HttpPost("Login")]
        //[Consumes(MediaTypeNames.Application.Json)] Use if a param is added
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AuthToken> LoginTask()
        {
            //TODO Not yet implemented
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            //if ({Value to use} == null)
            //if (string.IsNullOrEmpty({Value to use}))
            //{
            //    return BadRequest();
            //}

            //if ({Logic on the user that will be logged in and return a bool})
            //{
            //    return Status202Accepted();
            //}

            return Unauthorized();
        }

        /// <summary>
        /// Creates a new Api User, needs to be unauthorized
        /// </summary>
        /// <returns><see cref="string"/> Success?</returns>
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        //[Consumes(MediaTypeNames.Application.Json)] Use if a param is added
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> CreateUserTask()
        {
            //TODO Not yet implemented
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            //if ({Value to use} == null)
            //if (string.IsNullOrEmpty({Value to use}))
            //{
            //    return BadRequest();
            //}

            //if ({Logic on the user that will be created and return a bool and out the user})
            //{
            //    return Created("https://pamaxie.com/auth/", {the created user});
            //}

            return Problem();
        }

        /// <summary>
        /// Refreshes an exiting <see cref="AuthToken"/>
        /// </summary>
        /// <returns>Refreshed <see cref="AuthToken"/></returns>
        [Authorize]
        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AuthToken> RefreshTask()
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            StringValues token = Request.Headers["authorization"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            string userId = TokenGenerator.GetUserKey(token);

            if (_dbService.Users.Exists(userId))
            {
                return Unauthorized();
            }

            AuthToken newToken = _generator.CreateToken(userId);
            return Ok(newToken);
        }
    }
}