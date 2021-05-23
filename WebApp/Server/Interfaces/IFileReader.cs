using System.Collections.Generic;

namespace WebApp.Server
{
	public interface IFileReader
	{
		IEnumerable<string> ReadFileLines(string filePath);
	}
}
