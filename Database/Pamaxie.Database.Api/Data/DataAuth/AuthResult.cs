namespace Pamaxie.Api.Data
{
    /// <summary>
    /// Did user authenticate sucessfully with our API
    /// </summary>
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}