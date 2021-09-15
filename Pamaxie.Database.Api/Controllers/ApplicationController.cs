using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;

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
        private readonly DatabaseService _dbService;

        /// <summary>
        /// Constructor for <see cref="ApplicationController"/>
        /// </summary>
        /// <param name="dbService">Database Service</param>
        public ApplicationController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Gets a <see cref="PamaxieApplication"/> from the database by a key
        /// </summary>
        /// <param name="key">Unique Key of the <see cref="PamaxieApplication"/></param>
        /// <returns>A <see cref="PamaxieApplication"/> from the database</returns>
        [Authorize]
        [HttpGet("{key}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> GetTask(string key)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (string.IsNullOrEmpty(key))
            {
                return BadRequest();
            }

            return Ok(_dbService.Applications.Get(key));
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
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            return Created("", _dbService.Applications.Create(application));
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
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            if (_dbService.Applications.TryCreate(application, out PamaxieApplication createdApplication))
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
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            return Ok(_dbService.Applications.Update(application));
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
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            if (_dbService.Applications.TryUpdate(application, out PamaxieApplication updatedApplication))
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
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            if (_dbService.Applications.UpdateOrCreate(application, out PamaxieApplication updatedOrCreatedApplication))
            {
                return Created("", updatedOrCreatedApplication);
            }

            return Ok(updatedOrCreatedApplication);
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieApplication"/> in the database
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to be deleted</param>
        /// <returns><see cref="bool"/> if <see cref="PamaxieApplication"/> is deleted</returns>
        [Authorize]
        [HttpDelete("Delete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteTask(PamaxieApplication application)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            if (_dbService.Applications.Delete(application))
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> GetOwner(PamaxieApplication application)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            return Ok(_dbService.Applications.GetOwner(application));
        }

        /// <summary>
        /// Enables or disables the <see cref="PamaxieApplication"/> 
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to enable or disable</param>
        /// <returns>Enabled or disabled <see cref="PamaxieApplication"/></returns>
        [Authorize]
        [HttpPost("EnableOrDisable")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieApplication))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieApplication> EnableOrDisableTask(PamaxieApplication application)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            return Ok(_dbService.Applications.EnableOrDisable(application));
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
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (application == null)
            {
                return BadRequest();
            }

            if (_dbService.Applications.VerifyAuthentication(application))
            {
                return Ok(true);
            }

            return Unauthorized();
        }
    }
}