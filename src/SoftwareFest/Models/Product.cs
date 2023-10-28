namespace SoftwareFest.Models
{
    using SoftwareFest.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Description { get; set; } = null!;

        [Required]
        public long Price { get; set; }

        [Required]
        [Range(0, 0.0005)]
        public decimal EthPrice { get; set; }

        [Required]
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }

        public Business Business { get; set; } = null!;

        [Required]
        public ProductType Type { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int? Quantity { get; set; }
    }
}
