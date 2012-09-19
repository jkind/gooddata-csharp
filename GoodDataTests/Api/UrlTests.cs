using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class UrlTests
	{
		[Test]
		public void Combine()
		{
			Assert.AreEqual(string.Concat("https://secure.gooddata.com", Constants.MD_URI, "projectid", "/obj"),
			                Url.Combine("https://secure.gooddata.com", Constants.MD_URI, "projectid", "obj"));
		}
	}
}