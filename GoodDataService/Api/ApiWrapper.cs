using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GoodDataService.Api.Models;
using Newtonsoft.Json;

namespace GoodDataService.Api
{
	public class ApiWrapper : ApiWrapperBase
	{
		#region Project

		public string CreateProject(string title, string summary, string template = null)
		{
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI);
			var payload = new ProjectResponse
			              	{
			              		Project = new Project
			              		          	{
			              		          		Content = new ProjectContent
			              		          		          	{
			              		          		          		GuidedNavigation = 1
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

		public List<ProjectResponse> GetProjects()
		{
			var list = new List<ProjectResponse>();
			var url = string.Concat(Config.Url, Constants.PROFILE_URI, "/", ProfileId, "/projects");
			var response = GetRequest(url);
			var projectResponse = JsonConvert.DeserializeObject(response, typeof (ProjectsResponse)) as ProjectsResponse;
			if (projectResponse != null)
			{
				list.AddRange(projectResponse.Projects);
			}
			return list;
		}

		public void DeleteProject(string projectId)
		{
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId);
			DeleteRequest(url);
		}

		public Project FindProjectByTitle(string title)
		{
			var projects = GetProjects();
			var projectWrapper =
				projects.FirstOrDefault(u => string.Compare(u.Project.Meta.Title, title, StringComparison.OrdinalIgnoreCase) == 0);
			return projectWrapper != null ? projectWrapper.Project : null;
		}

		#endregion

		#region Export/Import

		public string ExecuteReport(string reportUri, ExportFormatTypes exportFormatType = ExportFormatTypes.csv)
		{
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
			return exportResponse.PartialMdArtifact;
		}

		public string ImportProject(string projectId, string token)
		{
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

		public string ImportPartials(string projectId, string token, bool overwriteNewer = false,
		                             bool updateLdmObjects = false)
		{
			var url = string.Concat(Config.Url, Constants.MD_URI, projectId, Constants.PROJECT_PARTIAL_IMPORT_URI);

			var payload = new PartialImportRequest
			              	{
			              		PartialMdImport = new PartialMdImport
			              		                  	{
			              		                  		Token = token,
			              		                  		OverwriteNewer = overwriteNewer,
			              		                  		UpdateLdmObjects = updateLdmObjects
			              		                  	}
			              	};
			var response = PostRequest(url, payload);
			var importResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return importResponse.Uri;
		}

		public bool PollStatus(string uri)
		{
			var url = string.Concat(Config.Url, uri);
			var response = GetRequest(url);
			var taskResponse = JsonConvert.DeserializeObject(response, typeof (TaskResponse)) as TaskResponse;
			return (taskResponse.TaskState.Status == Enum.GetName(typeof (TaskStates), TaskStates.OK));
		}

		public WebResponse GetFile(string uri)
		{
			var url = string.Concat(Config.Url, uri);
			return GetFileResponse(url);
		}

		public byte[] GetFileContents(WebResponse response)
		{
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

		public string CreateUser(string login, string password, string verfiyPassword, string firstName, string lastName)
		{
			var url = string.Concat(Config.Url, Constants.DOMAIN_URI, "/", Config.Domain, Constants.DOMAIN_USERS_SUFFIX);
			var payload = new CreateDomainUser
			              	{
			              		AccountSetting = new AccountSetting
			              		                 	{
			              		                 		Login = login,
			              		                 		Password = password,
			              		                 		VerifyPassword = verfiyPassword,
			              		                 		FirstName = firstName,
			              		                 		LastName = lastName
			              		                 	}
			              	};
			var response = PostRequest(url, payload);
			var userResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return userResponse.Uri.ExtractId(Constants.PROFILE_URI);
		}

		public void AddUsertoProject(string projectId, string userId, Roles role = Roles.DashboardOnly)
		{
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
			              		       		          		            		              "/", (int) role)
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

		public void UpdateProjectUserStatus(string projectId, string profileId, bool enabled)
		{
			var url = string.Concat(Config.Url, Constants.PROJECTS_URI, "/", projectId, Constants.PROJECT_USERS_SUFFIX);
			var payload = new ProjectUserRequest
			              	{
			              		User = new ProjectUserRequest.UserRequest
			              		       	{
			              		       		Content = new ProjectUserRequest.UserRequest.ContentRequest
			              		       		          	{
			              		       		          		Status = (enabled) ? "ENABLED" : "DISABLED"
			              		       		          	},
			              		       		Links = new ProjectUserRequest.UserRequest.LinksRequest
			              		       		        	{
			              		       		        		Self = string.Concat(Constants.PROFILE_URI, "/", profileId)
			              		       		        	}
			              		       	}
			              	};
			PostRequest(url, payload);
		}

		public List<AccountResponseSettingWrapper> GetDomainUsers(string domain = "")
		{
			if (string.IsNullOrEmpty(domain))
				domain = Config.Domain;
			var list = new List<AccountResponseSettingWrapper>();
			var url = string.Concat(Config.Url, Constants.DOMAIN_URI, "/", domain, Constants.DOMAIN_USERS_SUFFIX);
			var response = GetRequest(url);
			var usersResponse =
				JsonConvert.DeserializeObject(response, typeof (CreateDomainUserResponse)) as CreateDomainUserResponse;
			if (usersResponse != null)
			{
				list.AddRange(usersResponse.AccountSettings.Items);
			}
			return list;
		}

		public List<UserWrapper> GetProjectUsers(string projectId)
		{
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
			var userWrapper =
				users.FirstOrDefault(u => string.Compare(u.User.Content.Email, email, StringComparison.OrdinalIgnoreCase) == 0);
			return userWrapper != null ? userWrapper.User : null;
		}

		public AccountResponseSetting FindDomainUsersByLogin(string email, string domain = "")
		{
			var users = GetDomainUsers(domain);
			var userWrapper =
				users.FirstOrDefault(u => string.Compare(u.AccountSetting.Login, email, StringComparison.OrdinalIgnoreCase) == 0);
			return userWrapper != null ? userWrapper.AccountSetting : null;
		}

		public void DeleteUser(string profileId)
		{
			var url = string.Concat(Config.Url, Constants.PROFILE_URI, "/", profileId);
			DeleteRequest(url);
		}

		#endregion

		#region Management

		public IdentifiersResponse GetUris(string projectId, List<string> identifiers)
		{
			var url = string.Concat(Config.Url, projectId, Constants.IDENTIFIER_URI);

			var payload = identifiers;
			var response = PostRequest(url, payload);
			return JsonConvert.DeserializeObject(response, typeof (IdentifiersResponse)) as IdentifiersResponse;
		}

		public UsingResponse GetDependancies(string projectId, string objectId, bool? filterByReport = null)
		{
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
			var url = string.Concat(Config.Url, objectLink);
			var response = GetRequest(url);
			return JsonConvert.DeserializeObject<object>(response);
		}

		public List<Entry> Query(string projectId, ObjectTypes objectTypes)
		{
			var fragment = Constants.REPORT_QUERY;
			if (objectTypes == ObjectTypes.Dashboard)
				fragment = Constants.DASHBOARD_QUERY;
			if (objectTypes == ObjectTypes.Metric)
				fragment = Constants.METRICS_QUERY;
			var response = GetRequest(string.Concat(Config.Url, Constants.MD_URI, projectId, fragment));

			var queryResponse = JsonConvert.DeserializeObject(response, typeof (QueryResponse)) as QueryResponse;
			return queryResponse != null ? queryResponse.Query.Entries : null;
		}

		public List<string> GetQueryLinks(string projectId, ObjectTypes objectTypes)
		{
			var entries = Query(projectId, objectTypes);
			var uris = new List<string>();
			entries.ForEach(x => uris.Add(x.Link));
			return uris;
		}

		public List<ObjectMeta> GetFullObjects(string projectId, ObjectTypes objectTypes)
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
					default:
						list.Add(ObjectMeta.MapMetric(item));
						break;
				}
			}
			return list;
		}

		public List<ObjectMeta> GetActiveObjects(string projectId, ObjectTypes objectTypes)
		{
			var entries = GetFullObjects(projectId, objectTypes);
			return entries.Where(x => x.Deprecated == false).ToList();
		}

		#endregion
	}
}