using System;
using System.Collections.Generic;
using GoodDataService;
using GoodDataService.Models;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class UserTests
	{
		private readonly ApiWrapper reportingService;

		public UserTests()
		{
			reportingService = new ApiWrapper();
		}

		[Test]
		[Ignore]
		public void AddUserToProjectUserIsInProject(string email)
		{
			var project = reportingService.FindProjectByTitle(reportingService.Config.Domain);
			var user = reportingService.FindProjectUsersByEmail(project.ProjectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == email);
		}

		[Test]
		[Ignore]
		public void AddUsertoProject()
		{
			var projectName = reportingService.Config.Domain;
			var project = reportingService.FindProjectByTitle(projectName);
			var email = string.Format("gooddata@{0}.com", projectName);
			var domainUser = reportingService.FindDomainUsersByLogin(email);
			var projectUser = reportingService.FindProjectUsersByEmail(project.ProjectId, email);
			reportingService.AddUsertoProject(project.ProjectId, domainUser.ProfileId, Roles.Editor);
		}

		[Test]
		[Ignore]
		public void CheckUserIsInDomain(string email)
		{
			var user = reportingService.FindDomainUsersByLogin(email);
			Assert.IsTrue(user.Login == email);
		}

		[Test]
		[Ignore]
		public void CheckUserIsInProject(string email)
		{
			var project = reportingService.FindProjectByTitle(reportingService.Config.Domain);
			var user = reportingService.FindProjectUsersByEmail(project.ProjectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == email);
		}

		[Test]
		[Ignore]
		public void CreateUser()
		{
			var login = string.Format("ssotester@{0}.com", reportingService.Config.Domain);
			var password = "password";
			var firstName = "sso";
			var lastName = "admin";

			var newProfileId = reportingService.CreateUser(login, password, password, firstName, lastName);
			Assert.IsNotNullOrEmpty(newProfileId);

			var project = reportingService.FindProjectByTitle(reportingService.Config.Domain);
			reportingService.AddUsertoProject(project.ProjectId, newProfileId, Roles.DashboardOnly);

			CheckUserIsInProject(login);
		}

		[Test]
		public void CreateUser_Integration_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var login = string.Format("tester+{0}@{1}.com", title, reportingService.Config.Domain);
			var password = "password";
			var firstName = "firstname" + title;
			var lastName = "lastName" + title;
			var newProfileId = reportingService.CreateUser(login, password, password, firstName, lastName);
			Assert.IsNotNullOrEmpty(newProfileId);

			var projectTitle = "CreateUserTest";
			var projectId = reportingService.CreateProject(projectTitle, "Create User Test Summary");
			reportingService.AddUsertoProject(projectId, newProfileId, Roles.Admin);

			var user = reportingService.FindProjectUsersByEmail(projectId, login);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == login);

			reportingService.DeleteUser(newProfileId);

			var domainUser = reportingService.FindDomainUsersByLogin(login);
			Assert.IsNull(domainUser);

			reportingService.DeleteProject(projectId);

			var project = reportingService.FindProjectByTitle(projectTitle);
			Assert.IsNull(project);
		}

		[Test]
		[Ignore]
		public void DeleteUser(string email)
		{
			email = email ?? string.Format("ssotester@{0}.com", reportingService.Config.Domain);
			var user = reportingService.FindDomainUsersByLogin(email);
			reportingService.DeleteUser(user.ProfileId);
			user = reportingService.FindDomainUsersByLogin(email);
			Assert.IsNull(user);
		}

		[Test]
		public void GetDomainUsers_ExpectSucces()
		{
			var users = reportingService.GetDomainUsers();
			Assert.IsNotInstanceOf<List<AccountSetting>>(typeof (List<AccountSetting>));
			Assert.IsNotEmpty(users);
		}

		[Test]
		public void GetProjectUsers_ExpectSucces()
		{
			var projectId = reportingService.CreateProject("ProjectUserTest", "Project User Test Summary");
			var domainUsers = reportingService.GetDomainUsers();
			var max = Math.Min(domainUsers.Count, 2);
			for (int i = 0; i < max; i++)
			{
				reportingService.AddUsertoProject(projectId, domainUsers[i].AccountSetting.ProfileId);
			}
			var users = reportingService.GetProjectUsers(projectId);
			if (users == null)

				Assert.IsNotInstanceOf<List<AccountSetting>>(typeof (List<AccountSetting>));
			Assert.IsNotEmpty(users);

			reportingService.DeleteProject(projectId);
		}

		[Test]
		[Ignore]
		public void UpdateProjectUserStatus_SetDisabled_ExpectDsiabled()
		{
			var email = reportingService.Config.Login;
			var project = reportingService.FindProjectByTitle(reportingService.Config.Domain);
			var user = reportingService.FindProjectUsersByEmail(project.ProjectId, email);
			reportingService.UpdateProjectUserStatus(project.ProjectId, user.ProfileId, false);
			user = reportingService.FindProjectUsersByEmail(project.ProjectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "DISABLED");
			Assert.IsTrue(user.Content.Email == email);
			reportingService.UpdateProjectUserStatus(project.ProjectId, user.ProfileId, true);
		}
	}
}