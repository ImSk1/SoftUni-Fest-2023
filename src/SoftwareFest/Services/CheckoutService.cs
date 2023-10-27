using SoftwareFest.Services.Contracts;
using SoftwareFest.ViewModels;
using Stripe;
using Stripe.Checkout;

namespace SoftwareFest.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBusinessService _buinessService;

        public CheckoutService(IHttpContextAccessor httpContextAccessor, IBusinessService buinessService)
        {
            _httpContextAccessor = httpContextAccessor;
            _buinessService = buinessService;
        }
        public async Task<string> CheckOut(DetailsProductViewModel product)
        {
            //var baseUrl = _httpContextAccessor.HttpContext.Request.Host.ToString();
            var baseUrl = "https://localhost:7215";
            var business = await _buinessService.GetBusinessById(product.BusinessId);
            var sellerStripeId = business.StripeUserId;

            long amount = (long)(product.Price * 100);  // Price in cents
            long applicationFee = (long)(amount * 0.05);

            var options = new SessionCreateOptions
            {
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{baseUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = baseUrl + "/failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = amount, // Price is in USD cents.
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                                Description = product.Description,
                                Images = new List<string> { product.ImageUrl }
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = applicationFee,
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = sellerStripeId
                    }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id; ;
        }
    }
}
