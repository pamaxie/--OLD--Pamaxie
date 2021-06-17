using CryptSharp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;

namespace Pamaxie.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenGenerator _generator;

        public AuthController(TokenGenerator generator)
        {
            _generator = generator;
        }

        /// <summary>
        /// Signs in a user via Basic authentication and returns a token.
        /// </summary>
        /// <returns><see cref="AuthToken"/> Token for Authentication</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthToken> LoginTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());
            LoginData userData = JsonConvert.DeserializeObject<LoginData>(result);
            string userId = userData.UserName;
            string password = userData.Password;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
                return NotFound(ErrorHandler.UnAuthorized());

            if (!System.IO.File.Exists(UserAuthDataLocation.AuthDataLocation))
                return NotFound(ErrorHandler.ServerError());

            //Managing User Credentials and returning a new oAuth Token if the credentials provided
            //are admin credentials
            string data = System.IO.File.ReadAllText(UserAuthDataLocation.AuthDataLocation);
            List<LoginData> loginDataList = JsonConvert.DeserializeObject<List<LoginData>>(data);
            AuthToken token = null;
            foreach (LoginData loginData in loginDataList)
            {
                if (loginData.UserName != userId) continue;
                if (!Crypter.CheckPassword(password, loginData.Password)) continue;
                token = _generator.CreateToken(userId);
            }

            if (token == null)
                return Unauthorized(ErrorHandler.UnAuthorized());

            return Ok(token);
        }

        /// <summary>
        /// Creates a new Api User
        /// </summary>
        /// <returns><see cref="string"/> Sucess?</returns>
        [Authorize]
        [HttpPost("createUser")]
        public ActionResult<string> CreateUserTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());


            LoginData userData = JsonConvert.DeserializeObject<LoginData>(result);
            if (string.IsNullOrEmpty(userData.UserName) || string.IsNullOrEmpty(userData.Password))
                return NotFound(ErrorHandler.UnAuthorized());
            userData.Password = Crypter.Blowfish.Crypt(userData.Password);

            if (System.IO.File.Exists(UserAuthDataLocation.AuthDataLocation))
            {
                //Read all previous users out and write them back into the file.
                string existingUserText = System.IO.File.ReadAllText(UserAuthDataLocation.AuthDataLocation);
                List<LoginData> prevData = JsonConvert.DeserializeObject<List<LoginData>>(existingUserText);
                prevData.Add(userData);
                string existingUserData = JsonConvert.SerializeObject(prevData);
                System.IO.File.WriteAllText(UserAuthDataLocation.AuthDataLocation, existingUserData);
            }
            else
            {
                //Write new file and create it.
                List<LoginData> userList = new List<LoginData>
                {
                    userData
                };
                string newUserData = JsonConvert.SerializeObject(userList, Formatting.Indented);
                if (!Directory.Exists(UserAuthDataLocation.AuthDataFolder))
                    Directory.CreateDirectory(UserAuthDataLocation.AuthDataFolder);
                FileStream file = System.IO.File.Create(UserAuthDataLocation.AuthDataLocation);
                file.Close();
                System.IO.File.WriteAllText(UserAuthDataLocation.AuthDataLocation, newUserData);
            }

            return Ok("Success");
        }


        /// <summary>
        /// Refreshes an exiting oAuth Token
        /// </summary>
        /// <returns><see cref="AuthToken"/> Refreshed Token</returns>
        [Authorize]
        [HttpPost("refresh")]
        public ActionResult<AuthToken> RefreshTask()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            
            if (string.IsNullOrEmpty(result)) 
                return BadRequest(ErrorHandler.BadData());
            
            LoginData userData = JsonConvert.DeserializeObject<LoginData>(result);

            if (!string.IsNullOrEmpty(userData.Password))
                return NotFound(ErrorHandler.BadBadDeveloper());

            string userId = userData.UserName;
            AuthToken token = _generator.CreateToken(userId);
            return Ok(token);
        }
    }
}