using System;
using System.Reflection;

namespace WebApp.Shared
{
	public static class AttributeExtensions
	{
		/// <summary>
		/// Extenion method intended for enum instances to extract [Value(...)] values from a given enum instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static string GetValue<T>(this T instance)
		{
			MemberInfo[] info = instance.GetType().GetMember(instance.ToString());
			if (info == null || info.Length == 0) { return instance.ToString(); }
			if (Attribute.GetCustomAttribute(info[0], typeof(ValueAttribute)) is not ValueAttribute valueTag) { return instance.ToString(); }
			return valueTag.Value;
		}
	}
}
