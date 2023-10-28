namespace SoftwareFest.Services
{
    using MailKit.Search;
    using System;

    using Microsoft.EntityFrameworkCore;

    using SoftwareFest.Models;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;

    using SofwareFest.Infrastructure;
    using AutoMapper;
    using SoftwareFest.Pagination;

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

        public async Task Create(int productId, string userId, string stripeTransactionId)
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
            await _context.SaveChangesAsync();
        }

        public async Task<IPage<TransactionViewModel>> GetPagedProducts(int pageIndex = 1, int pageSize = 50)
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }


            var totalCount = await _context.Transactions
                .CountAsync();

            var result = new List<TransactionViewModel>();

            var transactions = await _context.Transactions
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            result = transactions.Select(x => _mapper.Map<TransactionViewModel>(x)).ToList();

            _logger.LogDebug($"SQLServer -> Got page number: {pageIndex}");
            return new Page<TransactionViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }
    }
}
