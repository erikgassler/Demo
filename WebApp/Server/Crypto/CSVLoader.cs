using System;
using System.Collections.Generic;
using WebApp.Shared;

namespace WebApp.Server
{
	public class CSVLoader : ICSVLoader
	{
		public CSVLoader(IFileReader fileReader)
		{
			FileReader = fileReader;
		}

		public IEnumerable<CryptoData> ParseFileAndReturnData()
		{
			IEnumerable<string> fileLines = FileReader.ReadFileLines(StaticFile);
			foreach (string line in fileLines)
			{
				if (string.IsNullOrWhiteSpace(line)) { continue; }
				// If line is colum headers, we can skip as we're not doing anything with the headers at the moment.
				if (line.Contains("unix")) { continue; }
				// When present, row data is expected to always be populated in the same order (unix,date,symbol,open,high,low,close,Volume BTC,Volume USDT) with no null/empty values
				string[] columns = line.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
				// Real world we would want to log the contents of a line that has the incorrect values to more easily detect when data is missing and trace the cause.
				if(columns.Length != 9) { continue; }
				yield return new CryptoData()
				{
					Unix = ExtractValue<long>(columns[UnixIndex].Replace(".0", "")),
					Date = ExtractValue<DateTime>(columns[DateIndex]),
					Symbol = columns[SymbolIndex],
					Open = ExtractValue<decimal>(columns[OpenIndex]),
					High = ExtractValue<decimal>(columns[HighIndex]),
					Low = ExtractValue<decimal>(columns[LowIndex]),
					Close = ExtractValue<decimal>(columns[CloseIndex]),
					VolumeBTC = ExtractValue<decimal>(columns[VolumeBTCIndex]),
					VolumeUSDT = ExtractValue<decimal>(columns[VolumeUSDTIndex])
				};
			}
			yield break;
		}

		private T ExtractValue<T>(string input)
		{
			return (T)Convert.ChangeType(input, typeof(T));
		}

		private const string StaticFile = "Uploads/Data.csv";
		private const int UnixIndex = 0;
		private const int DateIndex = 1;
		private const int SymbolIndex = 2;
		private const int OpenIndex = 3;
		private const int HighIndex = 4;
		private const int LowIndex = 5;
		private const int CloseIndex = 6;
		private const int VolumeBTCIndex = 7;
		private const int VolumeUSDTIndex = 8;

		private IFileReader FileReader { get; }
	}
}
