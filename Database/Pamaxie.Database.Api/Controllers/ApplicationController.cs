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
    /// Api Controller for handling all application interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
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
        /// <returns>A <see cref="IPamaxieApplication"/> from the database</returns>
        [Authorize]
        [HttpGet("get")]
        public ActionResult<IPamaxieApplication> GetTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = _dbService.Applications.Get(result);
            
            return Ok(application);
        }

        /// <summary>
        /// Creates a new <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns>Created <see cref="IPamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("create")]
        public ActionResult<IPamaxieApplication> CreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication createdApplication = _dbService.Applications.Create(application);

            return Ok(createdApplication);
        }

        /// <summary>
        /// Tries to create a new <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns>Created <see cref="IPamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("tryCreate")]
        public ActionResult<IPamaxieApplication> TryCreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool created = _dbService.Applications.TryCreate(application, out IPamaxieApplication createdApplication);

            return Ok(createdApplication); //TODO find a solution to involve the boolean "created"
        }

        /// <summary>
        /// Updates a <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns>Updated <see cref="IPamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("update")]
        public ActionResult<IPamaxieApplication> UpdateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication updatedApplication = _dbService.Applications.Update(application);

            return Ok(updatedApplication);
        }

        /// <summary>
        /// Tries to update a <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns>Updated <see cref="IPamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("tryUpdate")]
        public ActionResult<IPamaxieApplication> TryUpdateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool updated = _dbService.Applications.TryUpdate(application, out IPamaxieApplication updatedApplication);

            return Ok(updatedApplication); //TODO find a solution to involve the boolean "updated"
        }

        /// <summary>
        /// Updates or creates a <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns>Updated or created <see cref="IPamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<IPamaxieApplication> UpdateOrCreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool updatedOrCreated =
                _dbService.Applications.UpdateOrCreate(application,
                    out IPamaxieApplication updatedOrCreatedApplication);

            return Ok(updatedOrCreatedApplication); //TODO find a solution to involve the boolean "updatedOrCreated"
        }

        /// <summary>
        /// Deletes a <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns><see cref="bool"/> if <see cref="IPamaxieApplication"/> is deleted</returns>
        [Authorize]
        [HttpPost("delete")]
        public ActionResult<bool> DeleteTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool deleted = _dbService.Applications.Delete(application);

            return Ok(deleted);
        }

        /// <summary>
        /// Gets the owner from a <see cref="IPamaxieApplication"/>
        /// </summary>
        /// <returns>The owner of the application</returns>
        [Authorize]
        [HttpPost("getOwner")]
        public ActionResult<IPamaxieUser> GetOwner()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<IPamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            IPamaxieUser user = _dbService.Applications.GetOwner(application);

            return Ok(user);
        }
        
        /// <summary>
        /// Enables or disables the <see cref="IPamaxieApplication"/> 
        /// </summary>
        /// <returns>Enabled or disabled <see cref="IPamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("enableOrDisable")]
        public ActionResult<IPamaxieApplication> EnableOrDisableTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication enabledOrDisabledApplication = _dbService.Applications.EnableOrDisable(application);

            return Ok(enabledOrDisabledApplication);
        }
        
        /// <summary>
        /// Verify if the <see cref="IPamaxieApplication"/> is authorized
        /// </summary>
        /// <returns><see cref="bool"/> if the <see cref="IPamaxieApplication"/>is authorized</returns>
        [Authorize]
        [HttpPost("verifyAuthentication")]
        public ActionResult<bool> VerifyAuthenticationTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result))
                return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            bool authorized = _dbService.Applications.VerifyAuthentication(application);

            return Ok(authorized);
        }
    }
}   