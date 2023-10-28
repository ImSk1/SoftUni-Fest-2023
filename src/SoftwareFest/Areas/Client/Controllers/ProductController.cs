namespace SoftwareFest.Areas.Client.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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

        

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var product = await _productService.GetById(id);

            return View(product);
        }

        
    }
}
