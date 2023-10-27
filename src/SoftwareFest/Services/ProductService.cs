namespace SoftwareFest.Services
{
    using System.Linq;
    using System.Linq.Expressions;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SoftwareFest.Pagination;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;

    using SofwareFest.Infrastructure;

    using Product = SoftwareFest.Models.Product;

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

        public async Task AddProduct(ProductViewModel model, string userId)
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

        public async Task<bool> IsOwner(string userId, int productId)
        {
            //TODO: Do it again

            var temp = await _context.Users
                .Include(u => u.Business)
                .ThenInclude(b => b.Products)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return temp.Business.Products
                        .Select(p => p.Id)
                        .Contains(productId);
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductViewModel> GetById(int productId, string userId)
        {
            var product = await GetById(productId);
            product.IsMine = await IsOwner(userId, productId);

            return product;
        }

        public async Task<IPage<ShowProductViewModel>> GetPagedProducts(int pageIndex = 1, int pageSize = 50, Expression<Func<Models.Product, bool>>? predicate = null, Expression<Func<Models.Product, object>>? orderBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            predicate ??= p => true;
            orderBy ??= x => x.Id;

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
                var products = await _context.Products
                    .Where(predicate)
                    .OrderBy(orderBy)
                    .ToListAsync();

                 result = products.Select(x => _mapper.Map<ShowProductViewModel>(x)).ToList();
            }
            else
            {
                var products = await _context.Products
                    .Where(predicate)
                    .OrderByDescending(orderBy)
                    .ToListAsync();

                result = products.Select(x => _mapper.Map<ShowProductViewModel>(x)).ToList();
            }

            _logger.LogDebug($"SQLServer -> Got page number: {pageIndex}");
            return new Page<ShowProductViewModel>(result, pageIndex, pageSize, totalCount);
        }

        public async Task Update(ProductViewModel model)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            product.Description = model.Description;
            product.Price = model.Price;
            product.Name = model.Name;
            product.ImageUrl = model.ImageUrl;

            _logger.LogInformation($"Updated product with id {model.Id}");

            await _context.SaveChangesAsync();
        }

        public async Task<ProductViewModel> GetById(int productId)
        {
            var product = await _context.Products
                .Include(x => x.Business)
                .Where(x => x.Id == productId)
                .Select(x => _mapper.Map<ProductViewModel>(x))
                .FirstOrDefaultAsync();

            _logger.LogInformation($"Retrieved details for product with id {productId}");

            return product;
        }
    }
}
