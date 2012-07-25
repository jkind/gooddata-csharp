namespace GoodDataService
{
	public class SystemRoles
	{
		public const string Admin = "adminRole";
		public const string Editor = "editorRole";
		public const string DashboardOnly = "dashboardOnlyRole";
		public const string Viewer = "readOnlyUserRole";
	}

	public class SystemPlatforms
	{
		public const string MySql = "mysql";
		public const string PostGres = "Pg";
	}

	public enum ExpressionTypes
	{
		Equal,
		NotEqual,
		In,
		NotIn
	}

	public enum ObjectTypes
	{
		Dashboard,
		Report,
		Metric,
		Attribute,
		UserFilter
	}

	public enum ExportFormatTypes
	{
		pdf,
		csv,
		png,
		xls
	}
}