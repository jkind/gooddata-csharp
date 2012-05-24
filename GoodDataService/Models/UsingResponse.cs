using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoodDataService
{
	public class UsingResponse
	{
		public UsingDTO Using { get; set; }
	}

	public class UsingDTO
	{
		public List<Edges> Edges { get; set; }
		public List<Node> Nodes { get; set; }
	}

	public class Edges
	{
		public string To { get; set; }
		public string From { get; set; }
	}

	public class Node
	{
		public string Link { get; set; }
		public string Author { get; set; }
		public DateTime Created { get; set; }
		public string Deprecated { get; set; }
		public string Summary { get; set; }
		public DateTime Updated { get; set; }
		public string Title { get; set; }
		public string Category { get; set; }
		public string Contributor { get; set; }

		[JsonIgnore]
		public string ObjectId
		{
			get { return Link.ExtractObjectId(); }
		}
	}
}