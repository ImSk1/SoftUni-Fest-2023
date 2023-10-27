using AutoMapper;
using SoftwareFest.Infrastructure.Mapping;
using SoftwareFest.Models;

namespace SoftwareFest.ViewModels
{
    public class BusinessViewModel : UserViewModel, IMapFrom<Business>
    {
        public string BusinessName { get; set; } = default!;
        public void Mapping(Profile mapping)
        {
            mapping.CreateMap<Business, BusinessViewModel>().ReverseMap();
        }
    }
}
