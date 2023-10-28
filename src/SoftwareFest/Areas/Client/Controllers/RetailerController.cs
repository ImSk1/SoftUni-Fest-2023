using Microsoft.AspNetCore.Mvc;
using SoftwareFest.Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SoftwareFest.Areas.Client.Controllers
{
    public class RetailerController : BaseClientController
    {
        private readonly IRetailerService _retailerService;

        public RetailerController(IRetailerService retailerService)
        {
            _retailerService = retailerService;
        }

        [HttpGet("retailers")]
        public async Task<IActionResult> Retailers(
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 50)
        {
            var result = await _retailerService.GetPagedProducts(pageIndex, pageSize);

            return View(result);
        }

        [HttpPost("retailers")]
        public async Task<IActionResult> Retailers(
            string name,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 50)
        {
            var result = await _retailerService.GetPagedProducts(pageIndex, pageSize, name);

            return View(result);
        }

        [HttpGet("retailer/{retailerId}")]
        public async Task<IActionResult> Retailer(int retailerId,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 50)
        {
            var result = await _retailerService.GetPagedProductsByRetailerId(retailerId, pageIndex, pageSize, null);

            return View(result);
        }

        [HttpPost("retailer/{retailerId}")]
        public async Task<IActionResult> Retailer(int retailerId,
            string name,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 50)
        {
            var result = await _retailerService.GetPagedProductsByRetailerId(retailerId, pageIndex, pageSize, name);

            return View(result);
        }
    }
}
