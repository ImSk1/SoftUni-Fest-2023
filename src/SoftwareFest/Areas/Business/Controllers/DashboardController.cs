using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using SoftwareFest.Services.Contracts;
using System.Security.Claims;

namespace SoftwareFest.Areas.Business.Controllers
{
    public class DashboardController : BaseBusinessController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var page = await _dashboardService.LoadFor(userId);

            return View(page);
        }
    }
}
