using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SeniorVlogger.Common.Email;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.Models.Requests;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailSettings _emailSettings;
        private readonly IEmailService _emailService;

        public EmailController(IOptions<EmailSettings> options, IEmailService emailService)
        {
            _emailSettings = options.Value;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] FeedbackRequest request)
        {
            if (request == null) return BadRequest();

            await _emailService.SendFeedbackAsync(_emailSettings.FeedbackEmail, 
                request.Subject, request.Message);

            return Ok();
        }
    }
}
