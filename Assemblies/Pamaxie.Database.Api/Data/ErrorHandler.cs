using System;

namespace Pamaxie.Api.Data
{
    public class ErrorHandler
    {
        /// <summary>
        /// Should be called when an Item has been Deleted
        /// </summary>
        /// <returns></returns>
        public static string Deleted()
        {
            return "Item has been deleted";
        }

        /// <summary>
        /// Should be called when an Item has been Created
        /// </summary>
        /// <returns></returns>
        public static string Created()
        {
            return "Item has been created";
        }

        /// <summary>
        /// Should be called if a User is not authorized
        /// </summary>
        /// <returns></returns>
        public static string UnAuthorized()
        {
            return "Username or Password is incorrect";
        }

        /// <summary>
        /// Should be called if an entry already exists
        /// </summary>
        /// <returns></returns>
        public static string AlreadyExists()
        {
            return "User Already exists";
        }

        /// <summary>
        /// Should be called when Bad Data was provided in a request
        /// </summary>
        /// <returns></returns>
        public static string BadData()
        {
            return "The transmitted data was in a bad format or failed to parse correctly";
        }

        /// <summary>
        /// Should be called if a Server Error is encountered
        /// </summary>
        /// <returns></returns>
        public static string ServerError()
        {
            return "Internal Server Error";
        }

        /// <summary>
        /// Should be called if an Authentication Token is invalid
        /// </summary>
        /// <returns></returns>
        public static string InvalidToken()
        {
            return "Invalid/Unknown Auth Token";
        }

        /// <summary>
        /// Should be called if an Item cannot be found
        /// </summary>
        /// <returns></returns>
        public static string ItemNotFound()
        {
            return "Item could not be found";
        }

        /// <summary>
        /// Should be called if a User cannot be found
        /// </summary>
        /// <returns></returns>
        public static string UserNotFound()
        {
            return "User or Guild could not be found";
        }

        /// <summary>
        /// Tell Developers they should not use passwords in refreshing their auth token
        /// </summary>
        /// <returns></returns>
        internal static object BadBadDeveloper()
        {
            return "Never. EVER, EVER. Include your password in the refresh method. " +
                "We require you to be authenticated for it anyways and sending user passwords should only be done to gather the token";
        }
    }
}