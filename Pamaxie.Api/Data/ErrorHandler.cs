namespace Pamaxie.Api.Data
{
    public static class ErrorHandler
    {
        public static string Deleted()
        {
            return "Item has been deleted";
        }

        public static string Created()
        {
            return "Item has been created";
        }

        public static string UnAuthorized()
        {
            return "Application-Id or Application-Hash is incorrect or unknown. Please try other credentials.";
        }

        public static string AlreadyExists()
        {
            return "User Already exists";
        }

        public static string BadData()
        {
            return "The transmitted data was in a bad format or failed to parse correctly";
        }

        public static string ServerError()
        {
            return "Internal Server Error";
        }

        public static string InvalidToken()
        {
            return "Invalid/ Unknown Auth Token";
        }

        public static string UserNotFound()
        {
            return "User or Guild could not be found";
        }

        internal static string InvalidFormat()
        {
            return "Image format of sent url is not supported. Please check the documentation for supported file types for image scans.";
        }
    }
}