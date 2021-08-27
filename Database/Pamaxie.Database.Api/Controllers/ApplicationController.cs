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
    public class ApplicationController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly TokenGenerator _generator;
        private readonly DatabaseService _dbService;


        public ApplicationController(TokenGenerator generator, PamaxieDataContext context)
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
        public ActionResult<IPamaxieApplication> GetTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            //TODO get application from database

            return Ok(new PamaxieApplication() { Key = result }); //TODO return application
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
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Create application in the db and return that created application

            return Ok(application); //TODO return created application, not the application from request body
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
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Update application in the db and return that updated application

            return Ok(application); //TODO return updated application, not the application from request body
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("updateOrCreate")]
        public ActionResult<IPamaxieApplication> UpdateOrCreateTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Update or create application in the db and return that updated or created application

            return Ok(application); //TODO return updated or created application, not the application from request body
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("delete")]
        public ActionResult<IPamaxieApplication> DeleteTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Delete application from the db, if deletion failed, return BadRequest

            return Ok("Success");
        }
        
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("enableOrUpdate")]
        public ActionResult<IPamaxieApplication> EnableOrDisableTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO enable or disable the application from the db
            application.Disabled = !application.Disabled; //TODO Find out if the enable/disable function should be in .Client, .Server or the Api

            return Ok(application); //TODO return enabled or disabled application, not the application from request body
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [Authorize]
        [HttpPost("verifyAuthentication")]
        public ActionResult<bool> VerifyAuthenticationTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());

            IPamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(result);
            if (application == null)
                return BadRequest(ErrorHandler.BadData());

            //TODO Verify application with the application.Credentials, if verification fails, return false

            return Ok(true); //TODO return true of false depending on, if the verification failed or succeeded
        }
    }
}