using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Server;
using WebApp.Shared;
using Xunit;

namespace Demo.UnitTests.DemoServer.Crypto
{
	public class CryptIngestionTests : TestFramework
	{
		[Theory, MemberData(nameof(MockData))]
		public async Task VerifyCryptoIngestionService(IEnumerable<CryptoData> csvLoadedData)
		{
			ICryptoIngestion service = new CryptoIngestion(ServiceProvider(services =>
			{
				services.AddTransient(_ => Mock<ICSVLoader>(mock =>
				{
					mock.Setup(m => m.ParseFileAndReturnData())
						.Returns(csvLoadedData);
				}));
				services.AddTransient(_ => Mock<ISqlRunner>(mock =>
				{
				}));
			}));
			await service.RunCryptoIngestion();
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
