using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApp.Shared
{
	public static partial class Functional
	{
		public static T TryProcess<T>(
			Func<T> process,
			bool rethrowOnException = true,
			Action<Exception, Stopwatch> exceptionHandler = null,
			Action<Stopwatch> finallyHandler = null
			)
		{
			if (process == null) { return default; }
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				return process.Invoke();
			}
			catch(Exception exception)
			{
				exceptionHandler?.Invoke(exception, timer);
				if (rethrowOnException)
				{
					throw;
				}
				return default;
			}
			finally
			{
				finallyHandler?.Invoke(timer);
			}
		}

		public static async Task<T> TryProcess<T>(
			Func<Task<T>> process,
			bool rethrowOnException = true,
			Action<Exception, Stopwatch> exceptionHandler = null,
			Action<Stopwatch> finallyHandler = null
			)
		{
			if (process == null) { return default; }
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				return await process.Invoke();
			}
			catch (Exception exception)
			{
				exceptionHandler?.Invoke(exception, timer);
				if (rethrowOnException)
				{
					throw;
				}
				return default;
			}
			finally
			{
				finallyHandler?.Invoke(timer);
			}
		}
	}
}
