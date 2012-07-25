using System;
using System.Collections.Generic;
using System.Linq;
using GoodDataService;
using GoodDataService.Api.Models;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class UserTests : BaseTest
	{

		[Test]
		[Ignore]
		public void AddUserToProjectUserIsInProject(string email)
		{
			var project = ReportingService.FindProjectByTitle(TestProjectName);
			var user = ReportingService.FindProjectUsersByEmail(project.ProjectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == email);
		}

		[Test]
		[Ignore]
		public void AddUsertoProject()
		{
			var project = ReportingService.FindProjectByTitle(TestProjectName);
			var email = string.Format("gooddata@{0}.com", ReportingService.Config.Domain);
			var domainUser = ReportingService.FindDomainUsersByLogin(email);
			var projectUser = ReportingService.FindProjectUsersByEmail(project.ProjectId, email);
			ReportingService.AddUsertoProject(project.ProjectId, domainUser.ProfileId, SystemRoles.Editor);
		}

		[Test]
		[Ignore]
		public void CheckUserIsInDomain(string email)
		{
			var user = ReportingService.FindDomainUsersByLogin(email);
			Assert.IsTrue(user.Login == email);
		}

		[Test]
		[Ignore]
		public void CheckUserIsInProject(string email)
		{
			var project = ReportingService.FindProjectByTitle(TestProjectName);
			var user = ReportingService.FindProjectUsersByEmail(project.ProjectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == email);
		}

		[Test]
		[Ignore]
		public void CreateUser()
		{
			var login = string.Format("ssotester@{0}.com", ReportingService.Config.Domain);
			var password = "password";
			var firstName = "sso";
			var lastName = "admin";

			var newProfileId = ReportingService.CreateUser(login, password, password, firstName, lastName);
			Assert.IsNotNullOrEmpty(newProfileId);

			var project = ReportingService.FindProjectByTitle(TestProjectName);
			ReportingService.AddUsertoProject(project.ProjectId, newProfileId);

			CheckUserIsInProject(login);
		}

		[Test]
		public void CreateUser_Integration_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var login = string.Format("tester+{0}@{1}.com", title, ReportingService.Config.Domain);
			var password = "password";
			var firstName = "firstname" + title;
			var lastName = "lastName" + title;
			var ssoProvider = ReportingService.Config.Domain + ".com";
			var newProfileId = ReportingService.CreateUser(login, password, password, firstName, lastName,ssoProvider.ToLower(),"CA");
			Assert.IsNotNullOrEmpty(newProfileId);

			var projectTitle = "CreateUserTest";
			var projectId = ReportingService.CreateProject(projectTitle, "Create User Test Summary");
			ReportingService.AddUsertoProject(projectId, newProfileId, SystemRoles.Admin);

			var user = ReportingService.FindProjectUsersByEmail(projectId, login);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == login);

			ReportingService.DeleteUser(newProfileId);

			var domainUser = ReportingService.FindDomainUsersByLogin(login);
			Assert.IsNull(domainUser);

			ReportingService.DeleteProject(projectId);

			var project = ReportingService.FindProjectByTitle(projectTitle);
			Assert.IsNull(project);
		}

		[Test]
		[Ignore]
		public void CreateUserFilter_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var login = string.Format("tester+{0}@{1}.com", title, ReportingService.Config.Domain);
			var password = "password";
			var firstName = "firstname" + title;
			var lastName = "lastName" + title;
			var ssoProvider = ReportingService.Config.Domain + ".com";
			var newProfileId = ReportingService.CreateUser(login, password, password, firstName, lastName, ssoProvider.ToLower(), "CA");
			Assert.IsNotNullOrEmpty(newProfileId);

			var projectId = GetTestProject().ProjectId;
			ReportingService.AddUsertoProject(projectId, newProfileId);

			var response = ReportingService.CreateUserFilter(projectId, "Segment", "Segment Id", "1");
			Assert.IsNotNullOrEmpty(response);
			var filter = ReportingService.GetObject(response);
			Assert.IsNotNull(filter);

			var assignResponse = ReportingService.AssignUserFilters(projectId, new List<string> {newProfileId},new List<string> {response});
			Assert.IsTrue(assignResponse.Successful.Any(item => item.Contains(newProfileId)));

			ReportingService.DeleteUser(newProfileId);
			var domainUser = ReportingService.FindDomainUsersByLogin(login);
			Assert.IsNull(domainUser);
		}

		[Test]
		public void CreateSSOUser_Integration_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var login = string.Format("tester+{0}@{1}.com", title, ReportingService.Config.Domain);
			var password = "password";
			var firstName = "firstname" + title;
			var lastName = "lastName" + title;
			var newProfileId = ReportingService.CreateUser(login, password, password, firstName, lastName, ReportingService.Config.Domain + ".com");
			Assert.IsNotNullOrEmpty(newProfileId);

			var projectTitle = "CreateUserTest";
			var projectId = ReportingService.CreateProject(projectTitle, "Create User Test Summary");
			ReportingService.AddUsertoProject(projectId, newProfileId, SystemRoles.Admin);

			var user = ReportingService.FindProjectUsersByEmail(projectId, login);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == login);

			ReportingService.DeleteUser(newProfileId);

			var domainUser = ReportingService.FindDomainUsersByLogin(login);
			Assert.IsNull(domainUser);

			ReportingService.DeleteProject(projectId);

			var project = ReportingService.FindProjectByTitle(projectTitle);
			Assert.IsNull(project);
		}

		[Test]
		[Ignore]
		public void DeleteUser(string email)
		{
			email = email ?? string.Format("ssotester@{0}.com", ReportingService.Config.Domain);
			var user = ReportingService.FindDomainUsersByLogin(email);
			ReportingService.DeleteUser(user.ProfileId);
			user = ReportingService.FindDomainUsersByLogin(email);
			Assert.IsNull(user);
		}

		[Test]
		public void GetDomainUsers_ExpectSucces()
		{
			var users = ReportingService.GetDomainUsers();
			Assert.IsNotInstanceOf<List<AccountSetting>>(typeof (List<AccountSetting>));
			Assert.IsNotEmpty(users);
		}

		[Test]
		public void GetProjectUsers_ExpectSucces()
		{
			var projectId = ReportingService.CreateProject("ProjectUserTest", "Project User Test Summary");
			var domainUsers = ReportingService.GetDomainUsers();
			var max = Math.Min(domainUsers.Count, 2);
			for (var i = 0; i < max; i++)
			{
				ReportingService.AddUsertoProject(projectId, domainUsers[i].AccountSetting.ProfileId);
			}
			var users = ReportingService.GetProjectUsers(projectId);
			if (users == null)

				Assert.IsNotInstanceOf<List<AccountSetting>>(typeof (List<AccountSetting>));
			Assert.IsNotEmpty(users);

			ReportingService.DeleteProject(projectId);
		}

		[Test]
		[Ignore]
		public void UpdateProjectUserStatus_SetDisabled_ExpectDisabled()
		{
			var email = ReportingService.Config.Login;
			var project = ReportingService.FindProjectByTitle(TestProjectName);
			var user = ReportingService.FindProjectUsersByEmail(project.ProjectId, email);
			ReportingService.UpdateProjectUserAccess(project.ProjectId, user.ProfileId, false);
			user = ReportingService.FindProjectUsersByEmail(project.ProjectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "DISABLED");
			Assert.IsTrue(user.Content.Email == email);
			ReportingService.UpdateProjectUserAccess(project.ProjectId, user.ProfileId, true);
		}
	}
}