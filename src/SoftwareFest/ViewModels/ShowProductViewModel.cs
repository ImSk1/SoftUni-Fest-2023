namespace SoftwareFest.ViewModels
{

    using AutoMapper;

    using SoftwareFest.Infrastructure.Mapping;
    using SoftwareFest.Models;

    public class ShowProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public long Price { get; set; }

        public void Mapping(Profile mapping)
        {
            mapping.CreateMap<Product, ShowProductViewModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (decimal)src.Price / 100));

            mapping.CreateMap<ShowProductViewModel, Product>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (long)src.Price * 100));
        }
    }
}
