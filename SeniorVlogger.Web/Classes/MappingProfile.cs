using AutoMapper;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.ViewModels;

namespace SeniorVlogger.Web.Classes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, CategoryViewModel>();
            CreateMap<CategoryViewModel, CategoryDto>();

            CreateMap<SubscriptionDto, SubscriptionViewModel>();
            CreateMap<SubscriptionViewModel, SubscriptionDto>();

            CreateMap<BlogPostDto, BlogPostShortViewModel>();
            CreateMap<BlogPostShortViewModel, BlogPostDto>();

            CreateMap<BlogPostViewModel, BlogPostDto>();
            CreateMap<BlogPostDto, BlogPostViewModel>();

            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
        }
    }
}