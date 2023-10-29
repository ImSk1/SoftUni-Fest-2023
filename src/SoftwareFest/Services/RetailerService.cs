namespace SoftwareFest.Services
{
    using AutoMapper;
    using MailKit.Search;
    using Microsoft.EntityFrameworkCore;
    using SoftwareFest.Pagination;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;
    using SofwareFest.Infrastructure;
    using System;

    public class RetailerService : IRetailerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RetailerService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Page<RetailerViewModel>> GetPagedProducts(int pageIndex, int pageSize, string name = "")
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var totalCount = await _context.Businesses
                .Where(a => a.BusinessName.ToLower().Contains(string.IsNullOrEmpty(name) ? a.BusinessName.ToLower() : name.ToLower()))
                .CountAsync();

            var result = _context.Businesses
                .OrderBy(a => a.BusinessName)
                .Include(a => a.Products)
                .Select(_mapper.Map<RetailerViewModel>)
                .Where(a => a.BusinessName.ToLower().Contains(string.IsNullOrEmpty(name) ? a.BusinessName.ToLower() : name.ToLower()))
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            return new Page<RetailerViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }

        public async Task<Page<ShowProductViewModel>> GetPagedProductsByRetailerId(int retailerId, int pageIndex, int pageSize, string name)
        {
            pageIndex -= 1;

            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var business = await _context.Businesses
                .Include(a => a.Products)
                .FirstOrDefaultAsync(a => a.Id == retailerId);

            var totalCount = business.Products
                .Where(a => a.Quantity != 0)
                .Where(a => a.Name.ToLower().Contains(string.IsNullOrEmpty(name) ? a.Name.ToLower() : name.ToLower()))
                .Count();

            var result = business.Products
                .OrderBy(a => a.Price)
                .Where(a => a.Quantity != 0)
                .Where(a => a.Name.ToLower().Contains(string.IsNullOrEmpty(name) ? a.Name.ToLower() : name.ToLower()))
                .Select(_mapper.Map<ShowProductViewModel>)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            return new Page<ShowProductViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }
    }
}
