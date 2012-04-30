using System;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class ProjectTests
	{
		private readonly GoodDataService.ApiWrapper reportingService;

		public ProjectTests()
		{
			reportingService = new GoodDataService.ApiWrapper();
		}

		[Test]
		public void CreateProject_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var projectId = reportingService.CreateProject(title, "Summary" + title);

			var projects = reportingService.FindProjectByTitle(title);
			Assert.NotNull(projects);

			reportingService.DeleteProject(projectId);

			projects = reportingService.FindProjectByTitle(title);

			Assert.IsNull(projects);
		}

		[Test]
		[Ignore]
		public void CreateProject()
		{
			var title = reportingService.Config.Domain + "Tester";
			var projectId = reportingService.CreateProject(title, "Summary " + title);

			var projects = reportingService.FindProjectByTitle(title);
			Assert.NotNull(projects);
		}

		[Test]
		[Ignore]
		public void DeleteProject()
		{
			var title = reportingService.Config.Domain + "Tester";
			var projects = reportingService.FindProjectByTitle(title);
			reportingService.DeleteProject(projects.ProjectId);

			projects = reportingService.FindProjectByTitle(title);

			Assert.IsNull(projects);
		}

		
	}
}