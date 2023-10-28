using System.ComponentModel.DataAnnotations;

using AutoMapper;

using SoftwareFest.Infrastructure.Mapping;
using SoftwareFest.Models;
using SoftwareFest.Models.Enums;

namespace SoftwareFest.ViewModels
{
    public class ProductViewModel : IMapFrom<Product>
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required]
        [MinLength(20)]
        [MaxLength(250)]
        public string Description { get; set; } = default!;

        [Required]
        public double Price { get; set; }

        [Required]
        public ProductType Type { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public string? BusinessName { get; set; }

        public int? BusinessId { get; set; }

        public bool IsMine { get; set; } = false;

        public int? Quantity { get; set; }

        public void Mapping(Profile map)
        {
            map.CreateMap<Product, ProductViewModel>()
                .ForMember(desc => desc.BusinessName, opt => opt.MapFrom(src => src.Business.BusinessName))
                .ForMember(dest => dest.Price, cfg => cfg.MapFrom(src => (double)((double)src.Price / 100)));


            map.CreateMap<ProductViewModel,Product>()
                .ForMember(dest => dest.Price, cfg => cfg.MapFrom(src => (long)(src.Price * 100)));
        }
    }
}
