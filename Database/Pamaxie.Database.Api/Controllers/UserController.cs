using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly TokenGenerator _generator;
        private readonly DatabaseService _dbService;

        public UserController(TokenGenerator generator, PamaxieDataContext context)
        {
            _generator = generator;
            _dbService = new DatabaseService(context);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpGet("get")]
        public ActionResult<IPamaxieUser> GetTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            //TODO get user from database

            return Ok(new PamaxieUser() { Key = result }); //TODO return user
        }

        /// <summary>
        /// Creates a new <see cref="IPamaxieUser"/>
        /// </summary>
        /// <returns>Created <see cref="IPamaxieUser"/></returns>
        [Authorize]
        [HttpPost("create")]
        public ActionResult<IPamaxieUser> CreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Create user in the db and return that created user

            return Ok(user); //TODO return created user, not the user from request body
        }

        /// <summary>
        /// Updates a <see cref="IPamaxieUser"/>
        /// </summary>
        /// <returns>Updated <see cref="IPamaxieUser"/></returns>
        [Authorize]
        [HttpPost("update")]
        public ActionResult<IPamaxieUser> UpdateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Update user in the db and return that updated user

            return Ok(user); //TODO return updated user, not the user from request body
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<IPamaxieUser> UpdateOrCreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Update or create user in the db and return that updated or created user

            return Ok(user); //TODO return updated or created user, not the user from request body
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("delete")]
        public ActionResult<IPamaxieUser> DeleteTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Delete user from the db, if deletion failed, return BadRequest

            return Ok("Success");
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("getAllApplications")]
        public ActionResult<IEnumerable<IPamaxieApplication>> GetAllApplicationsTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Get all applications the user owns from the database
            
            return Ok(new List<IPamaxieApplication>
            {
                new PamaxieApplication { Key = "1", OwnerKey = user.Key },
                new PamaxieApplication { Key = "2", OwnerKey = user.Key }
            }); //TODO return the list of applications the user owns from the database
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("verifyEmail")]
        public ActionResult<bool> VerifyEmailTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Verify the user in the database, and return the verified user
            
            return Ok(user.EmailVerified = true); //TODO return the verified user
        }
    }
}