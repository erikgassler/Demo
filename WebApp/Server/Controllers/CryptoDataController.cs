using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.Server.Controllers
{
	public class CryptoDataController : ControllerBase
	{
		public CryptoDataController(ICSVLoader csvLoader)
		{
			CSVLoader = csvLoader;
		}

		[HttpGet("TriggerCrypto")]
		public async Task TriggerProcess()
		{
		}

		private ICSVLoader CSVLoader { get; }
	}
}
