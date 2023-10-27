using Microsoft.AspNetCore.Mvc;

namespace SoftwareFest.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
