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
		public static List<Entry> FindByTitle(this List<Entry> entries, string title)
		{
			return entries.Where(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public static List<Entry> FindByGreaterThanLastUpdated(this List<Entry> entries, DateTime lastUpdated)
		{
			return entries.Where(x => x.Updated > lastUpdated).ToList();
		}

		public static List<Entry> FindByTag(this List<Entry> entries, List<string> tagFilter)
		{
			var filteredEntries = new List<Entry>();
			foreach (var item in entries)
			{
				if (!string.IsNullOrEmpty(item.Tags))
				{
					var tags = item.Tags.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
					foreach (var tag in tags)
					{
						if (tagFilter.Any(t=>t.Equals(tag, StringComparison.OrdinalIgnoreCase)))
						{
							filteredEntries.Add(item);
						}
					}
				}

			}
			return filteredEntries;
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