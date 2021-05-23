using System;
using System.Reflection;

namespace WebApp.Shared
{
	public enum CustomHeaders
	{
		[Value("X-Error-Message")]
		ErrorMessage
	}

	public static class Constants
	{
		public static string GetValue(this CustomHeaders instance)
		{
			MemberInfo[] info = instance.GetType().GetMember(instance.ToString());
			if (info == null || info.Length == 0) { return instance.ToString(); }
			if (Attribute.GetCustomAttribute(info[0], typeof(ValueAttribute)) is not ValueAttribute valueTag) { return instance.ToString(); }
			return valueTag.Value;
		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class ValueAttribute : Attribute
	{
		public ValueAttribute(string value)
		{
			Value = value;
		}

		internal string Value { get; }
	}
}
