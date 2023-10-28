namespace SoftwareFest.Areas.Client.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Models;
    using SoftwareFest.Models.Enums;
    using SoftwareFest.Pagination.Enums;
    using SoftwareFest.Services.Contracts;

    [Authorize]
    public class ProductController : BaseClientController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("offers")]
        public async Task<IActionResult> All(
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 50)
        {
            var result = await _productService.GetPagedProducts(pageIndex, pageSize, x => x.Quantity > 0 || x.Quantity == null);

            return View(result);
        }

        [HttpPost("offers")]
        public async Task<IActionResult> All(
            string name, ProductType type, SortDirection direction,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 50)
        {
            Expression<Func<Product, bool>> predicate = p => p.Name.ToLower().Contains(string.IsNullOrEmpty(name) ? p.Name.ToLower() : name.ToLower()) && p.Type == (type == ProductType.All ? p.Type : type);

            var result = await _productService.GetPagedProducts(pageIndex, pageSize, predicate, p => p.Price, direction);

            ViewBag.Name = name;
            ViewBag.Type = type;
            ViewBag.Direction = direction;

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var product = await _productService.GetById(id);

            return View(product);
        }
    }
}
