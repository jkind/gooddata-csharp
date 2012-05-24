using System;
using GoodDataService;
using GoodDataService.Api;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class ProjectTests
	{
		private readonly ApiWrapper reportingService;

		public ProjectTests()
		{
			reportingService = new ApiWrapper();
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
		public void DeleteProject()
		{
			var title = reportingService.Config.Domain + "Tester";
			var projects = reportingService.FindProjectByTitle(title);
			reportingService.DeleteProject(projects.ProjectId);

			projects = reportingService.FindProjectByTitle(title);

			Assert.IsNull(projects);
		}

		[Test]
		public void ExportPartials_ExpectSucces()
		{
			var title = reportingService.Config.Domain;
			var project = reportingService.FindProjectByTitle(title);
			Assert.NotNull(project);

			var uris = reportingService.GetQueryLinks(project.ProjectId, ObjectTypes.Dashboard);
			var response = reportingService.ExportPartials(project.ProjectId, uris);
			Assert.NotNull(response);
		}

		[Test]
		public void ExportProject_ExpectSucces()
		{
			var title = reportingService.Config.Domain;
			var project = reportingService.FindProjectByTitle(title);
			Assert.NotNull(project);

			var response = reportingService.ExportProject(project.ProjectId, false, true);
			Assert.NotNull(response);
		}
	}
}