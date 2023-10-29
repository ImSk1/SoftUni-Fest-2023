using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoftwareFest.Models;
using SoftwareFest.Services.Contracts;
using SoftwareFest.ViewModels;
using SofwareFest.Infrastructure;

namespace SoftwareFest.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DashboardService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DashboardViewModel>> LoadFor(string userId)
        {
            var business = await _context.Businesses.FirstOrDefaultAsync(a => a.UserId == userId);

            var transactions = await _context.Transactions
                .Include(a => a.Product)
                .Include(a => a.Client)
                .Where(a => a.Product.BusinessId == business.Id)
                .ToListAsync();

            var model = transactions.Select(_mapper.Map<DashboardViewModel>).ToList();

            var payments = transactions.Select(t => new Payment
            {
                IsUsdPayment = !string.IsNullOrEmpty(t.StripeTransactionId),
                Date = t.Date,
                PriceUSD = (decimal)((decimal)t.Product.Price / 100),
                PriceETH = t.Product.EthPrice
            }).ToList();

            var walletAmounts = CalculateWalletAmounts(payments);

            for (int i = 0; i < model.Count; i++)
            {
                model[i].Payment = payments[i];
                model[i].WalletAmount = walletAmounts[i];
            }

            return model;
        }

        private List<WalletAmount> CalculateWalletAmounts(List<Payment> payments)
        {
            var walletAmounts = new List<WalletAmount>();
            decimal? usdRunningTotal = 0;
            decimal? ethRunningTotal = 0;

            foreach (var payment in payments)
            {
                if (payment.IsUsdPayment)
                    usdRunningTotal += payment.PriceUSD;
                
                else
                    ethRunningTotal += payment.PriceETH;

                walletAmounts.Add(new WalletAmount { Date = payment.Date, AmountUSD = usdRunningTotal, AmountETH = ethRunningTotal });
            }

            return walletAmounts;
        }
    }
}
