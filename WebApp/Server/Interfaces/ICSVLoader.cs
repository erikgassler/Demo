using System.Collections.Generic;
using WebApp.Shared;

namespace WebApp.Server
{
	public interface ICSVLoader
	{
		IEnumerable<CryptoData> ParseFileAndReturnData();
	}
}
