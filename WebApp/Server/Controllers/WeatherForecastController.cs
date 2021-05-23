using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Shared;

namespace WebApp.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerCore
	{
		public WeatherForecastController(
			IServiceProvider serviceProvider
			) : base(serviceProvider)
		{
		}

		[HttpGet]
		public Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
		{
			return TryProcess(async () =>
			{
				ISqlRunner sqlRunner = GetService<ISqlRunner>();
				var rng = new Random();
				WeatherForecastSummary[] summaries = await sqlRunner.RunSqlQuery<WeatherForecastSummary[]>("SELECT Id, Summary FROM [dbo].[WeatherForecastSummary] (NOLOCK)");
				return Enumerable.Range(1, 5).Select(index => new WeatherForecast
				{
					Date = DateTime.Now.AddDays(index),
					TemperatureC = rng.Next(-20, 55),
					Summary = summaries[rng.Next(summaries.Length)].Summary
				});
			});
		}
	}
}
