using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SoftwareFest.Infrastructure.Exceptions;

namespace SoftwareFest.Areas.Home.Controllers
{
    [AllowAnonymous]
    [Area("Home")]
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            Exception error = exceptionHandlerPathFeature?.Error!;
            switch (error)
            {
                case NotFoundException:
                    return RedirectToAction("ErrorPage", "Error", new { area = "Home", statusCode = 404 });
                case ArgumentException:
                    return RedirectToAction("ErrorPage", "Error", new { area = "Home", statusCode = 500 });

                default: return RedirectToAction("ErrorPage", "Error", new { area = "Home", statusCode = 500 });
            }
        }
        public IActionResult ErrorPage(int statusCode)
        {
            ViewData["statusCode"] = statusCode;
            return View();
        }
    }
}
