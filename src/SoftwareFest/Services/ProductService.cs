namespace SoftwareFest.Services
{
    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SoftwareFest.Models;
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


    }
}
