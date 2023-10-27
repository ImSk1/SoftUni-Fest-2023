using System.ComponentModel.DataAnnotations;

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
        public long Price { get; set; }

        [Required]
        public ProductType Type { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public int? BusinessId { get; set; }
    }
}
