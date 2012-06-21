using System.IO;
using GoodDataService;
using GoodDataService.Api;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class ExportTests
	{
		private readonly ApiWrapper reportingService;

		public ExportTests()
		{
			reportingService = new ApiWrapper();
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


		[TestCase(ExportFormatTypes.csv)]
		//[TestCase(ExportFormatTypes.xls)]
		//[TestCase(ExportFormatTypes.pdf)]
		public void ExportReport_ExpectSucces(ExportFormatTypes exportFormatTypes)
		{
			var title = reportingService.Config.Domain;
			var project = reportingService.FindProjectByTitle(title);
			Assert.NotNull(project);

			var reports = reportingService.Query(project.ProjectId, ObjectTypes.Report);
			var reportUri = reports[0].Link;
			var result = reportingService.ExportReport(reportUri, exportFormatTypes);

			Assert.IsNotNull(result);
			var file = reportingService.GetFile(result);
			Assert.Greater(file.ContentLength, 0);
			var bytes = reportingService.GetFileContents(file);
			Assert.IsNotNull(bytes);
			var filePath = Path.GetTempPath() + "x.tmp";
			File.WriteAllBytes(filePath, bytes);
			Assert.True(File.Exists(filePath));
		}
	}
}