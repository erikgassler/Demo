using Moq;
using System;

namespace Demo.UnitTests
{
	public abstract class TestFramework
	{
		protected T Mock<T>(Action<Mock<T>> setupHandler = null) where T: class
		{
			Mock<T> instance = new Mock<T>();
			setupHandler?.Invoke(instance);
			return instance.Object;
		}

		protected T IsAny<T>()
		{
			return It.IsAny<T>();
		}
	}
}
