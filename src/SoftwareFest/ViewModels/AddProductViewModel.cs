namespace SoftwareFest.ViewModels
{

    using System.ComponentModel.DataAnnotations;

    using SoftwareFest.Infrastructure.Mapping;
    using SoftwareFest.Models;
    using SoftwareFest.Models.Enums;

    public class AddProductViewModel : IMapFrom<Product>
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(20)]
        [MaxLength(250)]
        public string Description { get; set; } = null!;

        [Required]
        public long Price { get; set; }

        [Required]
        public ProductType Type { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}
