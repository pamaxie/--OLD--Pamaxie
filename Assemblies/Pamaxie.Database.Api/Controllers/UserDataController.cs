using CryptSharp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Pamaxie.Database.Redis.DataClasses;

namespace Pamaxie.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly TokenGenerator _generator;

        public UserDataController(TokenGenerator generator)
        {
            _generator = generator;
        }

        /// <summary>
        /// Creates a new user with the sent API token
        /// </summary>
        /// <returns><see cref="MediaData"/> Token for Authentication</returns>
        [HttpPost("createUser")]
        public ActionResult<MediaPredictionData> SetMediaDataTask(string hash)
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());
            LoginData userData = JsonConvert.DeserializeObject<LoginData>(result);
            string userId = userData.UserName;
            string password = userData.Password;


            if (!System.IO.File.Exists(UserAuthDataLocation.AuthDataLocation))
                return NotFound(ErrorHandler.ServerError());

            MediaPredictionData data = new MediaPredictionData(hash);
            if (!data.TryLoadData(out _))
                return NotFound();

            return data;
        }

    }
}