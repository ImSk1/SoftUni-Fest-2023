namespace SoftwareFest.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        [InverseProperty(nameof(Models.Business.User))]
        public Business Business { get; set; } = null!;

        [InverseProperty(nameof(Models.Client.User))]
        public Client Client { get; set; } = null!;

    }
}
