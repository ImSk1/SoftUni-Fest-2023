namespace SoftwareFest.Areas.Client.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;
    using Stripe.Checkout;

    [Authorize]
    public class CheckoutController : BaseClientController
    {
        private readonly IConfiguration _config;
        private readonly ICheckoutService _checkoutService;
        private readonly IProductService _productService;

        public CheckoutController(IConfiguration configuration, ICheckoutService checkoutService, IProductService productService)
        {
            _config = configuration;
            _checkoutService = checkoutService;
            _productService = productService;
        }

        [Route("checkout/{productId}")]
        public async Task<IActionResult> Checkout([FromRoute] int productId)
        {
            var product = await _productService.GetById(productId);

            var sessionId = await _checkoutService.CheckOut(product);
            var publicKey = _config["Stripe:PublicKey"];

            var checkoutOrderResponse = new CheckoutOrderResponse()
            {
                SessionId = sessionId,
                PublicKey = publicKey
            };
            return View(checkoutOrderResponse);
        }

        [HttpGet("checkout/success")]
        public IActionResult CheckoutSuccess([FromQuery] string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;


            return Ok($"{total} {customerEmail}");
        }
        [HttpGet("failed")]

        public IActionResult CheckoutFailed()
        {
            return Ok();
        }

    }
}
