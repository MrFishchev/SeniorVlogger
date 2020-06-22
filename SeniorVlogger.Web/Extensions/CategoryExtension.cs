using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.ViewModels;

namespace SeniorVlogger.Web.Extensions
{
    public static class CategoryExtension
    {
        public static CategoryViewModel ToViewModel(this CategoryDto objDb)
        {
            return new CategoryViewModel
            {
                Id = objDb.Id,
                Name = objDb.Name
            };
        }

        public static CategoryDto ToDto(this CategoryViewModel vm)
        {
            return new CategoryDto
            {
                Id = vm.Id,
                Name = vm.Name
            };
        }
    }
}
