namespace SoftwareFest.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;

    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddProductViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state from product creation");
                return View(model);
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await _productService.AddProduct(model, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
