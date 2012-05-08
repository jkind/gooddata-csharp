using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	public class QueryTests
	{
		private readonly GoodDataService.ApiWrapper reportingService;

		public QueryTests()
		{
			reportingService = new GoodDataService.ApiWrapper();
		}
		[Test]
		public void GetReportTests()
		{
			var project= reportingService.FindProjectByTitle(reportingService.Config.Domain);
			var reports = reportingService.Query(project.ProjectId, QueryTypes.Report);
			Assert.IsNotNull(reports);
		}

		[Test]
		public void GetDashboardsTests()
		{
			var project = reportingService.FindProjectByTitle(reportingService.Config.Domain);
			var reports = reportingService.Query(project.ProjectId, QueryTypes.Dashboard);
			Assert.IsNotNull(reports);
		}
	}
}
