using System.Collections.Generic;

namespace GoodDataService.Api.Models
{
	public class AttributeElemntsResponse
	{
		public AttributeElements AttributeElements { get; set; }
		public ElementsMeta ElementsMeta { get; set; }
	}

	public class ElementsMeta
	{
		public string Attribute { get; set; }
		public string AttributeDisplayForm { get; set; }
		public string Count { get; set; }
		public string Filter { get; set; }
		public string Mode { get; set; }
		public string Offset { get; set; }
		public string Order { get; set; }
		public string Prompt { get; set; }
		public string Records { get; set; }
	}

	public class AttributeElements
	{
		public List<Element> Elements { get; set; }
	}

	public class Element
	{
		public string Title { get; set; }
		public string Uri { get; set; }
	}


}
