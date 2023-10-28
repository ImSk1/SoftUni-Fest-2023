namespace SoftwareFest.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using SoftwareFest.Models;
    using SoftwareFest.Models.Enums;
    using SoftwareFest.Pagination;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;
    using SofwareFest.Infrastructure;
    using System;

    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ApplicationDbContext context, IMapper mapper, ILogger<TransactionService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Create(int productId, string userId, string stripeTransactionId = "")
        {
            var clientId = await _context.Clients
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var transaction = new Transaction
            {
                ProductId = productId,
                Date = DateTime.UtcNow,
                ClientId = clientId,
                StripeTransactionId = stripeTransactionId
            };

            _logger.LogInformation($"Transaction created for user with id {userId}, product with id {productId} and stripe transaction id {stripeTransactionId}");

            await _context.Transactions.AddAsync(transaction);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product.Type == ProductType.Physical)
            {
                product.Quantity--;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IPage<TransactionViewModel>> GetPagedTransactions(string userId, int pageIndex = 1, int pageSize = 50)
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }


            var totalCount = await _context.Transactions
                .Include(c => c.Client)
                .Include(b => b.Product)
                .ThenInclude(b => b.Business)
                .Where(c => c.Client.UserId == userId)
                .CountAsync();

            var result = new List<TransactionViewModel>();

            var transactions = await _context.Transactions
                .Include(c => c.Client)
                .Include(b => b.Product)
                .ThenInclude(b => b.Business)
                .Where(c => c.Client.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            result = transactions.Select(x => _mapper.Map<TransactionViewModel>(x)).ToList();

            _logger.LogDebug($"SQLServer -> Got page number: {pageIndex}");
            return new Page<TransactionViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }
    }
}
