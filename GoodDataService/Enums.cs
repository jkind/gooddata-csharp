namespace GoodDataService
{
	public enum Roles
	{
		Admin=1,
		Editor=2,
		DashboardOnly=3,
		Viewer = 5
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