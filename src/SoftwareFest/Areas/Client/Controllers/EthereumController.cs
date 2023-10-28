using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SoftwareFest.ViewModels;

namespace SoftwareFest.Areas.Client.Controllers
{
    [Route("[controller]")]
    public class EthereumController : BaseClientController
    {
        private readonly HttpClient httpClient;

        public EthereumController()
        {
            httpClient = new HttpClient();
        }
        [HttpPost("handle")]
        public async Task<IActionResult> HandleTransaction([FromBody] EthTransactionViewModel data)
        {
            string txHash = data.TxHash;
            string etherscanApiKey = "NG1XRHGEXWADZBY2TJJGP2KFU7P3CEAZY1"; // Replace with your Etherscan API key

            // Monitor the transaction using Etherscan
            string url = $"https://api.etherscan.io/api?module=transaction&action=getstatus&txhash={txHash}&apikey={etherscanApiKey}";
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);

            // Check if transaction was successful
            if (jsonContent["result"]["isError"].ToString() == "0")
            {
                // Transaction was successful
                // Do something, like updating the database
            }
            else
            {
                // Transaction failed
                // Handle the failure
            }

            return Json(new { status = "handled" });
        }
    }
}
