using System;
using System.Linq;
using GoodDataService;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class RoleTests : BaseTest
	{

		[Test]
		public void FindRoleByTitle()
		{
			var projectId = GetTestProject().ProjectId;
			var role = ReportingService.FindRoleByTitle(projectId, SystemRoles.DashboardOnly);
			Assert.NotNull(role);

			role = ReportingService.FindRoleByTitle(projectId, SystemRoles.Editor);
			Assert.NotNull(role);

			role = ReportingService.FindRoleByTitle(projectId, SystemRoles.Admin);
			Assert.NotNull(role);

			role = ReportingService.FindRoleByTitle(projectId, SystemRoles.Viewer);
			Assert.NotNull(role);
		}


		[Test]
		public void ChangeProjectUserRole()
		{
			var email = string.Format("tester+{0}@{1}.com", DateTime.Now.Ticks, ReportingService.Config.Domain);
			var profileId = CreateTestUser(email);
			var projectId = GetTestProject().ProjectId;
			ReportingService.AddUsertoProject(projectId, profileId);
			ReportingService.UpdateProjectUserAccess(projectId, profileId, true,SystemRoles.Editor);
			var user = ReportingService.FindProjectUsersByEmail(projectId,email);
			var editorRole = ReportingService.FindRoleByTitle(projectId, SystemRoles.Editor);
			if (user.Content.UserRoles.Any(role => role.Equals(editorRole.ProjectRole.Meta.Uri, StringComparison.OrdinalIgnoreCase)))
				Assert.Pass();

			ReportingService.DeleteUser(profileId);

		}
		
	}
}