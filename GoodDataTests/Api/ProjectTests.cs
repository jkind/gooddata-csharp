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
		public void DeleteMySqlProjects()
		{
			var projects = ReportingService.GetProjects().Where(x=>x.Content.Driver==SystemPlatforms.MySql).Where(x=>!x.Meta.Title.Contains("release")).ToList();
			projects.ForEach(x => Console.WriteLine(x.Meta.Title));
			//foreach (var project in projects)
			//{
			//    try
			//    {
			//        ReportingService.DeleteProject(project.ProjectId);
			//    }
			//    catch (Exception)
			//    {
					
			//    }
			//}
			Assert.NotNull(projects);
		}
	}
}