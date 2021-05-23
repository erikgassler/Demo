using System.Collections.Generic;
using System.IO;

namespace WebApp.Server
{
	public class FileReader : IFileReader
	{
		public IEnumerable<string> ReadFileLines(string filePath)
		{
			string fullPath = Path.GetFullPath(filePath);
			if (!File.Exists(fullPath))
			{
				yield break;
			}
			string[] lines = File.ReadAllLines(fullPath);
			if(lines.Length == 0)
			{
				yield break;
			}
			foreach(string line in lines)
			{
				yield return line;
			}
		}
	}
}
