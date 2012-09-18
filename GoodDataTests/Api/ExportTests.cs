using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class ExportTests : BaseTest
	{

		[Test]
		public void ExportProject_ExpectSucces()
		{
			var response = ReportingService.ExportProject(GetTestProjectId(), false, true);
			Assert.NotNull(response);
		}

		[Test]
		public void ExportPartials_ExpectSucces()
		{
			var projectId = GetTestProjectId();
			var uris = ReportingService.GetQueryLinks(projectId, ObjectTypes.Dashboard);
			var response = ReportingService.ExportPartials(projectId, uris);
			Assert.NotNull(response);
		}

		[Test]
		public void ImportPartials_ExpectSucces()
		{
			var projectId = GetTestProjectId();
			var uris = ReportingService.GetQueryLinks(projectId, ObjectTypes.Dashboard);
			var response = ReportingService.ExportPartials(projectId, new List<string>() {uris.First()});
			var importResponse = ReportingService.ImportPartials(projectId,response.Token);
			Assert.NotNull(importResponse);
		}


		[TestCase(ExportFormatTypes.csv)]
		//[TestCase(ExportFormatTypes.xls)]
		//[TestCase(ExportFormatTypes.pdf)]
		public void ExportReport_ExpectSucces(ExportFormatTypes exportFormatTypes)
		{
			var projectId = GetTestProjectId();
			var reports = ReportingService.Query(projectId, ObjectTypes.Report);
			var reportUri = reports[0].Link;
			var result = ReportingService.ExportReport(reportUri, exportFormatTypes);

			Assert.IsNotNull(result);
			var file = ReportingService.GetFile(result);
			Assert.Greater(file.ContentLength, 0);
			var bytes = ReportingService.GetFileContents(file);
			Assert.IsNotNull(bytes);
			var filePath = Path.GetTempPath() + "x.tmp";
			File.WriteAllBytes(filePath, bytes);
			Assert.True(File.Exists(filePath));
		}
	}
}