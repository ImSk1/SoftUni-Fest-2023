using Microsoft.AspNetCore.Mvc;
using SoftwareFest.Areas.Client.Controllers;
using SoftwareFest.Services.Contracts;
using SoftwareFest.ViewModels;
using System.Security.Claims;

namespace SoftwareFest.Areas.Business.Controllers
{
    public class ManageProductController : BaseBusinessController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ManageProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
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

            return RedirectToAction("~/");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!await _productService.IsOwner(userId, id))
            {
                return Forbid();
            }

            var product = await _productService.GetById(id);

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

            return RedirectToAction("~/", new { id = model.Id });
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
