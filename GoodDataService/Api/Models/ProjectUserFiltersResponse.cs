using System.Collections.Generic;

namespace GoodDataService.Api.Models
{
	public class ProjectUserFiltersResponse
	{
		public ProjectUserFilters UserFilters { get; set; }
	}

	public class ProjectUserFilters
	{
		public int Count { get; set; }
		public int Length { get; set; }
		public int Offset { get; set; }
		public List<Item> Items { get; set; }
	}

	public class Item
	{
		public List<string> UserFilters { get; set; }
		public string User { get; set; }
	}
}