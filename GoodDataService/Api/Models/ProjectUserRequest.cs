using System.Collections.Generic;

namespace GoodDataService.Api.Models
{
	public class ProjectUserRequest
	{
		public UserRequest User { get; set; }

		#region Nested type: UserRequest

		public class UserRequest
		{
			public ContentRequest Content { get; set; }
			public LinksRequest Links { get; set; }

			#region Nested type: ContentRequest

			public class ContentRequest
			{
				public string Status { get; set; }
				public List<string> UserRoles { get; set; }
			}

			#endregion

			#region Nested type: LinksRequest

			public class LinksRequest
			{
				public string Self { get; set; }
			}

			#endregion
		}

		#endregion
	}
}