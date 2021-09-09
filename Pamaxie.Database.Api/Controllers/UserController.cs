using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Api Controller for handling all user interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class UserController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly DatabaseService _dbService;

        public UserController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Get a user from the database with a user key from the request body
        /// </summary>
        /// <returns>A <see cref="PamaxieUser"/> from the database</returns>
        [Authorize]
        [HttpGet("get")]
        public ActionResult<PamaxieUser> GetTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = _dbService.Users.Get(result);

            return Ok(user);
        }

        /// <summary>
        /// Creates a new <see cref="PamaxieUser"/>
        /// </summary>
        /// <returns>Created <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("create")]
        public ActionResult<PamaxieUser> CreateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser createdUser = _dbService.Users.Create(user);

            return Ok(createdUser);
        }

        /// <summary>
        /// Tries to create a new <see cref="PamaxieUser"/>
        /// </summary>
        /// <returns>Created <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("tryCreate")]
        public ActionResult<PamaxieUser> TryCreateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool created = _dbService.Users.TryCreate(user, out PamaxieUser createdUser);

            return Ok(createdUser); //TODO find a solution to involve the boolean "created"
        }

        /// <summary>
        /// Updates a <see cref="PamaxieUser"/>
        /// </summary>
        /// <returns>Updated <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("update")]
        public ActionResult<PamaxieUser> UpdateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser updatedUser = _dbService.Users.Update(user);

            return Ok(updatedUser);
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieUser"/>
        /// </summary>
        /// <returns>Updated <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("tryUpdate")]
        public ActionResult<PamaxieUser> TryUpdateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool updated = _dbService.Users.TryUpdate(user, out PamaxieUser updatedUser);

            return Ok(updatedUser); //TODO find a solution to involve the boolean "updated"
        }

        /// <summary>
        /// Updates or creates a <see cref="PamaxieUser"/>
        /// </summary>
        /// <returns>Updated or created <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<PamaxieUser> UpdateOrCreateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool updateOrCreate = _dbService.Users.UpdateOrCreate(user, out PamaxieUser updatedOrCreatedUser);

            return Ok(updatedOrCreatedUser); //TODO find a solution to involve the boolean "updatedOrCreated"
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieUser"/>
        /// </summary>
        /// <returns><see cref="bool"/> if <see cref="PamaxieUser"/> is deleted</returns>
        [Authorize]
        [HttpPost("delete")]
        public ActionResult<bool> DeleteTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool deleted = _dbService.Users.Delete(user);

            return Ok(deleted);
        }

        /// <summary>
        /// Get all <see cref="PamaxieApplication"/>s the user owns
        /// </summary>
        /// <returns>A list of all <see cref="PamaxieApplication"/>s the user owns</returns>
        [Authorize]
        [HttpPost("getAllApplications")]
        public ActionResult<IEnumerable<PamaxieApplication>> GetAllApplicationsTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            IEnumerable<PamaxieApplication> applications = _dbService.Users.GetAllApplications(user);

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
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = JsonConvert.DeserializeObject<PamaxieUser>(result);
            if (user == null)
                return BadRequest(ErrorHandler.BadData());

            bool verified = _dbService.Users.VerifyEmail(user);

            return Ok(verified);
        }
    }
}