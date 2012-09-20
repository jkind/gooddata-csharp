using System;
using System.Collections.Generic;
using GoodDataService.Api.Models;
using Newtonsoft.Json;
using log4net;
using System.Linq;

namespace GoodDataService.Api
{
	public class MandatoryUserFilter : ApiWrapperBase
	{
		// https://secure.gooddata.com/gdc/md/projectid/userfilters
		// https://secure.gooddata.com/gdc/md/projectid/query/userfilters

		private static readonly ILog Logger = LogManager.GetLogger(typeof (MandatoryUserFilter));

		public void Assign(string projectId, string userUri, IEnumerable<string> filterUris)
		{
			Logger.DebugFormat("BEGIN MandatoryUserFilter.Assign ProjectId={0} User={1}", projectId, userUri);
			CheckAuthentication();

			var url = Url.Combine(Config.Url, Constants.MD_URI, projectId, "userfilters");
			var payload = new AssignUserFilterRequest(new[] {userUri}, filterUris.ToList());
			var response = PostRequest(url, payload);

			Logger.DebugFormat("END MandatoryUserFilter.Assign Response={0}", response);
		}

		public string Create(string projectId, string title, string expression)
		{
			Logger.DebugFormat("BEGIN MandatoryUserFilter.Create ProjectId={0}, Title={1}, Expression={2}", projectId, title, expression);
			CheckAuthentication();

			var url = Url.Combine(Config.Url, Constants.MD_URI, projectId, "obj");
			var request = new UserFilterRequest
				              {
					              UserFilter = new UserFilter
						                           {
							                           Content = new UserFilterContent {Expression = expression},
							                           Meta = new UserFilterMeta {Title = title}
						                           }
				              };
			var response = PostRequest(url, request);
			var filterResponse = JsonConvert.DeserializeObject<UriResponse>(response);

			Logger.DebugFormat("END MandatoryUserFilter.Create");
			return filterResponse.Uri;
		}

		public UserFilter Get(string filterUri)
		{
			Logger.DebugFormat("BEGIN MandatoryUserFilter.Get FilterUri={0}", filterUri);
			CheckAuthentication();

			var url = Url.Combine(Config.Url, filterUri);
			var response = GetRequest(url);
			var filter = JsonConvert.DeserializeObject<UserFilterRequest>(response).UserFilter;
			
			Logger.DebugFormat("END MandatoryUserFilter.Get");

			return filter;
		}

		public List<Entry> All(string projectId)
		{
			Logger.DebugFormat("BEGIN MandatoryUserFilter.All ProjectId={0}", projectId);

			CheckAuthentication();
			var results = new ApiWrapper().Query(projectId, ObjectTypes.UserFilter);

			Logger.DebugFormat("END MandatoryUserFilter.All");
			return results;
		}

		public void Update(string filterUri, string newTitle=null, string newExpression=null)
		{
			Logger.DebugFormat("BEGIN MandatoryUserFilter.Update FilterUri={0} NewTitle={1} NewExpression={2}", filterUri, newTitle ?? "<null>", newExpression ?? "<null>");

			if (null == newTitle && null == newExpression)
			{
				Logger.DebugFormat("END MandatoryUserFilter.Update (nothing to update)");
				return;
			}
				
			CheckAuthentication();
			var url = Url.Combine(Config.Url, filterUri);
			var response = GetRequest(url);
			var filter = JsonConvert.DeserializeObject<UserFilterRequest>(response);

			if (null != newExpression)
				filter.UserFilter.Content.Expression = newExpression;

			if (null != newTitle)
				filter.UserFilter.Meta.Title = newTitle;

			PostRequest(url, filter);

			Logger.DebugFormat("END MandatoryUserFilter.Update");
		}
	}
}