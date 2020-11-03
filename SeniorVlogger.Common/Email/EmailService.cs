using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.Common.Helpers;
using SeniorVlogger.Common.Properties;

namespace SeniorVlogger.Common.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
        {
            _emailSettings = options.Value;
            _logger = logger;
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

        private async Task Execute(string subject, string htmlMessage, string email)
        {
            try
            {
                htmlMessage = htmlMessage.Replace("%UserId%", Base64Helper.Base64Encode(email));

                var bodyBuilder = new BodyBuilder {HtmlBody = htmlMessage};
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(MailboxAddress.Parse(_emailSettings.EmailFrom));
                mailMessage.To.Add(MailboxAddress.Parse(email));
                mailMessage.Subject = subject;
                mailMessage.Body = bodyBuilder.ToMessageBody();

                await SendMessage(mailMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot send email to {email}");
            }
        }

        private async Task SendMessage(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer,
                _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
