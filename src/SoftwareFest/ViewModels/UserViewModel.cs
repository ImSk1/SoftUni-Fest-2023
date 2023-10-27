namespace SoftwareFest.ViewModels
{
    using AutoMapper;
    using SoftwareFest.Infrastructure.Mapping;
    using SoftwareFest.Models;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        public void Mapping(Profile mapping)
        {
            mapping.CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).ReverseMap();
        }
    }
}
