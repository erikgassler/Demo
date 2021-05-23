using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApp.Shared;

namespace WebApp.Server
{
	public class CryptoIngestion : ServiceBase, ICryptoIngestion
	{
		public CryptoIngestion(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			CSVLoader = GetService<ICSVLoader>();
		}

		public Task RunCryptoIngestion()
		{
			if (IngestionIsRunning) { return Task.CompletedTask; }
			lock (IngestionLock)
			{
				IngestionIsRunning = true;
				Task.WaitAny(TruncateIngestionTableForIngestionDemoRestart());
				Task.WaitAny(RunIngestion());
				IngestionIsRunning = false;
			}
			return Task.CompletedTask;
		}

		private async Task TruncateIngestionTableForIngestionDemoRestart()
		{
			ISqlRunner sqlRunner = GetService<ISqlRunner>();
			await sqlRunner.RunSqlQuery($"TRUNCATE TABLE {SqlTables.CryptoData.GetValue()};");
		}

		private async Task RunIngestion()
		{
			ISqlRunner sqlRunner = GetService<ISqlRunner>();
			IEnumerable<CryptoData> dataList = CSVLoader.ParseFileAndReturnData();
			foreach (CryptoData cryptoData in dataList)
			{
				await sqlRunner.RunSqlQuery($@"
	INSERT INTO {SqlTables.CryptoData.GetValue()} ([Unix], [Date], [Symbol], [Open], [High], [Low], [Close], [VolumeBTC], [VolumeUSDT])
	SELECT [Unix], [Date], [Symbol], [Open], [High], [Low], [Close], [VolumeBTC], [VolumeUSDT]
	FROM @cryptodata
", new SqlParameter[] { MapCryptoDataToSqlParameter(cryptoData) });
			}
		}

		private SqlParameter MapCryptoDataToSqlParameter(CryptoData cryptoData)
		{
			DataTable table = new();
			foreach (string column in new[] { "Id", "Unix", "Date", "Symbol", "Open", "High", "Low", "Close", "VolumeBTC", "VolumeUSDT" })
			{
				table.Columns.Add(column);
			}
			DataRow row = table.NewRow();
			row["Id"] = cryptoData.Id;
			row["Unix"] = cryptoData.Unix;
			row["Date"] = cryptoData.Date;
			row["Symbol"] = cryptoData.Symbol;
			row["Open"] = cryptoData.Open;
			row["High"] = cryptoData.High;
			row["Low"] = cryptoData.Low;
			row["Close"] = cryptoData.Close;
			row["VolumeBTC"] = cryptoData.VolumeBTC;
			row["VolumeUSDT"] = cryptoData.VolumeUSDT;
			table.Rows.Add(row);
			return new SqlParameter("@cryptodata", table)
			{
				SqlDbType = SqlDbType.Structured,
				TypeName = "[dbo].[CryptoDataInput]"
			};
		}

		private ICSVLoader CSVLoader { get; }
		private static bool IngestionIsRunning { get; set; }
		private static object IngestionLock { get; } = new object();
	}
}
