namespace SoftwareFest.Services
{
    using System.Linq.Expressions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using SoftwareFest.Models;
    using SoftwareFest.Pagination;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;

    using SofwareFest.Infrastructure;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddProduct(AddProductViewModel model, string userId)
        {
            var product = _mapper.Map<Product>(model);

            var businessId = await _context.Users
                .Include(u => u.Business)
                .Where(x => x.Id == userId)
                .Select(x => x.Business.Id)
                .FirstOrDefaultAsync();

            product.BusinessId = businessId;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Succesfully added product with id {product.Id}");
        }

        public async Task<DetailsProductViewModel> GetById(int id)
        {
            var product = await _context.Products
                .Where(x => x.Id == id)
                .Select(x => _mapper.Map<DetailsProductViewModel>(x))
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<List<ShowProductViewModel>> GetProducts()
        {
            var products = await _context.Products
                .Select(x => _mapper.Map<ShowProductViewModel>(x))
                .ToListAsync();

            _logger.LogInformation($"Retrieved all products from the database.");

            return products;
        }

        public async Task<IPage<ShowProductViewModel>> GetPagedProducts(int pageIndex = 1, int pageSize = 50, Expression<Func<Product, bool>>? predicate = null, Expression<Func<ShowProductViewModel, object>>? orderBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var totalCount = await _context.Products
                .Where(predicate)
                .CountAsync();

            var result = new List<ShowProductViewModel>();

            if (sortDirection == SortDirection.Ascending)
            {
                result = await _context.Products
                    .Where(predicate)
                    .Select(x => _mapper.Map<ShowProductViewModel>(x))
                    .OrderBy(orderBy)
                    .ToListAsync();
            }
            else
            {
                result = await _context.Products
                    .Where(predicate)
                    .Select(x => _mapper.Map<ShowProductViewModel>(x))
                    .OrderByDescending(orderBy)
                    .ToListAsync();
            }
            
            _logger.LogDebug($"SQLServer -> Got page number: {pageIndex}");
            return new Page<ShowProductViewModel>(result, pageIndex, pageSize, totalCount);
        }
    }
}
