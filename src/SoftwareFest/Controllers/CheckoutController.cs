namespace SoftwareFest.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;
    using Stripe.Checkout;

    public class CheckoutController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ICheckoutService _checkoutService;
        public CheckoutController(IConfiguration configuration, ICheckoutService checkoutService)
        {
            _config = configuration;
            _checkoutService = checkoutService;
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            ProductViewModel exampleProduct = new ProductViewModel
            {
                Id = 1,
                Name = "Laptop",
                Description = "High-performance laptop with 16GB RAM and 1TB SSD",
                Price = 99999,
                ImageUrl = "https://example.com/images/laptop.jpg",
                BusinessId = 1
            };
            var sessionId = await _checkoutService.CheckOut(exampleProduct);
            var publicKey = _config["Stripe:PublicKey"];

            var checkoutOrderResponse = new CheckoutOrderResponse()
            {
                SessionId = sessionId,
                PublicKey = publicKey
            };
            return View(checkoutOrderResponse);
        }

        [HttpGet("success")]
        public IActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;

            return Ok($"{total} {customerEmail}");
        }
    }
}
