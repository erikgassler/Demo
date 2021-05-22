using System;

namespace WebApp.Shared
{
	public interface IDevLog
	{
		void TrackException(Exception exception);
		void TrackEvent(string eventName);
		void TrackProperty(string propertyName, object value);
	}
}
