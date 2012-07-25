using System.Configuration;
using GoodDataService.Api;
using GoodDataService.Api.Models;

namespace GoodDataTests.Api
{
	public class BaseTest
	{
		public ApiWrapper ReportingService { get; private set; }
		public BaseTest()
		{
			ReportingService =new ApiWrapper();
		}

		public string TestProjectName
		{
			get { return ConfigurationManager.AppSettings["TestProject"]; }
		}

		public Project GetTestProject()
		{
			return ReportingService.FindProjectByTitle(TestProjectName);
		}

		public string CreateTestUser(string email)
		{
			var password = "password";
			var firstName = "sso";
			var lastName = "admin";

			return ReportingService.CreateUser(email, password, password, firstName, lastName, ReportingService.Config.Domain + ".com");
		}
	}
}