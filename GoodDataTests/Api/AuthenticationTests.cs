using GoodDataService.Api;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class AuthenticationTests
	{
		private readonly ApiWrapper reportingService;

		public AuthenticationTests()
		{
			reportingService = new ApiWrapper();
		}

		[Test]
		public void Authenticate_GoodCredentials_ExpectSucces()
		{
			reportingService.Authenticate(reportingService.Config.Login, reportingService.Config.Password);
			Assert.IsNotNullOrEmpty(reportingService.ProfileId);
		}

		[Test]
		public void GetToken_ExpectSuccess()
		{
			reportingService.Authenticate(reportingService.Config.Login, reportingService.Config.Password);
			reportingService.GetToken();
			Assert.IsNotNullOrEmpty(reportingService.ProfileId);
		}
	}
}