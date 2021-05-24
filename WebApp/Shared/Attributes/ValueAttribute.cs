using System;

namespace WebApp.Shared
{
	/// <summary>
	/// Attribute intended for Enum objects.
	/// </summary>
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
