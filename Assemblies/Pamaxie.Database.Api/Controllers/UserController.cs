using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Pamaxie.Database.Extensions.Server;

namespace Pamaxie.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly TokenGenerator _generator;
        private readonly DatabaseService _dbService;

        public UserController(TokenGenerator generator, PamaxieDataContext context)
        {
            _generator = generator;
            _dbService = new DatabaseService(context);
        }

        /// <summary>
        /// Gets a user via the entered key
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for Authentication</returns>
        [AllowAnonymous]
        [HttpGet("get={key}")]
        public ActionResult<AuthToken> LoginTask(string key)
        {
            return Ok("Success");
        }

        /// <summary>
        /// Creates a new Api User
        /// </summary>
        /// <returns><see cref="string"/> Sucess?</returns>
        [Authorize]
        [HttpPost("create")]
        public ActionResult<string> CreateUserTask()
        {
            return Ok("Success");
        }

        /// <summary>
        /// Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("update")]
        public ActionResult<AuthToken> UpdateTask()
        {
            return Ok("Success");
        }

        /// <summary>
        /// Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<AuthToken> UpdateOrCreateTask()
        {
            return Ok("Success");
        }
    }
}