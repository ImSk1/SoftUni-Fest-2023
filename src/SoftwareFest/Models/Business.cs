namespace SoftwareFest.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Business
    {
        public Business()
        {
            Products = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string BusinessName { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
        
        public string? StripeUserId { get; set; } = null!;

        [Required]
        public string EthereumWalletAddress { get; set; } = default!;

        [InverseProperty(nameof(Product.Business))]
        public List<Product> Products { get; set; } = null!;
    }
}
