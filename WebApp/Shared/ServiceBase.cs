using System;
using System.Diagnostics;
using static WebApp.Shared.Functional;

namespace WebApp.Shared
{
	/// <summary>
	/// Base class for services.
	/// Includes common functionality needed by general classes.
	/// </summary>
	public abstract class ServiceBase
	{
		public ServiceBase(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		public T TryLoggedProcess<T>(
			string processName,
			Func<T> process,
			Action<Stopwatch> finallyHandler = null
			)
		{
			return TryProcess(process,
				rethrowOnException: false,
				exceptionHandler: (exception, timer) =>
				{
					Log.TrackException(exception);
				},
				finallyHandler: timer =>
				{
					Log.TrackProperty($"EventTiming:{processName}", timer);
					Log.TrackEvent($"{processName}:Finished");
					finallyHandler?.Invoke(timer);
				});
		}

		protected IDevLog Log => GetService<IDevLog>();
		protected T GetService<T>() => (T)ServiceProvider.GetService(typeof(T));
		protected IServiceProvider ServiceProvider { get; }
	}
}
