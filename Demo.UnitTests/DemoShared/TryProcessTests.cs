using System;
using System.Threading.Tasks;
using Xunit;
using static WebApp.Shared.Functional;

namespace Demo.UnitTests.DemoShared
{
	public class TryProcessTests : TestFramework
	{
		[Fact]
		public void VerifyTryProcess()
		{
			int result = TryProcess(() =>
			{
				return 100;
			});
			Assert.Equal(100, result);
		}

		[Fact]
		public void VerifyTryProcessThrowsException()
		{
			bool throwError = true;
			int result = TryProcess(() =>
			{
				if (throwError)
				{
					throw new Exception("Test");
				}
				return 100;
			}, rethrowOnException: false);
			Assert.Equal(0, result);
		}

		[Fact]
		public async Task VerifyAsyncTryProcess()
		{
			int result = await TryProcess(async () =>
			{
				return await Task.Run(() => 100);
			});
			Assert.Equal(100, result);
		}

		[Fact]
		public async Task VerifyAsyncTryProcessThrowsError()
		{
			bool throwError = true;
			int result = await TryProcess(async () =>
			{
				if (throwError)
				{
					throw new Exception("Test");
				}
				return await Task.Run(() => 100);
			}, rethrowOnException: false);
			Assert.Equal(0, result);
		}
	}
}
