using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.Common.Helpers;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.Requests;
using SeniorVlogger.Models.ViewModels;
using SeniorVlogger.Web.Extensions;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : Controller
    {
        #region Fields

        private readonly ILogger<SubscriptionController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        #endregion

        #region Constructor

        public SubscriptionController(IUnitOfWork unitOfWork, IEmailService emailService, 
            ILogger<SubscriptionController> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        #endregion

        #region Actions

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<SubscriptionViewModel>> GetAll()
        {
            var subscriptions = await _unitOfWork.Subscriptions.GetAll();
            return subscriptions?.Select(s => s.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SubscriptionRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email)) return BadRequest();

            try
            {
                var subscription = await _unitOfWork.Subscriptions.GetFirstOrDefault(s=> 
                    s.Email.ToLower() == request.Email.ToLower());

                if (subscription == null)
                {
                    subscription = new SubscriptionDto
                    {
                        Email = request.Email,
                        IsSubscribed = true,
                        SubscribeDate = DateTime.UtcNow
                    };
                    await _unitOfWork.Subscriptions.Add(subscription);
                    _emailService.SendWelcomeAsync(subscription.Email);
                }
                else
                {
                    if (subscription.IsSubscribed)
                    {
                        return Json(new {exist = true, message = "You are already subscribed, but thank you!"});
                    }
                    subscription.IsSubscribed = true;
                    await _unitOfWork.Subscriptions.Update(subscription);
                    await _unitOfWork.Save();
                    _emailService.SendWelcomeBackAsync(subscription.Email).ConfigureAwait(false);
                    return Json(new {exist = true, message = "Welcome back, Friend! Thank you!"});
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cannot subscribe");
                throw;
            }

            await _unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("{base64Email}")]

        public async Task<IActionResult> Unsubscribe(string base64Email)
        {
            var decodedId = Base64Helper.Base64Decode(base64Email);
            if (string.IsNullOrWhiteSpace(decodedId)) return Problem();

            var subscription = await _unitOfWork.Subscriptions.GetFirstOrDefault(s => s.Email == decodedId);
            subscription.IsSubscribed = false;
            subscription.UnsubscribeDate = DateTime.UtcNow;

            await _unitOfWork.Save();
            return Ok();
        }

        #endregion
    }
}
