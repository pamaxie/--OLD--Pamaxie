using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Authentication;
using Pamaxie.Data;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

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
        private readonly JwtTokenGenerator _generator;

        /// <summary>
        /// Constructor for <see cref="AuthController"/>
        /// </summary>
        /// <param name="generator">Token generator</param>
        public AuthController(JwtTokenGenerator generator)
        {
            _generator = generator;
        }

        /// <summary>
        /// Signs in a user via Basic authentication and returns a token.
        /// </summary>
        /// <returns><see cref="JwtToken"/> Token for Authentication</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<JwtToken> LoginTask()
        {
            return Task.FromResult<JwtToken>("https://api.pamaxie.com/login");
        }

        /// <summary>
        /// Creates a new Api User, needs to be unauthorized
        /// </summary>
        /// <returns><see cref="string"/> Success?</returns>
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> CreateUserTask()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return BadRequest("Please specify a user to create");
            }

            JObject googleSearch = JObject.Parse(body);
            var user = googleSearch["userData"].ToObject<PamaxieUser>();

            user.UniqueKey = PamaxieCryptoHelpers.GetUserId(new System.Net.NetworkCredential(user.UserName, user.Password));
            user.Password = Argon2.Hash(user.Password);
            user.EmailVerified = false;
            user.Disabled = false;
            user.TTL = DateTime.MaxValue;

            if (_dbDriver.Service.PamaxieUserData.Exists(user.UniqueKey))
            {
                return BadRequest("The specified user already exists in the database");
            }

            _dbDriver.Service.PamaxieUserData.Create(user);
            return Created(String.Empty, null);
        }

        /// <summary>
        /// Refreshes an exiting <see cref="JwtToken"/>
        /// </summary>
        /// <returns>Refreshed <see cref="JwtToken"/></returns>
        [Authorize]
        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<JwtToken> RefreshTask()
        {

            StringValues token = Request.Headers["authorization"];

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            string userId = JwtTokenGenerator.GetUserKey(token);

            //Validate if the user was maybe deleted since the last auth
            if (!_dbDriver.Service.PamaxieUserData.Exists(userId))
            {
                return Unauthorized();
            }

            var newToken = _generator.CreateToken(userId, ApiApplicationConfiguration.JwtSettings);
            return Ok(newToken);
        }
    }
}