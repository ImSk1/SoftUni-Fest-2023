using AutoMapper;
using SoftwareFest.Models;
using SoftwareFest.ViewModels;

namespace SoftwareFest.Infrastructure.Mapping
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).ReverseMap();
        }
    }
}
