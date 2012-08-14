using System.Collections.Generic;

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
		public UserFilterRequest(string title, string name, List<string> value, bool inclusive = true)
		{
			var expressionType = ExpressionTypes.Equal;
			if (value.Count > 1)
				expressionType = ExpressionTypes.In;

			if (!inclusive)
			{
				expressionType = ExpressionTypes.NotEqual;
				if (value.Count > 1)
					expressionType = ExpressionTypes.NotIn;
			}

			//default, equals pattern
			var pattern = "[{0}]=[{1}]";
			var expression = string.Format(pattern, name, string.Join(",", value));
			if (expressionType == ExpressionTypes.NotEqual)
			{
				pattern = "[{0}]<>[{1}]";
				expression = string.Format(pattern, name, string.Join(",", value));
			}
			if (expressionType == ExpressionTypes.In || expressionType == ExpressionTypes.NotIn)
			{
				pattern = (expressionType == ExpressionTypes.In) ? "[{0}] IN ([{1}])" : "[{0}] NOT IN ([{1}])";
				expression = string.Format(pattern, name, string.Join("],[", value));
			}
		
			UserFilter = new UserFilter
			             	{
			             		Content = new UserFilterContent()
			             		          	{
			             		          		Expression = expression
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
