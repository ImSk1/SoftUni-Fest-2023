namespace SoftwareFest.Services
{
    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SoftwareFest.Models.Enums;
    using SoftwareFest.Pagination;
    using SoftwareFest.Pagination.Contracts;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;

    using SofwareFest.Infrastructure;

    using System.Linq;
    using System.Linq.Expressions;

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

            var businessId = await _context.Businesses
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            product.BusinessId = businessId;

            if (product.Type == ProductType.Service)
            {
                product.Quantity = null;
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Succesfully added product with id {product.Id}");
        }

        public async Task<bool> IsOwner(string userId, int productId)
        {
            var userBusiness = await _context.Businesses
                .FirstOrDefaultAsync(a => a.UserId == userId);

            var product = await _context.Products
                .FirstOrDefaultAsync(a => a.BusinessId == userBusiness!.Id);

            return product != null;
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

        public async Task<IPage<ShowProductViewModel>> GetPagedProducts(string name, int pageIndex = 1, int pageSize = 50, Expression<Func<Models.Product, object>>? orderBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            orderBy ??= x => x.Id;

            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var totalCount = await _context.Products
                .Where(x => x.Name.ToLower().Contains(string.IsNullOrEmpty(name) ? x.Name : name))
                .Where(x => x.Quantity != 0)
                .CountAsync();

            var result = new List<ShowProductViewModel>();

            if (sortDirection == SortDirection.Ascending)
            {
                var products = await _context.Products
                    .Include(x => x.Business)
                    .Where(x => x.Name.ToLower().Contains(string.IsNullOrEmpty(name) ? x.Name : name))
                    .Where(x => x.Quantity != 0)
                    .OrderBy(orderBy)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                result = products.Select(x => _mapper.Map<ShowProductViewModel>(x)).ToList();
            }
            else
            {
                var products = await _context.Products
                    .Include(x => x.Business)
                    .Where(x => x.Name.ToLower().Contains(string.IsNullOrEmpty(name) ? x.Name : name))
                    .Where(x => x.Quantity != 0)
                    .OrderByDescending(orderBy)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                result = products.Select(x => _mapper.Map<ShowProductViewModel>(x)).ToList();
            }

            _logger.LogDebug($"SQLServer -> Got page number: {pageIndex}");
            return new Page<ShowProductViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }

        public async Task<IPage<ShowProductViewModel>> GetPagedProductsByUserId(string userId, int pageIndex = 1, int pageSize = 50)
        {
            pageIndex -= 1;
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            var business = await _context.Businesses
                .Include(a => a.Products)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            var totalCount = business.Products.Count();

            var result = business.Products
                .OrderBy(p => p.Price)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(_mapper.Map<ShowProductViewModel>);

            _logger.LogDebug($"SQLServer -> Got page number: {pageIndex}");
            return new Page<ShowProductViewModel>(result, pageIndex + 1, pageSize, totalCount);
        }

        public async Task Update(ProductViewModel model)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            product!.Description = model.Description;
            product.Price = (long)(model.Price * 100);
            product.EthPrice = model.EthPrice;
            product.Name = model.Name;
            if (product.Type == ProductType.Service)
            {
                product.Quantity = null;
            }


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

            return product!;
        }

        public async Task<bool> HasEnoughQuantity(int productId)
        {
            var quantity = await _context.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Quantity)
                .FirstOrDefaultAsync();

            if (quantity == 0)
            {
                return false;
            }

            return true;
        }
    }
}
