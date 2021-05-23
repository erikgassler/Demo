using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApp.Server;
using WebApp.Shared;
using Xunit;

namespace Demo.UnitTests.DemoServer.Crypto
{
	public class CryptoStorageTests : TestFramework
	{
		[Theory, MemberData(nameof(MockData))]
		public async Task VerifyRunningCryptoReport(List<CryptoData> mockReturnedData)
		{
			ICryptoStorage service = new CryptoStorage(ServiceProvider(services =>
			{
				services.AddTransient<ISqlRunner>(_ => Mock<ISqlRunner>(mock =>
				{
					mock.Setup(m => m.RunSqlQuery<List<CryptoData>>(IsAny<string>(), IsAny<SqlParameter[]>()))
						.Returns(Task.FromResult(mockReturnedData));
				}));
			}));
			IEnumerable<CryptoData> reportResults = await service.RunCryptoIngestionReport(0);
		}

		public static IEnumerable<object[]> MockData()
		{
			yield return new object[]
			{
				new List<CryptoData>()
				{
					new CryptoData() {}
				}
			};
		}
	}
}
