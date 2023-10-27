using SoftwareFest.Services.Contracts;
using SoftwareFest.ViewModels;
using Stripe.Checkout;

namespace SoftwareFest.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> CheckOut(DetailsProductViewModel product)
        {
            //var baseUrl = _httpContextAccessor.HttpContext.Request.Host.ToString();
            var baseUrl = "https://localhost:7215";

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
                            UnitAmount = (long)(product.Price * 100), // Price is in USD cents.
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
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id; ;
        }
    }
}
