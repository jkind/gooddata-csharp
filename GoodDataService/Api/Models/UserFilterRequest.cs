namespace GoodDataService.Api.Models
{
//    {
//    "userFilter": {
//        "content": {
//            "expression": "[/gdc/md/PROJECT_ID/obj/OBJECT_ID]=[/gdc/md/PROJECT_ID/obj/OBJECT_ID/elements?id=ELEMENT_ID]"
//        },
//        "meta": {
//            "category": "userFilter",
//            "title": "User Filter Name"
//}
//    }
//    }

	public class UserFilterRequest
	{
		public UserFilter UserFilter { get; set; }
		public UserFilterRequest(string title, string name, string value, ExpressionTypes expressionType)
		{
			var pattern = "[{0}]=[{1}]";
			switch (expressionType)
			{
				case ExpressionTypes.NotEqual:
					pattern = "[{0}]<>[{1}]";
					break;
				case ExpressionTypes.In:
					pattern = "[{0}] IN [{1}]";
					break;
				case ExpressionTypes.NotIn:
					pattern = "[{0}] NOT IN [{1}]";
					break;
			}
			UserFilter = new UserFilter()
			             	{
			             		Content = new UserFilterContent()
			             		          	{
			             		          		Expression = string.Format(pattern, name, value)
			             		          	},
			             		Meta = new UserFilterMeta()
			             		       	{
			             		       		Title = title
			             		       	}
			             	};

		}
	}

	public class UserFilter
	{
		public UserFilterContent Content { get; set; }
		public UserFilterMeta Meta { get; set; }
	}

	public class UserFilterMeta
	{
		public string Category { get; set; }
		public string Title { get; set; }
	}

	public class UserFilterContent
	{
		public string Expression { get; set; }
		
	}
}
