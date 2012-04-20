using System;
using System.Collections.Generic;
using System.Linq;
using GoodDataService;
using GoodDataService.Models;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class UserTests
	{
		private readonly ApiWrapper reportingService;
		private readonly string profileId;

		public UserTests()
		{
			reportingService = new ApiWrapper();
			profileId = reportingService.Authenticate(reportingService.Config.Login, reportingService.Config.Password);
			reportingService.GetToken();
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
			var projectId = reportingService.CreateProject(profileId, "ProjectUserTest", "Project User Test Summary");
			var domainUsers = reportingService.GetDomainUsers();
			var max = Math.Min(domainUsers.Count, 2);
			for (int i = 0; i < max; i++)
			{
				reportingService.AddUsertoProject(projectId, domainUsers[i].AccountSetting.ProfileId);
			}
			var users = reportingService.GetProjectUsers(projectId);
			if (users==null)
				
			Assert.IsNotInstanceOf<List<AccountSetting>>(typeof (List<AccountSetting>));
			Assert.IsNotEmpty(users);

			reportingService.DeleteProject(projectId);
		}

		[Test]
		public void CreateUser_Integration_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var login = string.Format("tester+{0}@{1}.com",title,reportingService.Config.Domain);
			var password = "password";
			var firstName = "firstname" + title;
			var lastName = "lastName" + title;
			var newProfileId = reportingService.CreateUser(login, password, password, firstName, lastName);
			Assert.IsNotNullOrEmpty(newProfileId);

			var projectTitle = "CreateUserTest";
			var projectId = reportingService.CreateProject(profileId, projectTitle, "Create User Test Summary");
			reportingService.AddUsertoProject(projectId, newProfileId, ApiWrapper.Roles.Admin);

			CheckUserIsInProject(login);

			reportingService.DeleteUser(newProfileId);

			var user = reportingService.FindDomainUsersByLogin(login);
			Assert.IsNull(user);

			reportingService.DeleteProject(projectId);

			var project = reportingService.FindProjectByTitle(profileId,projectTitle);
			Assert.IsNull(project);
		}

		[Test]
		public void AddUserToProjectUserIsInProject(string email)
		{
			var projects = reportingService.GetProjects(profileId);
			var projectId = projects.FirstOrDefault().Project.ProjectId;
			var user = reportingService.FindProjectUsersByEmail(projectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == email);
		}


		[Test]
		[Ignore]
		public void CheckUserIsInProject(string email)
		{
			var projects = reportingService.GetProjects(profileId);
			var projectId = projects.FirstOrDefault().Project.ProjectId;
			var user = reportingService.FindProjectUsersByEmail(projectId, email);
			Assert.NotNull(user);
			Assert.IsTrue(user.Content.Status == "ENABLED");
			Assert.IsTrue(user.Content.Email == email);
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
		public void CreateUser()
		{
			//var login = string.Format("ssotester@{0}.com", reportingService.Config.Domain);
			//var password = "password";
			//var firstName = "sso";
			//var lastName = "admin";

			var login = "jkind+sso@groupcommerce.com";
			var password = "password";
			var firstName = "jonathan";
			var lastName = "kind";

			var newProfileId = reportingService.CreateUser(login, password, password, firstName, lastName);
			Assert.IsNotNullOrEmpty(newProfileId);

			var projects = reportingService.GetProjects(profileId);
			var projectId = projects.FirstOrDefault().Project.ProjectId;
			reportingService.AddUsertoProject(projectId, newProfileId, ApiWrapper.Roles.Editor);

			CheckUserIsInProject(login);
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
		[Ignore]
		public void AddUsertoProject()
		{
			var project = reportingService.FindProjectByTitle(profileId,"GroupCommerce");
			//var email = string.Format("ssotester@{0}.com", reportingService.Config.Domain);
			var email = "jkind+sso@groupcommerce.com";
			var domainUser = reportingService.FindDomainUsersByLogin(email);
			var projectUser = reportingService.FindProjectUsersByEmail(project.ProjectId,email);
			reportingService.AddUsertoProject(project.ProjectId, domainUser.ProfileId, ApiWrapper.Roles.Editor);
		}
	}
}