namespace SoftwareFest.Areas.Home.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using SoftwareFest.Models;


    [AllowAnonymous]
    public class HomeController : BaseHomeController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                if (User.IsInRole("Business"))
                {
                    return RedirectToAction("Dashboard", "Dashboard", new { Area = "Business" });
                }
                if (User.IsInRole("Client"))
                {
                    return RedirectToAction("All", "Product", new { Area = "Client" });
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}