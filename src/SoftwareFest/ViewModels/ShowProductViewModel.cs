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

        public double Price { get; set; }

        public double EthPrice { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ShowProductViewModel>()
                .ForMember(dest => dest.Price, cfg => cfg.MapFrom(src => (double)((double)src.Price / 100)));
        }
    }
}
