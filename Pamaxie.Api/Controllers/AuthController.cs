﻿using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public ActionResult<AuthToken> LoginTask(PamaxieApplication application)
        {
            //TODO: Use basic auth here please, do not use a HTTPPost for login.
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return StatusCode(StatusCodes.Status400BadRequest);

            PamaxieApplication? appData = JsonConvert.DeserializeObject<PamaxieApplication>(result);

            if (string.IsNullOrEmpty(appData?.Credentials.AuthorizationToken) || default == appData.Key)
                return StatusCode(StatusCodes.Status401Unauthorized);

            if (!appData.VerifyAuthentication())
                return StatusCode(StatusCodes.Status401Unauthorized);

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

            string userId = TokenGenerator.GetUserKey(token);
            UserDataServiceExtension.Exists(userId);
            AuthToken newToken = _generator.CreateToken(userId);
            return Ok(newToken);
        }
    }
}