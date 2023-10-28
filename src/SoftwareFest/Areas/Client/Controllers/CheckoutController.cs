namespace SoftwareFest.Areas.Client.Controllers
{
    using System.Security.Claims;

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
        private readonly ITransactionService _transactionService;

        public CheckoutController(IConfiguration configuration, ICheckoutService checkoutService, IProductService productService, ITransactionService transactionService)
        {
            _config = configuration;
            _checkoutService = checkoutService;
            _productService = productService;
            _transactionService = transactionService;
        }

        [Route("checkout/{productId}")]
        public async Task<IActionResult> Checkout([FromRoute] int productId)
        {
            var product = await _productService.GetById(productId);

            if (!await _productService.HasEnoughQuantity(productId))
            {
                return Forbid();
            }

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
        public async Task<IActionResult> CheckoutSuccess([FromQuery] string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId, new SessionGetOptions
            {
                Expand = new List<string>
                {
                    "line_items"
                }

            });

            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;
            var productId = int.Parse(session.Metadata["offer_id"]);
            var additionalInfo = session.Metadata["business_id"];
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            await _transactionService.Create(productId, userId, sessionId);

            return RedirectToAction("Transactions", "Transaction", new {Area = "Client"});
        }
        [HttpGet("failed")]

        public IActionResult CheckoutFailed()
        {
            return Ok();
        }

    }
}
