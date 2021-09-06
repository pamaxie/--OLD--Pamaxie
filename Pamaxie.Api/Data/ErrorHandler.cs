namespace Pamaxie.Api.Data
{
    /// <summary>
    /// Contains all error handling information
    /// TODO: Please get rid of this shit. Either use the proper error codes or just like, use the proper file for it (resx) so it can be modified later.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Information that a item have been deleted
        /// </summary>
        /// <returns>Text for deleted events</returns>
        public static string Deleted() => "Item has been deleted";

        /// <summary>
        /// Information that a item have been created
        /// </summary>
        /// <returns>Text for created events</returns>
        public static string Created() => "Item has been created";

        /// <summary>
        /// Information that a item is not authorized
        /// </summary>
        /// <returns>Text for unauthorized events</returns>
        public static string UnAuthorized() => "Application-Id or Application-Hash is incorrect or unknown. Please try other credentials.";

        /// <summary>
        /// Information that a item already exist
        /// </summary>
        /// <returns>Text for when a item already exist events</returns>
        public static string AlreadyExists() => "User Already exists";

        /// <summary>
        /// Information that a item does not contain the correct data
        /// </summary>
        /// <returns>Text for bad data events</returns>
        public static string BadData() => "The transmitted data was in a bad format or failed to parse correctly";

        /// <summary>
        /// Information that there is problems with the server
        /// </summary>
        /// <returns>Text for server error events</returns>
        public static string ServerError() => "Internal Server Error";

        /// <summary>
        /// Information that a token is not valid
        /// </summary>
        /// <returns>Text for invalid token events</returns>
        public static string InvalidToken() => "Invalid/ Unknown Auth Token";

        /// <summary>
        /// Information that a user or guild could not be found
        /// </summary>
        /// <returns>Text for user not found events</returns>
        public static string UserNotFound() => "User or Guild could not be found";

        /// <summary>
        /// Information that a item is not in a valid format
        /// </summary>
        /// <returns>Text for invalid format events</returns>
        internal static string InvalidFormat() => "Image format of sent url is not supported. Please check the documentation for supported file types for image scans.";
    }
}