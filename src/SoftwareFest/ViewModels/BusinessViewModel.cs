using AutoMapper;
using SoftwareFest.Infrastructure.Mapping;
using SoftwareFest.Models;
using System.ComponentModel.DataAnnotations;

namespace SoftwareFest.ViewModels
{
    public class BusinessViewModel : UserViewModel, IMapFrom<Business>
    {
        public int? Id { get; set; }
        public string BusinessName { get; set; } = default!;
        public string? StripeUserId { get; set; }
        public string? UserId { get; set; }

        [Required]
        public string EthereumWalletAddress { get; set; } = default!;
        public void Mapping(Profile mapping)
        {
            mapping.CreateMap<Business, BusinessViewModel>().ReverseMap();
        }
    }
}
