using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class AuthenticationTests : BaseTest
	{
		[Test]
		public void Authenticate_GoodCredentials_ExpectSucces()
		{
			ReportingService.Authenticate(ReportingService.Config.Login, ReportingService.Config.Password);
			Assert.IsNotNullOrEmpty(ReportingService.ProfileId);
		}

		[Test]
		public void GetToken_ExpectSuccess()
		{
			ReportingService.Authenticate(ReportingService.Config.Login, ReportingService.Config.Password);
			ReportingService.GetToken();
			Assert.IsNotNullOrEmpty(ReportingService.ProfileId);
		}
	}
}