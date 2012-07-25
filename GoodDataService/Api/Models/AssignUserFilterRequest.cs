using System.Collections.Generic;

namespace GoodDataService.Api.Models
{
//{ 
//    "userFilters": {
//        "items": [
//            {
//                "user": "/gdc/account/profile/<user-id>",
//                "userFilters": [
//                    "/gdc/md/<project-id>/obj/<user-filter-object-id>"
//                ]
//            }
//        ]
//    }
//}

	public class AssignUserFilterRequest
	{
		public AssignUserFilter UserFilters { get; set; }
		public AssignUserFilterRequest(IEnumerable<string> userprofileIds, List<string> userFilterUris)
		{
			UserFilters = new AssignUserFilter();
			foreach (var profileId in userprofileIds)
			{
				UserFilters.Add(profileId, userFilterUris);
			}
		}
	}

	public class AssignUserFilter
	{
		public List<AssignUserFilterContent> Items { get; set; }
		public void Add(string userProfileId,List<string> userFilters)
		{
			if (Items == null) Items = new List<AssignUserFilterContent>();
			Items.Add(new AssignUserFilterContent(userProfileId,userFilters));
		}
	}


	public class  AssignUserFilterContent
	{
		public AssignUserFilterContent(string userProfileId,List<string> userFilters)
		{
			User = "/gdc/account/profile/" + userProfileId;
			UserFilters = userFilters;
		}

		public string User { get; set; }
		public List<string> UserFilters { get; set; }
	}

	
	public class AssignUserFilterResponse
	{
		public AssignUserFiltersUpdateResult UserFiltersUpdateResult { get; set; }
	}

	public class AssignUserFiltersUpdateResult
	{
		public List<string> Failed { get; set; }
		public List<string> Successful { get; set; }
	}
}
