using System.Collections.Generic;
using WebApp.Server;
using Xunit;

namespace Demo.UnitTests.DemoServer.Abstract
{
	public class FileReaderTests
	{
		[Fact]
		public void VerifyFileReaderReadsCSVFile()
		{
			IFileReader fileReader = new FileReader();
			IEnumerable<string> fileContent = fileReader.ReadFileLines(RelativePathToTestFile);
			int rowCount = 0;
			foreach(string row in fileContent)
			{
				++rowCount;
			}
			Assert.Equal(NumberOfLinesInFile, rowCount);
		}

		private const string RelativePathToTestFile = "Uploads/TestData.csv";
		private const int NumberOfLinesInFile = 162603;
	}
}
