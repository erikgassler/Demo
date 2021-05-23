using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Shared;

namespace WebApp.Server.Controllers
{
	public class CryptoDataController : ControllerCore
	{
		public CryptoDataController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}

		[HttpGet("TriggerCryptoIngestion")]
		public Task TriggerCryptoProcess()
		{
			return TryProcess(() =>
			{
				ICryptoIngestion service = GetService<ICryptoIngestion>();
				return service.RunCryptoIngestion();
			});
		}

		[HttpGet("LoadCryptoRecords/{lastId?}")]
		public Task<IEnumerable<CryptoData>> LoadCryptoRecords(long lastId = 0)
		{
			return TryProcess(() =>
			{
				ICryptoStorage service = GetService<ICryptoStorage>();
				return service.RunCryptoIngestionReport(lastId);
			});
		}

	}
}
