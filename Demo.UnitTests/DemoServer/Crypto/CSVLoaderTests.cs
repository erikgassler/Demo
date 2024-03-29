﻿using Moq;
using System.Collections.Generic;
using WebApp.Server;
using WebApp.Shared;
using Xunit;

namespace Demo.UnitTests.DemoServer.Crypto
{
	public class CSVLoaderTests : TestFramework
	{
		[Fact]
		public void VerityCSVLoading()
		{
			ICSVLoader loader = new CSVLoader(
				Mock<IFileReader>(mock =>
				{
					mock.Setup(m => m.ReadFileLines(It.IsAny<string>()))
						.Returns(MockCSVData);
				})
				);
			IEnumerable<CryptoData> dataList = loader.ParseFileAndReturnData();
			int returnCount = 0;
			IEnumerator<CryptoData> listReader = dataList.GetEnumerator();
			while (listReader.MoveNext())
			{
				CryptoData dataItem = listReader.Current;
				Assert.True(dataItem.Unix > 0);
				Assert.True(dataItem.Date != default);
				Assert.NotEmpty(dataItem.Symbol);
				Assert.True(dataItem.Open > 0);
				Assert.True(dataItem.High > 0);
				Assert.True(dataItem.Low > 0);
				Assert.True(dataItem.Close > 0);
				Assert.True(dataItem.VolumeBTC > 0);
				Assert.True(dataItem.VolumeUSDT > 0);
				++returnCount;
			}
			Assert.Equal(12, returnCount);
		}

		private static IEnumerable<string> MockCSVData()
		{
			yield return "https://www.CryptoDataDownload.com";
			yield return @"unix,date,symbol,open,high,low,close,Volume BTC,Volume USDT";
			yield return @"1616309880000.0,2021-03-21 06:58:00,BTC-PERP,57120.0,57129.0,57120.0,57128.0,4.956303455398404,283143.7038";
			yield return @"1616309820000.0,2021-03-21 06:57:00,BTC-PERP,57177.0,57178.0,57120.0,57120.0,45.57778823354342,2603403.2639";
			yield return @"1616309760000.0,2021-03-21 06:56:00,BTC-PERP,57201.0,57201.0,57173.0,57177.0,5.723527736677335,327254.1454";
			yield return @"1616309700000.0,2021-03-21 06:55:00,BTC-PERP,57178.0,57260.0,57174.0,57201.0,17.717386447789373,1013452.2222";
			yield return @"1616309640000.0,2021-03-21 06:54:00,BTC-PERP,57254.0,57255.0,57174.0,57178.0,14.772805587813496,844679.4779";
			yield return @"1616309580000.0,2021-03-21 06:53:00,BTC-PERP,57224.0,57282.0,57186.0,57254.0,24.624791485311068,1409867.8117";
			yield return @"1616309520000.0,2021-03-21 06:52:00,BTC-PERP,57298.0,57298.0,57218.0,57224.0,28.65841651579757,1639949.2267";
			yield return @"1616309460000.0,2021-03-21 06:51:00,BTC-PERP,57340.0,57341.0,57293.0,57298.0,5.0935781388530135,291851.8402";
			yield return @"1616309400000.0,2021-03-21 06:50:00,BTC-PERP,57311.0,57341.0,57291.0,57340.0,9.825752675270317,563408.6584";
			yield return @"1616309340000.0,2021-03-21 06:49:00,BTC-PERP,57363.0,57363.0,57307.0,57311.0,10.89516028511106,624412.5311";
			yield return @"1616309280000.0,2021-03-21 06:48:00,BTC-PERP,57307.0,57386.0,57307.0,57363.0,19.528989948224464,1120241.4504";
			yield return @"1616309220000.0,2021-03-21 06:47:00,BTC-PERP,57333.0,57333.0,57300.0,57307.0,7.586815392534943,434777.6297";
		}
	}
}
