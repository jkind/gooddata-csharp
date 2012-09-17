using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GoodDataService.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoodDataService.Api
{
	public class ApiWrapper : ApiWrapperBase
	{
		#region Project

		public string CreateProject(string title, string summary, string template = null, string driver = SystemPlatforms.PostGres)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI);
			var payload = new ProjectResult()
			              	{
			              		Project = new Project
			              		          	{
			              		          		Content = new ProjectContent
			              		          		          	{
			              		          		          		GuidedNavigation = 1,
																Driver = driver
			              		          		          	},
			              		          		Meta = new Meta
			              		          		       	{
			              		          		       		Title = title,
			              		          		       		Summary = summary,
			              		          		       		ProjectTemplate = template
			              		          		       	}
			              		          	}
			              	};
			var response = PostRequest(url, payload);
			var projectResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return projectResponse.Uri.ExtractId(Constants.PROJECTS_URI);
		}

		public List<Project> GetProjects()
		{
			CheckAuthentication();
			var list = new List<Project>();
			var url = string.Concat(Config.Url, Constants.PROFILE_URI, "/", ProfileId, "/projects");
			var response = GetRequest(url);
			var projectResponse = JsonConvert.DeserializeObject(response, typeof (ProjectsResponse)) as ProjectsResponse;
			if (projectResponse != null)
			{
				projectResponse.Projects.ForEach(p=>list.Add(p.Project));
			}
			return list;
		}

		public void DeleteProject(string projectId)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId);
			DeleteRequest(url);
		}

		public void DeleteObjectByTitle(string projectId, string title, ObjectTypes objectType)
		{
			var item = FindObjectByTitle(projectId, title, objectType);
			item.ForEach(i=>DeleteObject(projectId, i.Link));
		}

		public void DeleteObject(string projectId, string relativeUri)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, relativeUri);
			DeleteRequest(url);
		}

		public Project FindProjectByTitle(string title)
		{
			var projects = GetProjects();
			return projects.FirstOrDefault(u => u.Meta.Title.Equals((title ?? "").Trim(), StringComparison.OrdinalIgnoreCase));
		}

		#endregion

		#region Export/Import

		public string ExecuteReport(string reportUri, ExportFormatTypes exportFormatType = ExportFormatTypes.csv)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.EXECUTOR);

			var payload = new ExecuteReportRequest
			              	{
			              		report_req = new ExecuteResult(exportFormatType)
			              		             	{
			              		             		Report = reportUri
			              		             	}
			              	};
			var response = PostRequest(url, payload);
			dynamic exportResponse = JsonConvert.DeserializeObject<object>(response);
			return exportResponse.reportResult2.meta.uri;
		}

		public string ExportReport(string reportUri, ExportFormatTypes exportFormatType = ExportFormatTypes.csv)
		{
			CheckAuthentication();
			var executeUri = ExecuteReport(reportUri);

			var url = string.Concat(Config.Url, Constants.EXPORT_EXECUTOR);

			var payload = new ExportReportRequest
			              	{
			              		result_req = new ResultRequest(exportFormatType)
			              		             	{
			              		             		Report = executeUri
			              		             	}
			              	};
			var response = PostRequest(url, payload);
			var exportResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return exportResponse.Uri;
		}

		public ExportArtifact ExportProject(string projectId, bool exportUsers = false, bool exportData = false)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, Constants.PROJECT_EXPORT_URI);

			var payload = new ExportRequest
			              	{
			              		ExportProject = new ExportProject
			              		                	{
			              		                		ExportUsers = Convert.ToInt16(exportUsers),
			              		                		ExportData = Convert.ToInt16(exportData)
			              		                	}
			              	};
			var response = PostRequest(url, payload);
			var exportResponse = JsonConvert.DeserializeObject(response, typeof (ExportResponse)) as ExportResponse;
			return exportResponse.ExportArtifact;
		}

		public ExportArtifact ExportPartials(string projectId, List<string> uris)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, Constants.PROJECT_PARTIAL_EXPORT_URI);

			var payload = new PartialExportRequest
			              	{
			              		PartialMDExport = new PartialMDExport
			              		                  	{
			              		                  		Uris = uris
			              		                  	}
			              	};
			var response = PostRequest(url, payload);
			var exportResponse = JsonConvert.DeserializeObject(response, typeof (PartialExportResponse)) as PartialExportResponse;
			return exportResponse.PartialMDArtifact;
		}

		public string ImportProject(string projectId, string token)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, Constants.PROJECT_IMPORT_URI);

			var payload = new ImportRequest
			              	{
			              		ImportProject = new ImportProject
			              		                	{
			              		                		Token = token
			              		                	}
			              	};
			var response = PostRequest(url, payload);
			var importResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return importResponse.Uri;
		}

		public string ImportPartials(string projectId, string token, bool overwriteNewer = true,
		                             bool updateLdmObjects = false)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, Constants.PROJECT_PARTIAL_IMPORT_URI);

			var payload = new PartialImportRequest
			              	{
			              		PartialMDImport = new PartialMDImport
			              		                  	{
			              		                  		Token = token,
			              		                  		OverwriteNewer = (overwriteNewer) ? 1 : 0,
			              		                  		UpdateLDMObjects = (updateLdmObjects)? 1 : 0
			              		                  	}
			              	};
			var response = PostRequest(url, payload);
			var importResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return importResponse.Uri;
		}

		public bool PollStatus(string uri)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, uri);
			var response = GetRequest(url);
			var taskResponse = JsonConvert.DeserializeObject(response, typeof (TaskResponse)) as TaskResponse;
			return (taskResponse.TaskState.Status == Enum.GetName(typeof (TaskStates), TaskStates.OK));
		}

		public bool PollPartialStatus(string uri)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, uri);
			var response = GetRequest(url);
			var taskResponse = JsonConvert.DeserializeObject(response, typeof(PartialTaskResponse)) as PartialTaskResponse;
			return (taskResponse.wTaskStatus.Status == "OK");
		}

		public WebResponse GetFile(string uri)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, uri);
			return GetFileResponse(url);
		}

		public byte[] GetFileContents(WebResponse response)
		{
			CheckAuthentication();
			using (var stream = response.GetResponseStream())
			{
				var buffer = new byte[int.Parse(response.Headers["Content-Length"])];
				var bytesRead = 0;
				var totalBytesRead = bytesRead;
				while (totalBytesRead < buffer.Length)
				{
					bytesRead = stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
					totalBytesRead += bytesRead;
				}
				return buffer;
			}
		}

		#endregion

		#region User

		public string CreateUser(string login, string password, string verfiyPassword, string firstName, string lastName, string ssoProvider = "", string country = "US")
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.DOMAIN_URI, "/", Config.Domain, Constants.DOMAIN_USERS_SUFFIX);
			var payload = new DomainUserRequest
			              	{
			              		AccountSetting = new AccountSetting
			              		                 	{
			              		                 		Login = login,
			              		                 		Password = password,
			              		                 		VerifyPassword = verfiyPassword,
			              		                 		FirstName = firstName,
			              		                 		LastName = lastName,
														SsoProvider = ssoProvider,
														Country = country
			              		                 	}
			              	};
			var response = PostRequest(url, payload);
			var userResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return userResponse.Uri.ExtractId(Constants.PROFILE_URI);
		}

		public void DeleteUserFilter(string projectId, string filterTitle)
		{
			DeleteObjectByTitle(projectId, filterTitle, ObjectTypes.UserFilter);
		}


		public string CreateUserFilter(string projectId, string filterTitle, Dictionary<string,List<string>> fillterCollection, bool inclusive = true)
		{
			var items = new Dictionary<string, List<string>>();
			var attributes = Query(projectId, ObjectTypes.Attribute);
			foreach (var item in fillterCollection)
			{
				var attribute = FindAttributeByTitle(projectId, item.Key, attributes);
				if (attribute == null)
				{
					Console.WriteLine(string.Format("No attribute found with title {0}", item.Key));
					return null;
				}

				var attributeElements = GetAttributeElements(projectId, attribute);

				var elements = new List<Element>();
				foreach (var elementTitle in item.Value)
				{
					var fullAttribute = FindAttributeElementByTitle(projectId, attribute, elementTitle, attributeElements);
					if (fullAttribute != null)
					{
						elements.Add(fullAttribute);
					}

				}
				if (elements.Count == 0)
				{
					Console.WriteLine(string.Format("No element {0} found for attribute {1}", string.Join(",", item.Value),
					                                attribute.Meta.Identifier));
					return null;
				}
				items.Add(attribute.Meta.Uri,elements.Select(element => element.Uri).ToList());
			}
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, "/obj");
			var payload = new UserFilterRequest(filterTitle, items, inclusive);
			var response = PostRequest(url, payload);
			var filterResponse = JsonConvert.DeserializeObject(response, typeof(UriResponse)) as UriResponse;
			return filterResponse.Uri;
		}

		public AssignUserFiltersUpdateResult AssignUserFilters(string projectId, List<string> userprofileIds, List<string> userFilterUris)
		{
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, "/userfilters");
			var payload = new AssignUserFilterRequest(userprofileIds,userFilterUris);
			var response = PostRequest(url, payload);
			var assignResponse = JsonConvert.DeserializeObject(response, typeof(AssignUserFilterResponse)) as AssignUserFilterResponse;
			return assignResponse.UserFiltersUpdateResult;
		}

		public List<Entry> FindObjectByTitle(string projectId, string title, ObjectTypes objectType)
		{
			var item = Query(projectId, objectType).FindByTitle((title ?? "").Trim());
			if (item == null) return null;
			return item;
		}

		public Models.Attribute FindAttributeByTitle(string projectId, string attributeTitle,  List<Entry> attributes=null)
		{
			if (attributes == null)
				attributes = Query(projectId, ObjectTypes.Attribute);
			if (attributes == null)
				return null;
			attributes = attributes.FindByTitle((attributeTitle ?? "").Trim());
	
			var url = string.Concat(Config.Url, attributes.First().Link);
			var response = GetRequest(url);
			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new BoolConverter());
			var attributeResponse = JsonConvert.DeserializeObject(response, typeof (AttributeResponse),settings) as AttributeResponse;
			return attributeResponse.Attribute;
		}

		public List<Element> GetAttributeElements(string projectId, Models.Attribute attribute)
		{
			var url = string.Concat(Config.Url, attribute.Content.DisplayForms[0].Links.Elements);
			var response = GetRequest(url);
			var attributeElemntsResponse = JsonConvert.DeserializeObject(response, typeof(AttributeElemntsResponse)) as AttributeElemntsResponse;
			return attributeElemntsResponse.AttributeElements.Elements;
		}

		public Element FindAttributeElementByTitle(string projectId, Models.Attribute attribute, string elementTitle, List<Element> elements=null)
		{
			if (elements ==null)
				elements = GetAttributeElements(projectId, attribute);
			return elements.FirstOrDefault(x => x.Title.Equals(elementTitle, StringComparison.OrdinalIgnoreCase));
		}

		public List<ProjectRole> GetRoles(string projectId)
		{
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId, Constants.PROJECT_ROLES_SUFFIX);
			var response = GetRequest(url);
			var rolesResponse = JsonConvert.DeserializeObject(response, typeof(RolesResponse)) as RolesResponse;
			var list = new List<ProjectRole>();
			foreach (var uri in rolesResponse.ProjectRoles.Roles)
			{
				var rawRoleResponse = GetRequest(string.Concat(Config.Url,uri));
				var roleResponse = JsonConvert.DeserializeObject(rawRoleResponse, typeof (RoleResponse)) as RoleResponse;
				roleResponse.ProjectRole.RoleId = uri.ExtractObjectId();
				roleResponse.ProjectRole.Meta.Uri = uri; 
				list.Add(roleResponse.ProjectRole);
			}
			return list;
		}

		public ProjectRole FindRoleByTitle(string projectId, string systemRole = SystemRoles.DashboardOnly)
		{
			return
				GetRoles(projectId).FirstOrDefault(r => r.Meta.Identifier.Equals(systemRole, StringComparison.OrdinalIgnoreCase));
		}

		public void AddUsertoProject(string projectId, string userId, string roleName = SystemRoles.DashboardOnly)
		{
			CheckAuthentication();

			var projectRole = FindRoleByTitle(projectId, roleName);
			if (projectRole == null)
			{
				throw new ArgumentException(string.Format("No role found for role name: {0}", roleName));
			}
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId, Constants.PROJECT_USERS_SUFFIX);

			var payload = new ProjectUserRequest
			              	{
			              		User = new ProjectUserRequest.UserRequest
			              		       	{
			              		       		Content = new ProjectUserRequest.UserRequest.ContentRequest
			              		       		          	{
			              		       		          		Status = "ENABLED",
			              		       		          		UserRoles = new List<string>
			              		       		          		            	{
			              		       		          		            		string.Concat(Constants.PROJECTS_URI, "/", projectId,
			              		       		          		            		              Constants.PROJECT_ROLES_SUFFIX,
			              		       		          		            		              "/", projectRole.RoleId)
			              		       		          		            	}
			              		       		          	},
			              		       		Links = new ProjectUserRequest.UserRequest.LinksRequest
			              		       		        	{
			              		       		        		Self = string.Concat(Constants.PROFILE_URI, "/", userId)
			              		       		        	}
			              		       	}
			              	};
			PostRequest(url, payload);
		}

		public void UpdateProjectUserAccess(string projectId, string profileId, bool enabled, string roleName = SystemRoles.DashboardOnly)
		{
			CheckAuthentication();
			var role = FindRoleByTitle(projectId, roleName);
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId, Constants.PROJECT_USERS_SUFFIX);
			var payload = new ProjectUserRequest
			              	{
			              		User = new ProjectUserRequest.UserRequest
			              		       	{
			              		       		Content = new ProjectUserRequest.UserRequest.ContentRequest
			              		       		          	{
			              		       		          		Status = (enabled) ? "ENABLED" : "DISABLED",
															UserRoles = new List<string>{role.Meta.Uri}
			              		       		          	},
			              		       		Links = new ProjectUserRequest.UserRequest.LinksRequest
			              		       		        	{
			              		       		        		Self = string.Concat(Constants.PROFILE_URI, "/", profileId)
			              		       		        	}
			              		       	}
			              	};
			PostRequest(url, payload);
		}

		public void UpdateProfileSettings(string projectId, string profileId)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.PROFILE_URI, profileId, Constants.PROFILE_SETTINGS_SUFFIX);
			var payload = ProfileSettingsRequest.CreateUSFormat();
			PostRequest(url, payload);
		}

		public void UpdateSSOProvider(string profileId)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.PROFILE_URI, "/",profileId);
			var payload = new DomainUserRequest()
			              	{
			              		AccountSetting = new AccountSetting()
			              		                 	{
			              		                 		SsoProvider = Config.Domain + ".com"
			              		                 	}
			              	};
			PutRequest(url, payload);
		}


		public List<AccountResponseSettingWrapper> GetDomainUsers(string domain = "")
		{
			CheckAuthentication();
			if (string.IsNullOrEmpty(domain))
				domain = Config.Domain;
			var list = new List<AccountResponseSettingWrapper>();
			var url = string.Concat(Config.Url, Constants.DOMAIN_URI, "/", domain, Constants.DOMAIN_USERS_SUFFIX);
			GetDomainUsers(url, ref list);
			return list;
		}

		private void GetDomainUsers(string url, ref List<AccountResponseSettingWrapper> list)
		{
			var response = GetRequest(url);
			try
			{
				var usersResponse = JsonConvert.DeserializeObject(response, typeof(CreateDomainUserResponse)) as CreateDomainUserResponse;
				if (usersResponse != null)
				{
					list.AddRange(usersResponse.AccountSettings.Items);
				}

				var o = JObject.Parse(response);
				var nextSetUrl = (string)o["accountSettings"]["paging"]["next"];
				if (nextSetUrl != null)
				{
					GetDomainUsers(string.Concat(Config.Url, nextSetUrl),ref list);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public List<UserWrapper> GetProjectUsers(string projectId)
		{
			CheckAuthentication();
			var list = new List<UserWrapper>();
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId, Constants.DOMAIN_USERS_SUFFIX);
			var response = GetRequest(url);
			var usersResponse = JsonConvert.DeserializeObject(response, typeof (UserResponse)) as UserResponse;
			if (usersResponse != null)
			{
				list.AddRange(usersResponse.Users);
			}
			return list;
		}

		public User FindProjectUsersByEmail(string projectId, string email)
		{
			var users = GetProjectUsers(projectId);
			var userWrapper = users.FirstOrDefault(u => u.User.Content.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
			return userWrapper != null ? userWrapper.User : null;
		}

		public List<User> GetFullProjectUsers(string projectId)
		{
			var list = new List<User>();
			var users = GetProjectUsers(projectId);
			foreach (var userWrapper in users)
			{
				//Fetch roles
				var roleNames = new List<string>();
				foreach (var role in userWrapper.User.Content.UserRoles)
				{
					var url = string.Concat(Config.Url, role);
					var response = GetRequest(url);
					dynamic roleResponse = JsonConvert.DeserializeObject<object>(response);
					var roleName = (string) roleResponse.projectRole.meta.title;
					roleNames.Add(roleName);
				}
				userWrapper.User.RoleNames = roleNames;

				//Fetch UserFilters
				var userfilterNames = new List<string>();
				var userfilters = Query(projectId, ObjectTypes.UserFilter);
				userfilters = userfilters.Where(x => x.Title.Contains(userWrapper.User.Content.Email)).ToList();
				foreach (var userfilter in userfilters)
				{
					if (userfilter.Title.Contains(userWrapper.User.Content.Email))
					{
						var url = string.Concat(Config.Url, userfilter.Link);
						var response = GetRequest(url);
						var userFilterResponse =
							JsonConvert.DeserializeObject(response, typeof (UserFilterResponse)) as UserFilterResponse;
						var elements = userFilterResponse.UserFilter.Content.Objects.Where(x => x.Category == "attributeElement");
						var titles = elements.Select(userFilterObject => userFilterObject.Title).ToList();
						userfilterNames.Add(userfilter.Title.Replace(userWrapper.User.Content.Email + " - ", "") + ": " + string.Join(", ", titles));
						Console.WriteLine("yes");
					}
				}
				userWrapper.User.UserFilterNames = userfilterNames;
				list.Add(userWrapper.User);
			}
			return list;
		}

		public User GetFullProjectUsersByEmail(string projectId, string email)
		{
			var users = GetProjectUsers(projectId);
			var userWrapper = users.FirstOrDefault(u => u.User.Content.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
			
			//Fetch roles
			var roleNames = new List<string>();
			foreach (var role in userWrapper.User.Content.UserRoles)
			{
				var url = string.Concat(Config.Url, role);
				var response = GetRequest(url);
				dynamic roleResponse = JsonConvert.DeserializeObject<object>(response);
				var roleName = (string) roleResponse.projectRole.meta.title;
				roleNames.Add(roleName);
			}
			userWrapper.User.RoleNames = roleNames;

			//Fetch UserFilters
			var userfilterNames = new List<string>();
			var userfilters = Query(projectId, ObjectTypes.UserFilter);
			userfilters = userfilters.Where(x => x.Title.Contains(email)).ToList();
			foreach (var userfilter in userfilters)
			{
				if (userfilter.Title.Contains(email))
				{
					var url = string.Concat(Config.Url, userfilter.Link);
					var response = GetRequest(url);
					var userFilterResponse = JsonConvert.DeserializeObject(response, typeof (UserFilterResponse)) as UserFilterResponse;
					var elements = userFilterResponse.UserFilter.Content.Objects.Where(x=>x.Category=="attributeElement");
					var titles = elements.Select(userFilterObject => userFilterObject.Title).ToList();
					userfilterNames.Add(userfilter.Title.Replace(email + " - ", "") + ": " +string.Join(", ", titles));
					Console.WriteLine("yes");
				}
			}
			userWrapper.User.UserFilterNames = userfilterNames;

			

			return userWrapper.User;
		}

		public AccountResponseSetting FindDomainUsersByLogin(string email, string domain="")
		{
			var users = GetDomainUsers(domain);
			var userWrapper = users.FirstOrDefault(u => u.AccountSetting.Login.Equals(email, StringComparison.OrdinalIgnoreCase));
			return userWrapper != null ? userWrapper.AccountSetting : null;
		}

		public void DeleteUser(string profileId)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.PROFILE_URI, "/", profileId);
			DeleteRequest(url);
		}

		#endregion

		#region Management

		public IdentifiersResponse GetUris(string projectId, List<string> identifiers)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, projectId, Constants.IDENTIFIER_URI);

			var payload = identifiers;
			var response = PostRequest(url, payload);
			return JsonConvert.DeserializeObject(response, typeof (IdentifiersResponse)) as IdentifiersResponse;
		}

		public UsingResponse GetDependancies(string projectId, string objectId, bool? filterByReport = null)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, "/using/", objectId);
			var response = GetRequest(url);
			var usingResponse = JsonConvert.DeserializeObject(response, typeof (UsingResponse)) as UsingResponse;
			if (usingResponse != null)
			{
				return usingResponse;
			}
			return null;
		}

		public string GetObjectIdentifier(string objectLink)
		{
			var objectResponse = GetObject(objectLink);
			if (objectResponse != null)
			{
				return objectResponse.meta.identifier;
			}
			return null;
		}

		public dynamic GetObject(string objectLink)
		{
			CheckAuthentication();
			var url = string.Concat(Config.Url, objectLink);
			var response = GetRequest(url);
			return JsonConvert.DeserializeObject<object>(response);
		}

		public List<Entry> Query(string projectId, ObjectTypes objectTypes)
		{
			CheckAuthentication();
			var fragment = Constants.REPORT_QUERY;
			if (objectTypes == ObjectTypes.Dashboard)
				fragment = Constants.DASHBOARD_QUERY;
			if (objectTypes == ObjectTypes.Metric)
				fragment = Constants.METRICS_QUERY;
			if (objectTypes == ObjectTypes.Attribute)
				fragment = Constants.ATTRIBUTES_QUERY;
			if (objectTypes == ObjectTypes.UserFilter)
				fragment = Constants.USERFILTER_QUERY;
			var response = GetRequest(string.Concat(Config.Url, Constants.MD_URI, projectId, fragment));
			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new BoolConverter());
			var queryResponse = JsonConvert.DeserializeObject(response, typeof(QueryResponse), settings) as QueryResponse;
			return queryResponse != null ? queryResponse.Query.Entries : null;
		}

		public List<string> GetQueryLinks(string projectId, ObjectTypes objectTypes)
		{
			var entries = Query(projectId, objectTypes);
			var uris = new List<string>();
			entries.ForEach(x => uris.Add(x.Link));
			return uris;
		}

		public List<ObjectMeta> GetObjectMetaData(string projectId, ObjectTypes objectTypes)
		{
			var entries = Query(projectId, objectTypes);
			var list = new List<ObjectMeta>();
			foreach (var entry in entries)
			{
				var item = GetObject(entry.Link);
				switch (objectTypes)
				{
					case ObjectTypes.Dashboard:
						list.Add(ObjectMeta.MapDashboard(item));
						break;
					case ObjectTypes.Report:
						list.Add(ObjectMeta.MapReport(item));
						break;
					case ObjectTypes.UserFilter:
						list.Add(ObjectMeta.MapUserFilter(item));
						break;
					default:
						list.Add(ObjectMeta.MapMetric(item));
						break;
				}
			}
			return list;
		}

		public List<ObjectMeta> GetActiveObjects(string projectId, ObjectTypes objectTypes)
		{
			var entries = GetObjectMetaData(projectId, objectTypes);
			return entries.Where(x => x.Deprecated == false).ToList();
		}

		#endregion
	}

	public class ProfileSettingsRequest
	{
		public static ProfileSettingsRequest CreateUSFormat()
		{
			return new ProfileSettingsRequest
			       	{
			       		ProfileSetting = new ProfileSetting
			       		                 	{
			       		                 		Separators = new Separators
			       		                 		             	{
			       		                 		             		Decimal = ".",
			       		                 		             		Thousand = ","
			       		                 		             	}
			       		                 	}
			       	};
		}

		public ProfileSetting ProfileSetting { get; set; }
	}

	public class ProfileSetting
	{
		public Separators Separators { get; set; }

	}
	public class Separators
	{
		public string Thousand { get; set; }
		public string Decimal { get; set; }
    }
		
}