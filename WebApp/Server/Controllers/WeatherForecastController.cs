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
		public WeatherForecastController(
			ISqlRunner sqlRunner
			)
		{
			SqlRunner = sqlRunner;
		}

		[HttpGet]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			try
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
			catch(Exception exception)
			{
				Response.Headers.Add(CustomHeaders.ErrorMessage.GetValue(), exception.Message);
				Response.StatusCode = 500;
				return default;
			}
		}

		private ISqlRunner SqlRunner { get; }
	}
}
