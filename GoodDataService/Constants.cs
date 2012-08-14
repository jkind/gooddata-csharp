namespace GoodDataService
{
	public static class Constants
	{
		public static readonly string ATTR_QUERY = "/query/attributes";
		public static readonly string DATA_INTERFACES_URI = "/ldm/singleloadinterface";
		public static readonly string DML_EXEC_URI = "/dml/manage";
		public static readonly string DOMAIN_URI = "/gdc/account/domains";
		public static readonly string DOMAIN_USERS_SUFFIX = "/users";
		public static readonly string ETL_MODE_DLI = "DLI";
		public static readonly string ETL_MODE_URI = "/etl/mode";
		public static readonly string ETL_MODE_VOID = "VOID";
		public static readonly string EXECUTOR = "/gdc/xtab2/executor3";
		public static readonly string EXPORT_EXECUTOR = "/gdc/exporter/executor";
		public static readonly string IDENTIFIER_URI = "/identifiers";
		public static readonly string INVITATION_URI = "/invitations";
		public static readonly string LOGIN_URI = "/gdc/account/login";
		public static readonly string MAQL_EXEC_URI = "/ldm/manage";
		public static readonly string MD_URI = "/gdc/md/";
		public static readonly string OBJ_URI = "/obj";
		public static readonly string PROFILE_URI = "/gdc/account/profile";
		public static readonly string PROFILE_SETTINGS_SUFFIX = "/settings";
		public static readonly string PROJECTS_URI = "/gdc/projects";
		public static readonly string PROJECT_EXPORT_URI = "/maintenance/export";
		public static readonly string PROJECT_IMPORT_URI = "/maintenance/import";
		public static readonly string PROJECT_PARTIAL_EXPORT_URI = "/maintenance/partialmdexport";
		public static readonly string PROJECT_PARTIAL_IMPORT_URI = "/maintenance/partialmdimport";
		public static readonly string PROJECT_ROLES_SUFFIX = "/roles";
		public static readonly string PROJECT_USERS_SUFFIX = "/users";
		public static readonly string PULL_URI = "/etl/pull";
		public static readonly string REPORT_QUERY = "/query/reports";
		public static readonly string DASHBOARD_QUERY = "/query/projectdashboards";
		public static readonly string METRICS_QUERY = "/query/metrics";
		public static readonly string ATTRIBUTES_QUERY = "/query/attributes";
		public static readonly string USERFILTER_QUERY = "/query/userfilters";
		public static readonly string SLI_DESCRIPTOR_URI = "/descriptor";
		public static readonly string TOKEN_URI = "/gdc/account/token";
	}
}