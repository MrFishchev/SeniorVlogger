namespace SeniorVlogger.Models.Requests
{
    public class FeedbackRequest
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public string CallbackEmail { get; set; }
    }
}
