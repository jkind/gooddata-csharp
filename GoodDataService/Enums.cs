namespace GoodDataService
{
	public enum Roles
	{
		Admin,
		Editor,
		DashboardOnly
	}

	public enum ObjectTypes
	{
		Dashboard,
		Report,
		Metric
	}

	public enum ExportFormatTypes
	{
		pdf,
		csv,
		png,
		xls
	}
}