using Bunit;
using Demo.Client.Shared;
using Xunit;

namespace Demo.UnitTests.DemoClient.Shared
{
	public class NavMenuTests
	{
		[Fact]
		public void Test1()
		{
			using TestContext context = new();
			IRenderedComponent<NavMenu> navMenu = context.RenderComponent<NavMenu>();

			Assert.True(navMenu.Find(".NavMenu").ClassList.Contains("collapse"));

			navMenu.Find(".navbar-toggler").Click();

			Assert.False(navMenu.Find(".NavMenu").ClassList.Contains("collapse"));
		}
	}
}
