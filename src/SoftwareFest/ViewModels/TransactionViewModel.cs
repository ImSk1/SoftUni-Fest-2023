namespace SoftwareFest.ViewModels
{
    using AutoMapper;
    using SoftwareFest.Infrastructure.Mapping;
    using SoftwareFest.Models;

    public class TransactionViewModel : IMapFrom<Transaction>
    {
        public string BusinessName { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public DateTime Date { get; set; }

        public decimal? AmountUSD { get; set; }

        public decimal? AmountETH { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionViewModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.Product.Business.BusinessName))
                .ForMember(dest => dest.AmountUSD, opt => opt.MapFrom(src => (decimal)((decimal)src.Product.Price / 100)))
                .ForMember(dest => dest.AmountETH, opt => opt.MapFrom(src => src.Product.EthPrice));
        }
    }
}
