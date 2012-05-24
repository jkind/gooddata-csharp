using System;

namespace GoodDataService.Models
{
	public class ObjectMeta
	{
		public string Uri { get; set; }
		public string Author { get; set; }
		public string Tags { get; set; }
		public DateTime Created { get; set; }
		public string Identifier { get; set; }
		public bool Deprecated { get; set; }
		public string Summary { get; set; }
		public DateTime Updated { get; set; }
		public string Title { get; set; }
		public string Category { get; set; }
		public string Contributor { get; set; }
		public ObjectTypes ObjectType { get; set; }
		public dynamic Content { get; set; }

		public static ObjectMeta MapReport(dynamic item)
		{
			return new ObjectMeta
			       	{
			       		ObjectType = ObjectTypes.Report,
			       		Content = item.report,
			       		Uri = item.report.meta.uri,
			       		Author = item.report.meta.author,
			       		Tags = item.report.meta.tags,
			       		Created = item.report.meta.created,
			       		Identifier = item.report.meta.identifier,
			       		Deprecated = item.report.meta.deprecated == "0" ? false : true,
			       		Summary = item.report.meta.summary,
			       		Updated = item.report.meta.updated,
			       		Title = item.report.meta.title,
			       		Category = item.report.meta.category,
			       		Contributor = item.report.meta.contributor
			       	};
		}

		public static ObjectMeta MapDashboard(dynamic item)
		{
			return new ObjectMeta
			       	{
			       		ObjectType = ObjectTypes.Dashboard,
			       		Uri = item.projectDashboard.meta.uri,
			       		Author = item.projectDashboard.meta.author,
			       		Tags = item.projectDashboard.meta.tags,
			       		Created = item.projectDashboard.meta.created,
			       		Identifier = item.projectDashboard.meta.identifier,
			       		Deprecated = item.projectDashboard.meta.deprecated == "0" ? false : true,
			       		Summary = item.projectDashboard.meta.summary,
			       		Updated = item.projectDashboard.meta.updated,
			       		Title = item.projectDashboard.meta.title,
			       		Category = item.projectDashboard.meta.category,
			       		Contributor = item.projectDashboard.meta.contributor
			       	};
		}

		public static ObjectMeta MapMetric(dynamic item)
		{
			return new ObjectMeta
			       	{
			       		ObjectType = ObjectTypes.Metric,
			       		Content = item.metric,
			       		Uri = item.metric.meta.uri,
			       		Author = item.metric.meta.author,
			       		Tags = item.metric.meta.tags,
			       		Created = item.metric.meta.created,
			       		Identifier = item.metric.meta.identifier,
			       		Deprecated = item.metric.meta.deprecated == "0" ? false : true,
			       		Summary = item.metric.meta.summary,
			       		Updated = item.metric.meta.updated,
			       		Title = item.metric.meta.title,
			       		Category = item.metric.meta.category,
			       		Contributor = item.metric.meta.contributor
			       	};
		}
	}
}