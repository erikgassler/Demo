using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using WebApp.Shared;

namespace Demo.UnitTests
{
	public abstract class TestFramework
	{
		protected static T Mock<T>(Action<Mock<T>> setupHandler = null) where T: class
		{
			Mock<T> instance = new();
			setupHandler?.Invoke(instance);
			return instance.Object;
		}

		protected static T IsAny<T>()
		{
			return It.IsAny<T>();
		}

		protected IServiceProvider ServiceProvider(Action<IServiceCollection> servicesConfigHandler = null)
		{
			IServiceCollection services = new ServiceCollection();
			services.AddScoped(_ => Mock<IDevLog>());
			servicesConfigHandler?.Invoke(services);
			// Returning scoped service provider
			return services.BuildServiceProvider().CreateScope().ServiceProvider;
		}
	}
}
