using System;
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

            CreateMap<BlogPostDto, BlogPostShortViewModel>()
                .ForMember(p => p.Tags, 
                    d => d.MapFrom(p => p.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)));
            CreateMap<BlogPostShortViewModel, BlogPostDto>();

            CreateMap<BlogPostViewModel, BlogPostDto>();
            CreateMap<BlogPostDto, BlogPostViewModel>()
                .ForMember(p => p.Tags,
                    d => d.MapFrom(p => p.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries))); ;

            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
        }
    }
}