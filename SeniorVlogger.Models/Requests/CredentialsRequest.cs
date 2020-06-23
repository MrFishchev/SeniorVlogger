namespace SeniorVlogger.Models.Requests
{
    public class CredentialsRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
