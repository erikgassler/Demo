using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Shared;

namespace WebApp.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(
			ISqlRunner sqlRunner
			)
		{
			SqlRunner = sqlRunner;
		}

		[HttpGet]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			var rng = new Random();
			WeatherForecastSummary[] summaries = await SqlRunner.RunSqlQuery<WeatherForecastSummary[]>("SELECT Id, Summary FROM [dbo].[WeatherForecastSummary] (NOLOCK)");
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = summaries[rng.Next(summaries.Length)].Summary
			})
			.ToArray();
		}

		private ISqlRunner SqlRunner { get; }
	}
}
