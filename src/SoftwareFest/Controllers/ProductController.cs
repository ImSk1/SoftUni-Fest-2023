namespace SoftwareFest.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    [Authorize]
    public class ProductController : Controller
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
            var result = await _productService.GetPagedProducts(pageIndex, pageSize);
            
            return View(result);
        }

        [HttpGet("create")]
        public IActionResult Add()
        {
            var model = new ProductViewModel();
            return View(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state from product creation");
                return View(model);
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await _productService.AddProduct(model, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var product = await _productService.GetById(id, userId);
            
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!await _productService.IsOwner(userId, id))
            {
                return Forbid();
            }

            var product = await _productService.GetById(id, userId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Update failed - bad modelstate");
                return View(model);
            }
            if (model.Id == null)
            {
                _logger.LogError("Update failed - bad modelstate");
                return BadRequest();
            }

            await _productService.Update(model);

            return RedirectToAction(nameof(Details), new {id = model.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!await _productService.IsOwner(userId, id))
            {
                return Forbid();
            }

            await _productService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
