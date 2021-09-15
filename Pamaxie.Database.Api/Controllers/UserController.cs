using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Api Controller for handling all <see cref="PamaxieUser"/> interactions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class UserController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        /// <summary>
        /// Constructor for <see cref="UserController"/>
        /// </summary>
        /// <param name="dbService">Database Service</param>
        public UserController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Get a <see cref="PamaxieUser"/> from the database by a key
        /// </summary>
        /// <param name="key">Unique Key of the <see cref="PamaxieUser"/></param>
        /// <returns>A <see cref="PamaxieUser"/> from the database</returns>
        [Authorize]
        [HttpGet("{key}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> GetTask(string key)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (string.IsNullOrEmpty(key))
            {
                return BadRequest();
            }

            return Ok(_dbService.Users.Get(key));
        }

        /// <summary>
        /// Creates a new <see cref="PamaxieUser"/> in the database
        /// </summary>
        /// <param name="user"><see cref="PamaxieUser"/> to be created</param>
        /// <returns>Created <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> CreateTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            return Created("", _dbService.Users.Create(user));
        }

        /// <summary>
        /// Tries to create a new <see cref="PamaxieUser"/> in the database
        /// </summary>
        /// <param name="user"><see cref="PamaxieUser"/> to be created</param>
        /// <returns>Created <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("TryCreate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> TryCreateTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            if (_dbService.Users.TryCreate(user, out PamaxieUser createdUser))
            {
                return Created("", createdUser);
            }

            return Problem();
        }

        /// <summary>
        /// Updates a <see cref="PamaxieUser"/> in the database
        /// </summary>
        /// <param name="user">Updated values on <see cref="PamaxieUser"/></param>
        /// <returns>Updated <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPut("Update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> UpdateTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(_dbService.Users.Update(user));
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieUser"/> in the database
        /// </summary>
        /// <param name="user">Updated values on <see cref="PamaxieUser"/></param>
        /// <returns>Updated <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPut("TryUpdate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> TryUpdateTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            if (_dbService.Users.TryUpdate(user, out PamaxieUser updatedUser))
            {
                return Ok(updatedUser);
            }

            return Problem();
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieUser"/> in the database,
        /// if the <see cref="PamaxieUser"/> does not exist, then a new one will be created
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to be created, or updated values on <see cref="PamaxieUser"/></param>
        /// <returns>Updated or created <see cref="PamaxieUser"/></returns>
        [Authorize]
        [HttpPost("UpdateOrCreate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PamaxieUser> UpdateOrCreateTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            if (_dbService.Users.UpdateOrCreate(user, out PamaxieUser updatedOrCreatedUser))
            {
                return Created("", updatedOrCreatedUser);
            }

            return Ok(updatedOrCreatedUser);
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieUser"/> in the database
        /// </summary>
        /// <param name="user"><see cref="PamaxieUser"/> to be deleted</param>
        /// <returns><see cref="bool"/> if <see cref="PamaxieUser"/> is deleted</returns>
        [Authorize]
        [HttpDelete("Delete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PamaxieUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            if (_dbService.Users.Delete(user))
            {
                return Ok(true);
            }

            return Problem();
        }

        /// <summary>
        /// Get all <see cref="PamaxieApplication"/>s the user owns
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> of the applications</param>
        /// <returns>A list of all <see cref="PamaxieApplication"/>s the <see cref="PamaxieUser"/> owns</returns>
        [Authorize]
        [HttpGet("GetAllApplications")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PamaxieApplication>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<PamaxieApplication>> GetAllApplicationsTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(_dbService.Users.GetAllApplications(user));
        }

        /// <summary>
        /// Verifies the <see cref="PamaxieUser"/>'s email address
        /// </summary>
        /// <param name="user"><see cref="PamaxieUser"/> to be deleted</param>
        /// <returns><see cref="bool"/> if the user got verified</returns>
        [Authorize]
        [HttpPost("VerifyEmail")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> VerifyEmailTask(PamaxieUser user)
        {
            if (!_dbService.ConnectionSuccess)
            {
                return Problem();
            }

            if (user == null)
            {
                return BadRequest();
            }

            if (_dbService.Users.VerifyEmail(user))
            {
                return Ok(true);
            }

            return Problem();
        }
    }
}