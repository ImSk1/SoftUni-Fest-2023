namespace SoftwareFest.Areas.Client.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Services.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class RetailerController : BaseClientController
    {
        private readonly IRetailerService _retailerService;

        public RetailerController(IRetailerService retailerService)
        {
            _retailerService = retailerService;
        }

        [HttpGet("retailers")]
        public async Task<IActionResult> Retailers(
            string name = "",
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 3)
        {
            var result = await _retailerService.GetPagedProducts(pageIndex, pageSize, name);

            ViewBag.Name = name;

            return View(result);
        }

        [HttpGet("retailer/{retailerId}")]
        public async Task<IActionResult> Retailer(int retailerId,
            string name = null,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageIndex = 1,
            [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
            int pageSize = 6)
        {
            var result = await _retailerService.GetPagedProductsByRetailerId(retailerId, pageIndex, pageSize, name);

            ViewBag.Name = name;

            return View(result);
        }
    }
}
