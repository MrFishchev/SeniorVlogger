using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SeniorVlogger.Common.Email.IEmail
{
    public interface IEmailService : IEmailSender
    {
        Task SendWelcomeAsync(string email);

        Task SendWelcomeBackAsync(string email);
    }
}
