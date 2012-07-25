using System;
using System.Collections.Generic;
using System.Linq;
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

	public static class EntryFilters
	{
		public static Entry FindByTitle(this List<Entry> entries, string title)
		{
			return entries.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
		}

		public static Entry FindByLastUpdated(this List<Entry> entries, DateTime lastUpdated)
		{
			return entries.FirstOrDefault(x => x.Updated >= lastUpdated);
		}
	}

	public class Entry
	{
		public string Link { get; set; }
		public string Author { get; set; }
		public string Tags { get; set; }
		public DateTime Created { get; set; }
		public bool Deprecated { get; set; }
		public string Summary { get; set; }
		public DateTime? Updated { get; set; }
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