using AutoMapper;

using SoftwareFest.Infrastructure.Mapping;
using SoftwareFest.Models;
using SoftwareFest.Models.Enums;

namespace SoftwareFest.ViewModels
{
    public class DetailsProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public long Price { get; set; }

        public string Type { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public void Mapping(Profile mapping)
        {
            mapping.CreateMap<DetailsProductViewModel, Product>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse(typeof(ProductType), src.Type)));

            mapping.CreateMap<Product, DetailsProductViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
