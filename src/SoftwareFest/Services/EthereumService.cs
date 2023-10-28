//using System.Runtime.CompilerServices;
//using Nethereum.Model;
//using Nethereum.Web3;

//namespace SoftwareFest.Services
//{
//    public class EthereumService
//    {
//        private readonly Web3 _web3;

//        public EthereumService(string rpcUrl, string privateKey)
//        {
//            _web3 = new Web3($"https://sepolia.infura.io/v3/d5c3de3866034e919c471d0980068868");
//        }

//        public async Task<string> GetBalance()
//        {
//            var balance = await _web3.Eth.GetBalance.SendRequestAsync("0x8cCd8161353809144b2f5CA8F3F0e2BB1a2bc336");
//            var etherAmount = Web3.Convert.FromWei(balance.Value);
//        }
//    }
//}
