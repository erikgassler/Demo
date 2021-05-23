using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApp.Shared;

namespace WebApp.Server.Controllers
{
	public class ControllerCore : ControllerBase
	{
		public ControllerCore(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		public T TryProcess<T>(
			Func<T> process,
			Action<Stopwatch> finallyHandler = null,
			[CallerFilePath] string filePath = null,
			[CallerMemberName] string method = null,
			[CallerLineNumber] int lineNumber = 0
			)
		{
			if (process == null) { return default; }
			Stopwatch timer = Stopwatch.StartNew();
			try
			{
				return process.Invoke();
			}
			catch (Exception exception)
			{
				Log.TrackProperty("Exception:FilePath", filePath);
				Log.TrackProperty("Exception:Method", method);
				Log.TrackProperty("Exception:LineNumber", lineNumber);
				Log.TrackException(exception);
				Response.Headers.Add(CustomHeaders.ErrorMessage.GetValue(), exception.Message);
				Response.StatusCode = 500;
				return default;
			}
			finally
			{
				Log.TrackProperty($"ControllerTiming:{method}", timer);
				Log.TrackEvent($"Controller:{method}:Finished");
				finallyHandler?.Invoke(timer);
			}
		}

		protected IDevLog Log => GetService<IDevLog>();
		protected T GetService<T>() => (T)ServiceProvider.GetService(typeof(T));
		protected IServiceProvider ServiceProvider { get; }
	}
}
