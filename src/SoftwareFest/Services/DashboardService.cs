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

            return transactions.Select(_mapper.Map<DashboardViewModel>).ToList();
        }
    }
}
