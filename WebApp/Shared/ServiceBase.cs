using System;
using System.Diagnostics;

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

		public T TryProcess<T>(
			string processName,
			Func<T> process,
			Action<Stopwatch> finallyHandler = null
			)
		{
			if(process == null) { return default; }
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				return process.Invoke();
			}
			catch(Exception exception)
			{
				Log.TrackException(exception);
				return default;
			}
			finally
			{
				Log.TrackProperty($"EventTiming:{processName}", timer);
				Log.TrackEvent($"{processName}:Finished");
				finallyHandler?.Invoke(timer);
			}
		}

		protected IDevLog Log => GetService<IDevLog>();
		protected T GetService<T>() => (T)ServiceProvider.GetService(typeof(T));
		protected IServiceProvider ServiceProvider { get; }
	}
}
