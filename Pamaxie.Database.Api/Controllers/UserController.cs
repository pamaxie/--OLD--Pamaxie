using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;
using Pamaxie.Jwt;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Api Controller for handling all user interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly DatabaseService _dbService;

        public UserController(TokenGenerator generator, DatabaseService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Get a user from the database with a user key from the request body
        /// </summary>
        /// <returns>A <see cref="IPamaxieUser"/> from the database</returns>
        [Authorize]
        [HttpGet("get")]
        public ActionResult<IPamaxieUser> GetTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) 
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = _dbService.Users.Get(result);
            
            return Ok(user);
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
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser createdUser = _dbService.Users.Create(user);

            return Ok(createdUser);
        }

        /// <summary>
        /// Tries to create a new <see cref="IPamaxieUser"/>
        /// </summary>
        /// <returns>Created <see cref="IPamaxieUser"/></returns>
        [Authorize]
        [HttpPost("tryCreate")]
        public ActionResult<IPamaxieUser> TryCreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool created = _dbService.Users.TryCreate(user, out IPamaxieUser createdUser);

            return Ok(createdUser); //TODO find a solution to involve the boolean "created"
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
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser updatedUser = _dbService.Users.Update(user);

            return Ok(updatedUser);
        }

        /// <summary>
        /// Tries to update a <see cref="IPamaxieUser"/>
        /// </summary>
        /// <returns>Updated <see cref="IPamaxieUser"/></returns>
        [Authorize]
        [HttpPost("tryUpdate")]
        public ActionResult<IPamaxieUser> TryUpdateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool updated = _dbService.Users.TryUpdate(user, out IPamaxieUser updatedUser);

            return Ok(updatedUser); //TODO find a solution to involve the boolean "updated"
        }

        /// <summary>
        /// Updates or creates a <see cref="IPamaxieUser"/>
        /// </summary>
        /// <returns>Updated or created <see cref="IPamaxieUser"/></returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<IPamaxieUser> UpdateOrCreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool updateOrCreate = _dbService.Users.UpdateOrCreate(user, out IPamaxieUser updatedOrCreatedUser);

            return Ok(updatedOrCreatedUser); //TODO find a solution to involve the boolean "updatedOrCreated"
        }

        /// <summary>
        /// Deletes a <see cref="IPamaxieUser"/>
        /// </summary>
        /// <returns><see cref="bool"/> if <see cref="IPamaxieUser"/> is deleted</returns>
        [Authorize]
        [HttpPost("delete")]
        public ActionResult<bool> DeleteTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool deleted = _dbService.Users.Delete(user);

            return Ok(deleted);
        }

        /// <summary>
        /// Get all <see cref="IPamaxieApplication"/>s the user owns
        /// </summary>
        /// <returns>A list of all <see cref="IPamaxieApplication"/>s the user owns</returns>
        [Authorize]
        [HttpPost("getAllApplications")]
        public ActionResult<IEnumerable<IPamaxieApplication>> GetAllApplicationsTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) 
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            IEnumerable<IPamaxieApplication> applications = _dbService.Users.GetAllApplications(user);

            return Ok(applications);
        }

        /// <summary>
        /// Verifies the user's email address
        /// </summary>
        /// <returns><see cref="bool"/> if the user got verified</returns>
        [Authorize]
        [HttpPost("verifyEmail")]
        public ActionResult<bool> VerifyEmailTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool verified = _dbService.Users.VerifyEmail(user);
            
            return Ok(verified);
        }
    }
}