using AutoMapper;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using SoftwareFest.Pagination;
using SoftwareFest.Pagination.Enums;
using SoftwareFest.Services.Contracts;
using SoftwareFest.ViewModels;
using SofwareFest.Infrastructure;
using System;

namespace SoftwareFest.Services
{
    public class RetailerService : IRetailerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RetailerService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Page<RetailerViewModel>> GetPagedProducts(int pageIndex, int pageSize)
        {
            pageIndex -= 1;

            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var totalCount = await _context.Businesses.CountAsync();
            var result = _context.Businesses.Include(a => a.Products).Select(_mapper.Map<RetailerViewModel>);

            return new Page<RetailerViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }

        public async Task<Page<RetailerViewModel>> GetPagedProducts(int pageIndex, int pageSize, string name)
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var totalCount = await _context.Businesses.CountAsync();
            var result = _context.Businesses.Include(a => a.Products).Select(_mapper.Map<RetailerViewModel>);

            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(a => a.BusinessName.ToLower().Contains(name.ToLower()));
            }

            return new Page<RetailerViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }

        public async Task<Page<ShowProductViewModel>> GetPagedProductsByRetailerId(int retailerId, int pageIndex, int pageSize, string name)
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var business = await _context.Businesses.Include(a => a.Products).FirstOrDefaultAsync(a => a.Id == retailerId);

            var totalCount = business.Products.Count();

            var result = business.Products.Select(_mapper.Map<ShowProductViewModel>);

            return new Page<ShowProductViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }
    }
}
