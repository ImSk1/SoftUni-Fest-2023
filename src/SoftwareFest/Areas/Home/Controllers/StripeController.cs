namespace SoftwareFest.Areas.Home.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Models;
    using SoftwareFest.Services.Contracts;
    using Stripe;
    using System.Security.Claims;

    [Authorize]
    [Route("[controller]")]
    public class StripeController : BaseHomeController
    {
        private readonly IConfiguration _config;
        private readonly IBusinessService _businessService;

        public StripeController(IConfiguration configuration, IBusinessService businessService)
        {
            _config = configuration;
            _businessService = businessService;
        }


        [HttpGet("connect")]
        public async Task<IActionResult> ConnectStripe()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentBusiness = await _businessService.GetBusinessByUserId(userId);
            if (currentBusiness.StripeUserId != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "Home" });
            }
            var cliendId = _config["Stripe:ClientId"];
            var redirectUrl = "https://localhost:7215/stripe/callback";
            return Redirect(
                $"https://connect.stripe.com/oauth/authorize?response_type=code&client_id={cliendId}&scope=read_write&redirect_uri={redirectUrl}");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> StripeCallback(string code)
        {
            var options = new OAuthTokenCreateOptions
            {
                Code = code,
                GrantType = "authorization_code"
            };

            var service = new OAuthTokenService();
            var response = await service.CreateAsync(options);

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var business = await _businessService.GetBusinessByUserId(userId);
            business.StripeUserId = response.StripeUserId;
            await _businessService.UpdateBusiness(business);


            return Redirect("~/");
        }
    }
}
