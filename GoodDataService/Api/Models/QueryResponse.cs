using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoodDataService.Api.Models
{
	public class QueryResponse
	{
		public Query Query { get; set; }
	}

	public class Query
	{
		public List<Entry> Entries { get; set; }
		public QueryMeta Meta { get; set; }
	}

	public class Entry
	{
		public string Link { get; set; }
		public string Author { get; set; }
		public string Tags { get; set; }
		public string Created { get; set; }
		public string Deprecated { get; set; }
		public string Summary { get; set; }
		public string Updated { get; set; }
		public string Title { get; set; }
		public string Category { get; set; }
		public string Contributor { get; set; }

		[JsonIgnore]
		public string ObjectId
		{
			get { return Link.ExtractObjectId(); }
		}
	}

	public class QueryMeta
	{
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Category { get; set; }
	}
}