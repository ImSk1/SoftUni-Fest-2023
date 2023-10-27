namespace SoftwareFest.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApplicationUser : IdentityUser
    {
        [InverseProperty(nameof(Models.Business.User))]
        public Business Business { get; set; } = null!;

        [InverseProperty(nameof(Models.Client.User))]
        public Client Client { get; set; } = null!;
    }
}
