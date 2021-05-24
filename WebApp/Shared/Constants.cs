namespace WebApp.Shared
{
	/// <summary>
	/// Custom response headers and their name defined in Value attributes.
	/// </summary>
	public enum CustomHeaders
	{
		[Value("X-Error-Message")]
		ErrorMessage
	}

	/// <summary>
	/// Sql tables used by solution with their `[schema].[tablename]` defined in Value attributes.
	/// </summary>
	public enum SqlTables
	{
		[Value("[dbo].[WeatherForecastSummary]")]
		WeatherForecastSummary,
		[Value("[dbo].[CryptoData]")]
		CryptoData
	}

	/// <summary>
	/// Container for constant values that require being contained in a class.
	/// </summary>
	public static class Constants
	{
	}
}
