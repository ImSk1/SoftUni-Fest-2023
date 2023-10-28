using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SoftwareFest.ViewModels;

namespace SoftwareFest.Areas.Client.Controllers
{
    [Route("[controller]")]
    public class EthereumController : BaseClientController
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;

        public EthereumController(IConfiguration config)
        {
            httpClient = new HttpClient();
            _configuration = config;
        }
        [HttpPost("handle")]
        public async Task<IActionResult> HandleTransaction([FromBody] EthTransactionViewModel data)
        {
            string txHash = data.TxHash;
            string etherscanApiKey = _configuration["EtherScan:ApiKey"]!;

            string url = $"https://api.etherscan.io/api?module=transaction&action=getstatus&txhash={txHash}&apikey={etherscanApiKey}";
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);

            // Check if transaction was successful
            if (jsonContent["result"]["isError"].ToString() == "0")
            {
                return Json( new { url = "/ethereum/callback/true"});
            }
            else
            {
                return Json(new { url = "/ethereum/callback/false" });
            }
        }

        [HttpGet("callback/{isSuccessful}")]
        public IActionResult PaymentCallback([FromRoute] bool isSuccessful)
        {
            if (isSuccessful)
            {
                ViewData["message"] = "Crypto payment processed.";
            }
            else
            {
                ViewData["message"] = "There was an error during the payment process.";

            }
            return View();
        }
       
    }
}
