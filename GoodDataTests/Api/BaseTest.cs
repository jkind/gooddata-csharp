using System.Configuration;
using System.Diagnostics;
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

		[DebuggerStepThrough]
		public Project GetTestProject()
		{
			return ReportingService.FindProjectByTitle(TestProjectName);
		}

		[DebuggerStepThrough]
		public string GetTestProjectId()
		{
			var project = GetTestProject();
			return project.ProjectId;
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