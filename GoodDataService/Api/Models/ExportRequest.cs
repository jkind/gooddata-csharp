using System;
using System.Collections.Generic;

namespace GoodDataService.Api.Models
{
	public class ExportRequest
	{
		public ExportProject ExportProject { get; set; }
	}

	public class ExportProject
	{
		public short ExportUsers { get; set; }
		public short ExportData { get; set; }
	}

	public class ExportResponse
	{
		public ExportArtifact ExportArtifact { get; set; }
	}

	public class ExportArtifact
	{
		public ExportStatus Status { get; set; }
		public string Token { get; set; }
	}

	public class ExportStatus
	{
		public string Uri { get; set; }
	}

	public class ImportProject
	{
		public string Token { get; set; }
	}

	public class PartialExportRequest
	{
		public PartialMDExport PartialMDExport { get; set; }
	}

	public class PartialMDExport
	{
		public List<string> Uris { get; set; }
	}

	public class PartialExportResponse
	{
		public ExportArtifact PartialMDArtifact { get; set; }
	}

	public class PartialImportRequest
	{
		public PartialMDImport PartialMDImport { get; set; }
	}

	public class PartialMDImport
	{
		public string Token { get; set; }
		public int OverwriteNewer { get; set; }
		public int UpdateLDMObjects { get; set; }
	}

	public class IdentifiersResponse
	{
		public string Identifier { get; set; }
		public string Uri { get; set; }
	}

	public class ExportReportRequest
	{
		public ResultRequest result_req { get; set; }
	}

	public class ExecuteReportRequest
	{
		public ExecuteResult report_req { get; set; }
	}

	public class ExecuteResult
	{
		public ExecuteResult(ExportFormatTypes exportFormatType)
		{
			Format = Enum.GetName(typeof (ExportFormatTypes), exportFormatType);
		}

		public string Report { get; set; }
		public string Format { get; private set; }
	}

	public class ResultRequest
	{
		public ResultRequest(ExportFormatTypes exportFormatType)
		{
			Format = Enum.GetName(typeof (ExportFormatTypes), exportFormatType);
		}

		public string Report { get; set; }
		public string Format { get; private set; }
	}

	public class TaskResponse
	{
		public TaskState TaskState { get; set; }
	}

	public enum TaskStates
	{
		RUNNING,
		OK
	}

	public class TaskState
	{
		public string Msg { get; set; }
		public string Status { get; set; }
	}

	public class PartialTaskResponse
	{
		public wTaskStatus wTaskStatus { get; set; }
	}

	public class wTaskStatus
	{
		public string Status { get; set; }
	}
}