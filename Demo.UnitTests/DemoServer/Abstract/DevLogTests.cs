using System;
using WebApp.Server;
using WebApp.Shared;
using Xunit;

namespace Demo.UnitTests.DemoServer.Abstract
{
	public class DevLogTests
	{
		[Fact]
		public void VerifyTrackException()
		{
			IDevLog devLog = new DevLog();
			devLog.TrackException(new Exception("Test"));
		}
		
		[Fact]
		public void VerifyTrackProperty()
		{
			IDevLog devLog = new DevLog();
			devLog.TrackProperty("TestProperty", 97);
		}

		[Fact]
		public void VerifyTrackEvent()
		{
			IDevLog devLog = new DevLog();
			devLog.TrackEvent("TestEvent");
		}
	}
}
