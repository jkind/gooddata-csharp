using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GoodDataService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoodDataService
{
	public enum Roles
	{
		Admin = 1,
		Editor = 2,
		DashboardOnly = 3
	}
	public class ApiWrapper
	{
		public static readonly string ATTR_QUERY = "/query/attributes";
		public static readonly string DATA_INTERFACES_URI = "/ldm/singleloadinterface";
		public static readonly string DML_EXEC_URI = "/dml/manage";
		public static readonly string DOMAIN_URI = "/gdc/account/domains";
		public static readonly string DOMAIN_USERS_SUFFIX = "/users";
		public static readonly string ETL_MODE_DLI = "DLI";
		public static readonly string ETL_MODE_URI = "/etl/mode";
		public static readonly string ETL_MODE_VOID = "VOID";
		public static readonly string EXECUTOR = "/gdc/xtab2/executor3";
		public static readonly string EXPORT_EXECUTOR = "/gdc/exporter/executor";
		public static readonly string IDENTIFIER_URI = "/identifiers";
		public static readonly string INVITATION_URI = "/invitations";
		public static readonly string LOGIN_URI = "/gdc/account/login";
		public static readonly string MAQL_EXEC_URI = "/ldm/manage";
		public static readonly string MD_URI = "/gdc/md/";
		public static readonly string OBJ_URI = "/obj";
		public static readonly string PROFILE_URI = "/gdc/account/profile";
		public static readonly string PROJECTS_URI = "/gdc/projects";
		public static readonly string PROJECT_EXPORT_URI = "/maintenance/export";
		public static readonly string PROJECT_IMPORT_URI = "/maintenance/import";
		public static readonly string PROJECT_PARTIAL_EXPORT_URI = "/maintenance/partialmdexport";
		public static readonly string PROJECT_PARTIAL_IMPORT_URI = "/maintenance/partialmdimport";
		public static readonly string PROJECT_ROLES_SUFFIX = "/roles";
		public static readonly string PROJECT_USERS_SUFFIX = "/users";
		public static readonly string PULL_URI = "/etl/pull";
		public static readonly string REPORT_QUERY = "/query/reports";
		public static readonly string SLI_DESCRIPTOR_URI = "/descriptor";
		public static readonly string TOKEN_URI = "/gdc/account/token";


		public readonly CookieContainer _cookieJar;
		public string ProfileId { get; set; }

		public ApiWrapper()
		{
			Config = GoodDataConfigurationSection.GetConfig();
			_cookieJar = new CookieContainer();
			Authenticate(Config.Login, Config.Password);
			GetToken();
		}

		public ApiWrapper(string userName, string password)
		{
			Config = GoodDataConfigurationSection.GetConfig();
			_cookieJar = new CookieContainer();
			Authenticate(userName,password);
			GetToken();
		}

		public GoodDataConfigurationSection Config { get; set; }

		public void GetToken()
		{
			var url = Config.Url + TOKEN_URI;
			MakeRequest(url, "GET", "");
		}

		public void Authenticate(string userName, string password)
		{
			var url = Config.Url + LOGIN_URI;
			var payload = new AuthenticationRequest
			              	{
			              		PostUserLogin = new PostUserLogin
			              		                	{
			              		                		Login = userName,
			              		                		Password = password
			              		                	}
			              	};
			var response = MakeRequest(url, "POST", payload);
			var userResponse = JsonConvert.DeserializeObject(response, typeof (AuthenticationResponse)) as AuthenticationResponse;
			if (userResponse != null)
			{
				ProfileId = userResponse.UserLogin.State.ExtractId(LOGIN_URI);
			}
		}

		// callback used to validate the certificate in an SSL conversation
		private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain,
		                                       SslPolicyErrors policyErrors)
		{
			if (Convert.ToBoolean(Config.IgnoreSslErrors))
			{
				// allow any old dodgy certificate...
				return true;
			}
			return policyErrors == SslPolicyErrors.None;
		}

		public void DeleteProject(string projectId)
		{
			var url = string.Concat(Config.Url, PROJECTS_URI, "/", projectId);
			MakeRequest(url, "DELETE", null);
		}

		public List<ProjectDto> GetProjects()
		{
			var list = new List<ProjectDto>();
			var url = string.Concat(Config.Url, PROFILE_URI, "/", ProfileId, "/projects");
			var response = MakeRequest(url, "GET", null);
			var projectResponse = JsonConvert.DeserializeObject(response, typeof (ProjectsDto)) as ProjectsDto;
			if (projectResponse != null)
			{
				list.AddRange(projectResponse.Projects);
			}
			return list;
		}

		public string CreateProject(string title, string summary, string template="")
		{
			var url = string.Concat(Config.Url, PROJECTS_URI);
			var payload = new ProjectDto
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
			var response = MakeRequest(url, "POST", payload);
			var projectResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return projectResponse.Uri.ExtractId(PROJECTS_URI);
		}

		public string CreateUser(string login, string password, string verfiyPassword, string firstName, string lastName)
		{
			var url = string.Concat(Config.Url, DOMAIN_URI, "/", Config.Domain, DOMAIN_USERS_SUFFIX);
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
			var response = MakeRequest(url, "POST", payload);
			var userResponse = JsonConvert.DeserializeObject(response, typeof (UriResponse)) as UriResponse;
			return userResponse.Uri.ExtractId(PROFILE_URI);
		}

		public void AddUsertoProject(string projectId, string userId, Roles role = Roles.DashboardOnly)
		{
			var url = string.Concat(Config.Url, PROJECTS_URI, "/", projectId, PROJECT_USERS_SUFFIX);
			var payload = new ProjectUserRequest
			              	{
			              		User = new ProjectUserRequest.UserRequest
			              		       	{
			              		       		Content = new ProjectUserRequest.UserRequest.ContentRequest
			              		       		          	{
			              		       		          		Status = "ENABLED",
			              		       		          		UserRoles = new List<string>
			              		       		          		            	{
			              		       		          		            		string.Concat(PROJECTS_URI, "/", projectId, PROJECT_ROLES_SUFFIX,
			              		       		          		            		              "/", (int) role)
			              		       		          		            	}
			              		       		          	},
			              		       		Links = new ProjectUserRequest.UserRequest.LinksRequest
			              		       		        	{
			              		       		        		Self = string.Concat(PROFILE_URI, "/", userId)
			              		       		        	}
			              		       	}
			              	};
			PostRequest(url, payload);
		}

		public void UpdateProjectUserStatus(string projectId, string profileId, bool enabled)
		{	
			var url = string.Concat(Config.Url, PROJECTS_URI, "/", projectId, PROJECT_USERS_SUFFIX);
			var payload = new ProjectUserRequest
			{
				User = new ProjectUserRequest.UserRequest
				{
					Content = new ProjectUserRequest.UserRequest.ContentRequest
					{
						Status = (enabled) ? "ENABLED": "DISABLED"
					},
					Links = new ProjectUserRequest.UserRequest.LinksRequest
					{
						Self = string.Concat(PROFILE_URI, "/", profileId)
					}
				}
			};
			PostRequest(url, payload);
		}


		private string PostRequest(string url, object postData)
		{
			return MakeRequest(url, "POST", postData);
		}


		private string MakeRequest(string url, string method, object postData)
		{
			var webRequest = WebRequest.Create(url) as HttpWebRequest;
			// allows for skipping validation warnings such as from self-signed certs
			ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;

			if (webRequest != null)
			{
				SetupRequest(webRequest, method);
				if (webRequest.Method == WebRequestMethods.Http.Post)
				{
					SetupPostData(webRequest, postData);
				}
			}

			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse) webRequest.GetResponse();
				using (var s = response.GetResponseStream())
				{
					using (var sr = new StreamReader(s))
					{
						return sr.ReadToEnd();
					}
				}
			}
			catch (WebException ex)
			{
				using (var exceptionResponse = ex.Response)
				{
					var httpResponse = (HttpWebResponse) exceptionResponse;
					Trace.WriteLine(string.Format("Error code: {0}", httpResponse.StatusCode));
					using (var data = exceptionResponse.GetResponseStream())
					{
						var text = new StreamReader(data).ReadToEnd();
						var errorResponse = JsonConvert.DeserializeObject(text, typeof (ErrorDto)) as ErrorDto;
						Trace.WriteLine(errorResponse.Error.Message);
						throw new GoodDataApiException(errorResponse.Error.Message);
					}
				}
			}
			finally
			{
				if (response != null)
					response.Close();
			}
		}

		private static void SetupPostData(HttpWebRequest webRequest, object postData)
		{
			StreamWriter requestWriter;
			var jsonData = JsonConvert.SerializeObject(postData, Formatting.None,
			                                           new JsonSerializerSettings
			                                           	{
			                                           		ContractResolver =
			                                           			new CamelCasePropertyNamesContractResolver(),
			                                           		NullValueHandling = NullValueHandling.Ignore
			                                           	});
			//Send the data.
			var encoding = new UTF8Encoding();
			var dataBytes = encoding.GetBytes(jsonData);
			webRequest.ContentLength = dataBytes.Length;
			using (requestWriter = new StreamWriter(webRequest.GetRequestStream(), encoding))
			{
				requestWriter.Write(jsonData);
			}
		}

		private void SetupRequest(HttpWebRequest webRequest, string method)
		{
			webRequest.CookieContainer = _cookieJar;
			//Hack to fix cookies domain
			//http://social.microsoft.com/Forums/en-US/netfxnetcom/thread/1297afc1-12d4-4d75-8d3f-7563222d234c
			var table = (Hashtable)_cookieJar.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, _cookieJar, new object[] { }); 
	        var keys = new ArrayList(table.Keys); 
	        foreach (var key in keys) 
	        { 
	            var newKey = ((string) key).Substring(1); 
	            table[newKey] = table[key]; 
	        }

			webRequest.Method = method.ToUpper();
			webRequest.ServicePoint.Expect100Continue = false;
			webRequest.ContentType = "application/json; charset=utf-8";
			webRequest.Accept = "application/json";
			webRequest.UserAgent = "GoodData CSharp CL/1.0";
			webRequest.Headers.Add("Accept-Charset", "utf-8");
		}

		public List<AccountResponseSettingWrapper> GetDomainUsers(string domain ="")
		{
			if (string.IsNullOrEmpty(domain))
				domain = Config.Domain;
			var list = new List<AccountResponseSettingWrapper>();
			var url = string.Concat(Config.Url, DOMAIN_URI, "/", domain, DOMAIN_USERS_SUFFIX);
			var response = MakeRequest(url, "GET", null);
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
			var url = string.Concat(Config.Url, PROJECTS_URI, "/", projectId, DOMAIN_USERS_SUFFIX);
			var response = MakeRequest(url, "GET", null);
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

		public AccountResponseSetting FindDomainUsersByLogin(string email,string domain="")
		{
			var users = GetDomainUsers(domain);
			var userWrapper =
				users.FirstOrDefault(u => string.Compare(u.AccountSetting.Login, email, StringComparison.OrdinalIgnoreCase) == 0);
			return userWrapper != null ? userWrapper.AccountSetting : null;
		}

		public void DeleteUser(string profileId)
		{
			var url = string.Concat(Config.Url, PROFILE_URI, "/", profileId);
			MakeRequest(url, "DELETE", null);
		}

		public Project FindProjectByTitle(string title)
		{
			var projects = GetProjects();
			var projectWrapper = projects.FirstOrDefault(u => string.Compare(u.Project.Meta.Title, title, StringComparison.OrdinalIgnoreCase) == 0);
			return projectWrapper != null ? projectWrapper.Project : null;
		}
	}
}