using System;
using System.Linq;
using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	public class QueryTests
	{
		private ApiWrapper _reportingService;

		[SetUp]
		public void Setup()
		{
			_reportingService = new ApiWrapper();
		}

		[Test]
		public void GetReportTest()
		{
			var project = _reportingService.FindProjectByTitle(_reportingService.Config.Domain);
			var reports = _reportingService.Query(project.ProjectId, ObjectTypes.Report);
			Assert.IsNotNull(reports);
		}

		[Test]
		public void GetDashboardsTest()
		{
			var project = _reportingService.FindProjectByTitle(_reportingService.Config.Domain);
			var reports = _reportingService.Query(project.ProjectId, ObjectTypes.Dashboard);
			Assert.IsNotNull(reports);
		}

		[Test]
		public void GetMetricsTest()
		{
			var project = _reportingService.FindProjectByTitle(_reportingService.Config.Domain);
			var reports = _reportingService.Query(project.ProjectId, ObjectTypes.Metric);
			Assert.IsNotNull(reports);
		}

		[Test]
		public void GetProjectReportFullObjectsTest()
		{
			var project = _reportingService.FindProjectByTitle(_reportingService.Config.Domain);
			var items = _reportingService.GetFullObjects(project.ProjectId, ObjectTypes.Report);
			Assert.IsNotNull(items);
			Assert.IsNotNullOrEmpty(items[0].Identifier);
			Assert.IsNotNullOrEmpty(items[0].Title);
			Assert.IsNotNull(items[0].Summary);
		}

		[Test]
		public void GetProjectDashboardFullObjectsTest()
		{
			var project = _reportingService.FindProjectByTitle(_reportingService.Config.Domain);
			var items = _reportingService.GetFullObjects(project.ProjectId, ObjectTypes.Dashboard);
			Assert.IsNotNull(items);
			Assert.IsNotNullOrEmpty(items[0].Identifier);
			Assert.IsNotNullOrEmpty(items[0].Title);
			Assert.IsNotNull(items[0].Summary);
		}

		[Test]
		public void GetDashboardReportsTest()
		{
			var project = _reportingService.FindProjectByTitle(_reportingService.Config.Domain);
			var items = _reportingService.Query(project.ProjectId, ObjectTypes.Dashboard);
			foreach (var item in items)
			{
				var usingReponse = _reportingService.GetDependancies(project.ProjectId, item.ObjectId, true);
				Assert.IsNotNull(usingReponse);
				var reports =
					usingReponse.Using.Nodes.Where(x => x.Category.Equals("report", StringComparison.OrdinalIgnoreCase)).ToList();
				Assert.GreaterOrEqual(reports.Count, 1);
			}
		}
	}
}