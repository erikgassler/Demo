using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApp.Shared;
using static WebApp.Shared.Functional;

namespace WebApp.Server.Controllers
{
	public class ControllerCore : ControllerBase
	{
		public ControllerCore(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		public T TryLoggedProcess<T>(
			Func<T> process,
			Action<Stopwatch> finallyHandler = null,
			[CallerFilePath] string filePath = null,
			[CallerMemberName] string method = null,
			[CallerLineNumber] int lineNumber = 0
			)
		{
			return TryProcess(process,
				rethrowOnException: false,
				exceptionHandler: (exception, timer) =>
				{
					Log.TrackProperty("Exception:FilePath", filePath);
					Log.TrackProperty("Exception:Method", method);
					Log.TrackProperty("Exception:LineNumber", lineNumber);
					Log.TrackException(exception);
					Response.Headers.Add(CustomHeaders.ErrorMessage.GetValue(), exception.Message);
					Response.StatusCode = 500;
				},
				finallyHandler: timer =>
				{
					Log.TrackProperty($"ControllerTiming:{method}", timer);
					Log.TrackEvent($"Controller:{method}:Finished");
					finallyHandler?.Invoke(timer);
				});
		}

		protected IDevLog Log => GetService<IDevLog>();
		protected T GetService<T>() => (T)ServiceProvider.GetService(typeof(T));
		protected IServiceProvider ServiceProvider { get; }
	}
}
