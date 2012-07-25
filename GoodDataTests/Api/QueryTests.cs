using System;
using System.Linq;
using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	public class QueryTests : BaseTest
	{
		public string TestProjectId { get; set; }
		public QueryTests()
		{
			TestProjectId = GetTestProject().ProjectId;
		}

		[Test]
		public void GetReportTest()
		{
			var items = ReportingService.Query(TestProjectId, ObjectTypes.Report);
			Assert.IsNotNull(items);
		}

		[Test]
		public void GetDashboardsTest()
		{
			var items = ReportingService.Query(TestProjectId, ObjectTypes.Dashboard);
			Assert.IsNotNull(items);
		}

		[Test]
		public void GetUserFitlersTest()
		{
			var items = ReportingService.Query(TestProjectId, ObjectTypes.UserFilter);
			Assert.IsNotNull(items);
		}

		[Test]
		public void GetMetricsTest()
		{
			var reports = ReportingService.Query(TestProjectId, ObjectTypes.Metric);
			Assert.IsNotNull(reports);
		}

		[Test]
		public void GetProjectReportFullObjectsTest()
		{
			var items = ReportingService.GetObjectMetaData(TestProjectId, ObjectTypes.Report);
			Assert.IsNotNull(items);
			Assert.IsNotNullOrEmpty(items[0].Identifier);
			Assert.IsNotNullOrEmpty(items[0].Title);
			Assert.IsNotNull(items[0].Summary);
		}

		[Test]
		public void GetProjectDashboardFullObjectsTest()
		{
			var items = ReportingService.GetObjectMetaData(TestProjectId, ObjectTypes.Dashboard);
			Assert.IsNotNull(items);
			Assert.IsNotNullOrEmpty(items[0].Identifier);
			Assert.IsNotNullOrEmpty(items[0].Title);
			Assert.IsNotNull(items[0].Summary);
		}

		[Test]
		public void GetDepenciesTest()
		{
			var items = ReportingService.Query(TestProjectId, ObjectTypes.Dashboard);
			foreach (var item in items)
			{
				var usingReponse = ReportingService.GetDependancies(TestProjectId, item.ObjectId, true);
				Assert.IsNotNull(usingReponse);
				var reports =
					usingReponse.Using.Nodes.Where(x => x.Category.Equals("report", StringComparison.OrdinalIgnoreCase)).ToList();
				Assert.GreaterOrEqual(reports.Count, 1);
			}
		}
	}
}