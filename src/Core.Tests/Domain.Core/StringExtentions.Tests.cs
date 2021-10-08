using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{

	[TestFixture]
	public class StringExtentionsTests
	{

		[Test]
		public void TrimEnd()
		{
			Assert.AreEqual("AppService", "AppService".TrimEnd((string[])null));
			Assert.AreEqual("AppService", "AppService".TrimEnd(""));

			Assert.AreEqual("", "".TrimEnd((string[])null));
			Assert.AreEqual(null, ((string)null).TrimEnd(""));

			Assert.AreEqual("App", "AppService".TrimEnd("Service"));
			Assert.AreEqual("App", "AppServiceService".TrimEnd("Service"));
			Assert.AreEqual("ServiceApp", "ServiceApp".TrimEnd("Service"));
			Assert.AreEqual("ServiceApp", "ServiceAppService".TrimEnd("Service"));
		}

		[Test]
		public void TrimStart()
		{
			Assert.AreEqual("AppService", "AppService".TrimStart((string[])null));
			Assert.AreEqual("AppService", "AppService".TrimStart(""));

			Assert.AreEqual("", "".TrimStart((string[])null));
			Assert.AreEqual(null, ((string)null).TrimStart(""));

			Assert.AreEqual("Service", "AppService".TrimStart("App"));
			Assert.AreEqual("Service", "AppAppService".TrimStart("App"));
			Assert.AreEqual("ServiceApp", "ServiceApp".TrimStart("App"));
			Assert.AreEqual("ServiceApp", "AppServiceApp".TrimStart("App"));
		}


	}

}