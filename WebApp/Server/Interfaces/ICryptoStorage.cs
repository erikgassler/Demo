using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Shared;

namespace WebApp.Server
{
	public interface ICryptoStorage
	{
		Task<IEnumerable<CryptoData>> RunCryptoIngestionReport(long lastId);
	}
}
