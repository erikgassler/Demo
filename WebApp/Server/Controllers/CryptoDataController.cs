using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApp.Server.Controllers
{
	public class CryptoDataController : ControllerCore
	{
		public CryptoDataController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			CSVLoader = GetService<ICSVLoader>();
		}

		[HttpGet("TriggerCrypto")]
		public async Task TriggerCryptoProcess()
		{
		}

		private ICSVLoader CSVLoader { get; }
	}
}
