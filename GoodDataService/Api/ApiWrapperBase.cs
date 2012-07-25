using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GoodDataService.Api.Models;
using GoodDataService.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoodDataService.Api
{
	public class ApiWrapperBase
	{
		public CookieContainer CookieJar;

		public ApiWrapperBase()
		{
			Config = GoodDataConfigurationSection.GetConfig();
			CookieJar = new CookieContainer();
			
		}

		public string ProfileId { get; set; }
		public GoodDataConfigurationSection Config { get; set; }

		public void GetToken()
		{
			var url = Config.Url + Constants.TOKEN_URI;
			GetRequest(url);
		}

		public void Authenticate(string userName, string password)
		{
			var url = Config.Url + Constants.LOGIN_URI;
			var payload = new AuthenticationRequest
			              	{
			              		PostUserLogin = new PostUserLogin
			              		                	{
			              		                		Login = userName,
			              		                		Password = password
			              		                	}
			              	};
			var response = PostRequest(url, payload);
			var userResponse = JsonConvert.DeserializeObject(response, typeof (AuthenticationResponse)) as AuthenticationResponse;
			if (userResponse != null)
			{
				ProfileId = userResponse.UserLogin.State.ExtractId(Constants.LOGIN_URI);
			}
		}


		/// <summary>
		/// 	Callback used to validate the certificate in an SSL conversation
		/// </summary>
		protected bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain,
		                                         SslPolicyErrors policyErrors)
		{
			if (Convert.ToBoolean(Config.IgnoreSslErrors))
			{
				// allow any old dodgy certificate...
				return true;
			}
			return policyErrors == SslPolicyErrors.None;
		}

		public string PostRequest(string url, object postData)
		{
			return MakeRequest(url, "POST", postData);
		}

		public string GetRequest(string url)
		{
			return MakeRequest(url, "GET", null);
		}

		public void DeleteRequest(string url)
		{
			MakeRequest(url, "DELETE", null);
		}

		public byte[] DownloadFile(string url)
		{
			using (var client = new WebClient())
			{
				return client.DownloadData(url);
			}
		}

		public WebResponse GetFileResponse(string url)
		{
			var webRequest = WebRequest.Create(url) as HttpWebRequest;
			// allows for skipping validation warnings such as from self-signed certs
			ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;

			if (webRequest != null)
			{
				SetupRequest(webRequest, "GET");
			}

			try
			{
				return webRequest.GetResponse();
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
						throw new GoodDataApiException(text);
					}
				}
			}
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
						throw new GoodDataApiException(text);
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
			webRequest.CookieContainer = CookieJar;
			//Hack to fix cookies domain
			//http://social.microsoft.com/Forums/en-US/netfxnetcom/thread/1297afc1-12d4-4d75-8d3f-7563222d234c
			var table =
				(Hashtable)
				CookieJar.GetType().InvokeMember("m_domainTable",
				                                 BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null,
				                                 CookieJar, new object[] {});
			var keys = new ArrayList(table.Keys);
			foreach (var key in keys)
			{
				if (!string.IsNullOrEmpty((string)key))
				{
					var newKey = ((string) key).Substring(1);
					table[newKey] = table[key];
				}
			}

			webRequest.Method = method.ToUpper();
			webRequest.ServicePoint.Expect100Continue = false;
			webRequest.ContentType = "application/json; charset=utf-8";
			webRequest.Accept = "application/json";
			webRequest.UserAgent = "GoodData CSharp CL/1.0";
			webRequest.Headers.Add("Accept-Charset", "utf-8");
		}

		protected void CheckAuthentication()
		{
			if (CookieJar.Count != 0) return;
			Authenticate(Config.Login, Config.Password);
			GetToken();
		}
	}
}