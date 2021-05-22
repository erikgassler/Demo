using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Demo.Server.Controllers
{
	public class AppSettingsController : ControllerBase
	{
		public AppSettingsController(IConfiguration configuration)
		{
			TenantId = configuration.GetValue<string>("AzureAd:TenantId");
			ClientId = configuration.GetValue<string>("AzureAd:ClientId");
		}

		[HttpGet("appsettings.json")]
		public string GetAppSettings()
		{
			return "{}";
		}

		private string TenantId { get; }
		private string ClientId { get; }
	}
}
