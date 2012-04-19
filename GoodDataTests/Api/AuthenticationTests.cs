using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class AuthenticationTests
	{
		private readonly GoodDataService.ApiWrapper reportingService;

		public AuthenticationTests()
		{
			reportingService = new GoodDataService.ApiWrapper();
		}

		[Test]
		public void Authenticate_GoodCredentials_ExpectSucces()
		{
			var profileId = reportingService.Authenticate(reportingService.Config.Login, reportingService.Config.Password);
			Assert.IsNotNullOrEmpty(profileId);
		}

		[Test]
		public void GetToken_ExpectSuccess()
		{
			var profileId = reportingService.Authenticate(reportingService.Config.Login, reportingService.Config.Password);
			reportingService.GetToken();
			Assert.IsNotNullOrEmpty(profileId);
		}
	}
}