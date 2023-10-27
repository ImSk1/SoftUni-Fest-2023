namespace SoftwareFest.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SoftwareFest.Models.Enums;

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
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }

        public Business Business { get; set; } = null!;

        [Required]
        public ProductType Type { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}
