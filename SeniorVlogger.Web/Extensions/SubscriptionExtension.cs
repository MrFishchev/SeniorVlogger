using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.ViewModels;

namespace SeniorVlogger.Web.Extensions
{
    public static class SubscriptionExtension
    {
        public static SubscriptionDto ToDto(this SubscriptionViewModel vm)
        {
            return new SubscriptionDto
            {
                Id = vm.Id,
                Email = vm.Email.ToLower(),
                UnsubscribeDate = vm.UnsubscribeDate,
                SubscribeDate = vm.SubscribeDate,
                IsSubscribed = vm.IsSubscribed
            };
        }

        public static SubscriptionViewModel ToViewModel(this SubscriptionDto objDb)
        {
            return new SubscriptionViewModel
            {
                Id = objDb.Id,
                Email = objDb.Email,
                UnsubscribeDate = objDb.UnsubscribeDate,
                SubscribeDate = objDb.SubscribeDate,
                IsSubscribed = objDb.IsSubscribed
            };
        }
    }
}
