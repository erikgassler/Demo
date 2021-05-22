using System.Collections.Generic;
using WebApp.Server;
using Xunit;

namespace Demo.UnitTests.DemoServer.Abstract
{
	public class JsonConvertTests
	{
		[Theory, MemberData(nameof(TestInput))]
		public void VerifyJsonConversions(TestObject dataInput)
		{
			IJsonConvert converter = new JsonConvert();
			string json = converter.Serialize(dataInput);
			TestObject converted = converter.Deserialize<TestObject>(json);
			Assert.Equal(converted, dataInput);
		}

		public static IEnumerable<object[]> TestInput()
		{
			yield return new object[]
			{
				new TestObject()
				{
					Test = 49
				}
			};
		}

		public class TestObject
		{
			public int Test { get; set; }

			public override bool Equals(object obj)
			{
				return GetHashCode() == obj.GetHashCode();
			}

			public override string ToString()
			{
				return Test.ToString();
			}

			public override int GetHashCode()
			{
				return Test.GetHashCode();
			}
		}
	}
}
