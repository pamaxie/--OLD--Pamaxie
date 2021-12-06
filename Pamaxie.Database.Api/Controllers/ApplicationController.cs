using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Jwt;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Api Controller for handling all <see cref="PamaxieApplication"/> interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class ApplicationController : ControllerBase
    {

        private readonly IPamaxieDatabaseDriver _dbDriver;

        /// <summary>
        /// Constructor for <see cref="ApplicationController"/>
        /// </summary>
        /// <param name="dbDriver">Database Service</param>
        public ApplicationController(IPamaxieDatabaseDriver dbDriver)
        {
            _dbDriver = dbDriver;
        }

        /// <summary>
        /// Gets a <see cref="PamaxieApplication"/> from the database by a key
        /// </summary>
        /// <param name="key">Unique UniqueKey of the <see cref="PamaxieApplication"/></param>
        /// <returns>A <see cref="PamaxieApplication"/> from the database</returns>
        [Authorize]
        [HttpGet("Get={key}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> GetTask(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                return BadRequest();
            }

            if (!_dbDriver.Service.PamaxieApplicationData.Exists(key))
            {
                return NotFound();
            }

            return Ok(_dbDriver.Service.PamaxieApplicationData.Get(key));
        }

        /// <summary>
        /// Creates a new <see cref="PamaxieApplication"/> in the database
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to be created</param>
        /// <returns>Created <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> CreateTask(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            if (!validateApplicationOwner(application.UniqueKey, out application, application))
            {
                return Unauthorized();
            }

            return Created("", _dbDriver.Service.PamaxieApplicationData.Create(application));
        }

        /// <summary>
        /// Tries to create a new <see cref="PamaxieApplication"/> in the database
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to be created</param>
        /// <returns>Created <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("TryCreate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> TryCreateTask(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            if (!validateApplicationOwner(application.UniqueKey, out application, application))
            {
                return Unauthorized();
            }

            if (_dbDriver.Service.PamaxieApplicationData.TryCreate(application, out PamaxieApplication createdApplication))
            {
                return Created("", createdApplication);
            }

            return Problem();
        }

        /// <summary>
        /// Updates a <see cref="PamaxieApplication"/> in the database
        /// </summary>
        /// <param name="application">Updated values on <see cref="PamaxieApplication"/></param>
        /// <returns>Updated <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPut("Update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> UpdateTask(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            if (!validateApplicationOwner(application.UniqueKey, out application, application))
            {
                return Unauthorized();
            }

            return Ok(_dbDriver.Service.PamaxieApplicationData.Update(application));
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieApplication"/> in the database
        /// </summary>
        /// <param name="application">Updated values on <see cref="PamaxieApplication"/></param>
        /// <returns>Updated <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPut("TryUpdate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> TryUpdateTask(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            if (!validateApplicationOwner(application.UniqueKey, out application, application))
            {
                return Unauthorized();
            }

            if (_dbDriver.Service.PamaxieApplicationData.TryUpdate(application, out PamaxieApplication updatedApplication))
            {
                return Ok(updatedApplication);
            }

            return Problem();
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieApplication"/> in the database,
        /// if the <see cref="PamaxieApplication"/> does not exist, then a new one will be created
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to be created, or updated values on <see cref="PamaxieApplication"/></param>
        /// <returns>Updated or created <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("UpdateOrCreate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> UpdateOrCreateTask(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            if (_dbDriver.Service.PamaxieApplicationData.UpdateOrCreate(application, out PamaxieApplication updatedOrCreatedApplication))
            {
                return Created("", updatedOrCreatedApplication);
            }

            return Ok(updatedOrCreatedApplication);
        }

        /// <summary>
        /// Checks if a <see cref="PamaxieApplication"/> exists in the database
        /// </summary>
        /// <param name="key">Unique UniqueKey of the <see cref="PamaxieApplication"/></param>
        /// <returns><see cref="bool"/> if <see cref="PamaxieApplication"/> exists in the database</returns>
        [Authorize]
        [HttpGet("Exists={key}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> ExistsTask(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest();
            }

            return Ok(_dbDriver.Service.PamaxieApplicationData.Exists(key));
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieApplication"/> in the database
        /// </summary>
        /// <param name="applicationId">The Id of the application that should be deleted</param>
        /// <returns><see cref="bool"/> if <see cref="PamaxieApplication"/> is deleted</returns>
        [Authorize]
        [HttpDelete("Delete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteTask(string applicationId)
        {
            if (applicationId == null)
            {
                return BadRequest();
            }

            if (!validateApplicationOwner(applicationId, out var application))
            {
                return Unauthorized();
            }

            if (_dbDriver.Service.PamaxieApplicationData.Delete(application))
            {
                return Ok(true);
            }



            return Problem();
        }

        /// <summary>
        /// Gets the owner from a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to get owner from</param>
        /// <returns>The owner of the <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpGet("GetOwner")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> GetOwner(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            return Ok(_dbDriver.Service.PamaxieApplicationData.GetOwner(application));
        }

        /// <summary>
        /// Enables or disables the <see cref="PamaxieApplication"/> 
        /// </summary>
        /// <param name="applicationId">Id of the application that should be enabled or disabled</param>
        /// <returns>Enabled or disabled <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPut("EnableOrDisable")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> EnableOrDisableTask(string applicationId)
        {
            if (applicationId == null)
            {
                return BadRequest();
            }

            if (!validateApplicationOwner(applicationId, out var application))
            {
                return Unauthorized();
            }

            return Ok(_dbDriver.Service.PamaxieApplicationData.EnableOrDisable(application));
        }

        /// <summary>
        /// Verify if the <see cref="AppAuthCredentials"/> is authorized from a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">Application to verify authentication</param>
        /// <returns><see cref="bool"/> if the <see cref="PamaxieApplication"/>is authorized</returns>
        [Authorize]
        [HttpPost("VerifyAuthentication")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> VerifyAuthenticationTask(PamaxieApplication application)
        {
            if (application == null)
            {
                return BadRequest();
            }

            if (_dbDriver.Service.PamaxieApplicationData.VerifyAuthentication(application))
            {
                return Ok(true);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Validates if the person making changes to an application is its actual owner
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        private bool validateApplicationOwner(string applicationId, out PamaxieApplication application, PamaxieApplication applicationIn = null)
        {
            //Validate if the owner is the one trying to do this request.
            if (applicationIn == null)
            {
                application = _dbDriver.Service.PamaxieApplicationData.Get(applicationId);
            }
            else
            {
                application = applicationIn;
            }

            string token = Request.Headers["authorization"];
            string userId = TokenGenerator.GetUserKey(token);
            return application.OwnerKey == userId;
        }
    }
}