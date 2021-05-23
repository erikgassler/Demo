using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApp.Shared;

namespace WebApp.Server
{
	public class CryptoStorage : ServiceBase, ICryptoStorage
	{
		public CryptoStorage(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}

		public async Task<IEnumerable<CryptoData>> RunCryptoIngestionReport(long lastId)
		{
			List<CryptoData> dataList = await TryProcess("RunCryptoIngestionReport", async () =>
			{
				ISqlRunner sqlRunner = GetService<ISqlRunner>();
				return await sqlRunner.RunSqlQuery<List<CryptoData>>($@"
	SELECT
		[Id]
		, [Unix]
		, [Date]
		, [Symbol]
		, [Open]
		, [High]
		, [Low]
		, [Close]
		, [VolumeBTC]
		, [VolumeUSDT]
	FROM [dbo].[CryptoData] (NOLOCK)
	WHERE [Id] > @lastid
	ORDER BY [Id] ASC
",
					new SqlParameter[] {
						new SqlParameter("@lastid", lastId)
					});
			});
			return dataList;
		}
	}
}
