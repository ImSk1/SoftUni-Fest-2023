namespace SoftwareFest.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Business
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string BusinessName { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
