using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.Common.Helpers;
using SeniorVlogger.Common.Properties;

namespace SeniorVlogger.Common.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(subject, htmlMessage, email);
        }

        public Task SendNewPostAvailableAsync(string email, string url, string title, string description, string imageUrl)
        {
            var htmlMessage = Resources.NewPostAvailableTemplate;
            htmlMessage = htmlMessage.Replace("%PostTitle%", title);
            htmlMessage = htmlMessage.Replace("%PostDescription%", description);
            htmlMessage = htmlMessage.Replace("%PostUrl%", url);
            htmlMessage = htmlMessage.Replace("%PostImageUrl%", imageUrl);
            return Execute($"Read new: {title}", htmlMessage, email);
        }

        public Task SendWelcomeAsync(string email)
        {
            return Execute("Thank you for subscribing!",
                Resources.WelcomeEmailTemplate, email);
        }

        public Task SendWelcomeBackAsync(string email)
        {
            return Execute("Welcome back friend!", 
                Resources.WelcomeBackEmailTemplate, email);
        }

        public Task SendFeedbackAsync(string email, string subject, string message)
        {
            var htmlMessage = Resources.FeedbackRequestEmailTemplate;
            htmlMessage = htmlMessage.Replace("%Subject%", subject);
            htmlMessage = htmlMessage.Replace("%CallbackEmail%", email);
            htmlMessage = htmlMessage.Replace("%Message%", message);
            return Execute(subject, htmlMessage, email);
        }

        private Task Execute(string subject, string htmlMessage, string email)
        {
            htmlMessage = htmlMessage.Replace("%UserId%", Base64Helper.Base64Encode(email));

            var client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", _emailSettings.EmailApiKey)
            };

            var request = new RestRequest();
            request.AddParameter("domain", _emailSettings.EmailDomain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", $"Senior Vlogger <{_emailSettings.EmailFrom}>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("html", htmlMessage);
            request.Method = Method.POST;
            return client.ExecuteAsync(request);
        }
    }
}
