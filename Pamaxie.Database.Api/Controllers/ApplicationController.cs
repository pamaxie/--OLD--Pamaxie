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
    /// Api Controller for handling all application interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class ApplicationController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly DatabaseService _dbService;

        public ApplicationController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Gets a application from the database with a application key from the request body
        /// </summary>
        /// <returns>A <see cref="PamaxieApplication"/> from the database</returns>
        [Authorize]
        [HttpGet("get")]
        public ActionResult<PamaxieApplication> GetTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());
            result = result.Replace("\"", string.Empty);
            
            PamaxieApplication application = _dbService.Applications.Get(result);

            return Ok(application);
        }

        /// <summary>
        /// Creates a new <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns>Created <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("create")]
        public ActionResult<PamaxieApplication> CreateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication createdApplication = _dbService.Applications.Create(application);

            return Ok(createdApplication);
        }

        /// <summary>
        /// Tries to create a new <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns>Created <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("tryCreate")]
        public ActionResult<PamaxieApplication> TryCreateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool created = _dbService.Applications.TryCreate(application, out PamaxieApplication createdApplication);

            return Ok(createdApplication); //TODO find a solution to involve the boolean "created"
        }

        /// <summary>
        /// Updates a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns>Updated <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("update")]
        public ActionResult<PamaxieApplication> UpdateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication updatedApplication = _dbService.Applications.Update(application);

            return Ok(updatedApplication);
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns>Updated <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("tryUpdate")]
        public ActionResult<PamaxieApplication> TryUpdateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool updated = _dbService.Applications.TryUpdate(application, out PamaxieApplication updatedApplication);

            return Ok(updatedApplication); //TODO find a solution to involve the boolean "updated"
        }

        /// <summary>
        /// Updates or creates a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns>Updated or created <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<PamaxieApplication> UpdateOrCreateTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool updatedOrCreated =
                _dbService.Applications.UpdateOrCreate(application,
                    out PamaxieApplication updatedOrCreatedApplication);

            return Ok(updatedOrCreatedApplication); //TODO find a solution to involve the boolean "updatedOrCreated"
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns><see cref="bool"/> if <see cref="PamaxieApplication"/> is deleted</returns>
        [Authorize]
        [HttpPost("delete")]
        public ActionResult<bool> DeleteTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool deleted = _dbService.Applications.Delete(application);

            return Ok(deleted);
        }

        /// <summary>
        /// Gets the owner from a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <returns>The owner of the application</returns>
        [Authorize]
        [HttpPost("getOwner")]
        public ActionResult<PamaxieUser> GetOwner()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            PamaxieUser user = _dbService.Applications.GetOwner(application);

            return Ok(user);
        }

        /// <summary>
        /// Enables or disables the <see cref="PamaxieApplication"/> 
        /// </summary>
        /// <returns>Enabled or disabled <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("enableOrDisable")]
        public ActionResult<PamaxieApplication> EnableOrDisableTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication enabledOrDisabledApplication = _dbService.Applications.EnableOrDisable(application);

            return Ok(enabledOrDisabledApplication);
        }

        /// <summary>
        /// Verify if the <see cref="PamaxieApplication"/> is authorized
        /// </summary>
        /// <returns><see cref="bool"/> if the <see cref="PamaxieApplication"/>is authorized</returns>
        [Authorize]
        [HttpPost("verifyAuthentication")]
        public ActionResult<bool> VerifyAuthenticationTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool authorized = _dbService.Applications.VerifyAuthentication(application);

            return Ok(authorized);
        }
    }
}