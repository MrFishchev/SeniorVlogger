using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using SeniorVlogger.Common.Email.IEmail;
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

        private Task Execute(string subject, string htmlMessage, string email)
        {
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
