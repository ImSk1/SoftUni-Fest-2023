namespace SoftwareFest.ViewModels
{
    using AutoMapper;
    using SoftwareFest.Infrastructure.Mapping;
    using SoftwareFest.Models;

    public class DashboardViewModel : IMapFrom<Transaction>
    {
        public string BusinessName { get; set; } = null!;

        public string ProductName { get; set; } = null!;
        public string ClientName { get; set; } = null!;

        public Payment? Payment { get; set; }
        public WalletAmount? WalletAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, DashboardViewModel>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.FirstName + " " + src.Client.LastName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.Product.Business.BusinessName))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => new Payment
                {
                    IsUsdPayment = !string.IsNullOrEmpty(src.StripeTransactionId),
                    Date = src.Date,
                    PriceUSD = (decimal)((decimal)src.Product.Price / 100),
                    PriceETH = src.Product.EthPrice
                }))
                .ForMember(dest => dest.WalletAmount, opt => opt.MapFrom(src => new WalletAmount
                {
                    Date = src.Date,
                    AmountUSD = (decimal)((decimal)src.Product.Price / 100),
                    AmountETH = src.Product.EthPrice
                }));
        }
    }

    public class Payment
    {
        public bool IsUsdPayment { get; set; }
        public DateTime Date { get; set; }
        public decimal? PriceUSD { get; set; }
        public decimal? PriceETH { get; set; }
    }

    public class WalletAmount
    {
        public DateTime Date { get; set; }
        public decimal? AmountUSD { get; set; }
        public decimal? AmountETH { get; set; }
    }
}
