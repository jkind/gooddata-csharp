using System;
using System.Linq;
using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class ProjectTests : BaseTest
	{

		[Test]
		public void GetProjectUserFilters_ExpectSucces()
		{
			var reponse = ReportingService.GetProjectUserFilters(GetTestProjectId());
			Assert.IsNotNull(reponse, string.Format("{0} Users with User Filters",reponse.Count.ToString()));
		}

		[Test]
		public void CreateProject_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var projectId = ReportingService.CreateProject(title, "Summary" + title);

			var projects = ReportingService.FindProjectByTitle(title);
			Assert.NotNull(projects);
			Assert.AreEqual("Pg", projects.Content.Driver);

			ReportingService.DeleteProject(projectId);

			projects = ReportingService.FindProjectByTitle(title);

			Assert.IsNull(projects);
		}

		[Test]
		public void CreateProjectMySql_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var projectId = ReportingService.CreateProject(title, "Summary" + title,null,SystemPlatforms.MySql);

			var projects = ReportingService.FindProjectByTitle(title);
			Assert.NotNull(projects);
			Assert.AreEqual("mysql", projects.Content.Driver);

			ReportingService.DeleteProject(projectId);

			projects = ReportingService.FindProjectByTitle(title);

			Assert.IsNull(projects);
		}

		[Test]
		[Ignore]
		public void CreateProject()
		{
			var title = ReportingService.Config.Domain + "Tester";
			var projectId = ReportingService.CreateProject(title, "Summary " + title);

			var projects = ReportingService.FindProjectByTitle(title);
			Assert.NotNull(projects);
		}

		[Test]
		[Ignore]
		public void DeleteProject()
		{
			var title = ReportingService.Config.Domain + "Tester";
			var projects = ReportingService.FindProjectByTitle(title);
			ReportingService.DeleteProject(projects.ProjectId);

			projects = ReportingService.FindProjectByTitle(title);

			Assert.IsNull(projects);
		}

		[Test]
		[Ignore]
		public void DeleteProjects()
		{
			var projects = ReportingService.GetProjects().Where(x => x.Meta.Title.Contains("local - ") | x.Meta.Title.Contains("dev - ") | x.Meta.Title.Contains("release - ") | x.Meta.Title.Contains("staging - ")).ToList();
			projects.ForEach(x => Console.WriteLine(x.Meta.Title));
			//foreach (var project in projects)
			//{
			//    try
			//    {
			//        ReportingService.DeleteProject(project.ProjectId);
			//    }
			//    catch (Exception ex)
			//    {
			//        Console.WriteLine(ex);
			//    }
			//}
			Assert.NotNull(projects);
		}
	}
}