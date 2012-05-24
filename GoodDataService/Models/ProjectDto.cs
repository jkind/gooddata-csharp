using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoodDataService.Models
{
	public class ProjectsDto
	{
		public List<ProjectDto> Projects { get; set; }
	}

	public class ProjectDto
	{
		public Project Project { get; set; }
	}

	public class Project
	{
		[JsonIgnore]
		public string ProjectId
		{
			get { return Links.Metadata.ExtractId(Constants.MD_URI); }
		}

		public ProjectContent Content { get; set; }
		public Links Links { get; set; }
		public Meta Meta { get; set; }
	}

	public class ProjectContent
	{
		public int GuidedNavigation { get; set; }
		public string IsPublic { get; set; }
		public string State { get; set; }
	}

	public class Meta
	{
		public string Title { get; set; }
		public string Summary { get; set; }
		public string ProjectTemplate { get; set; }
		public string Driver { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
		public string Author { get; set; }
		public string Contributor { get; set; }
	}

	public class Links
	{
		public string Roles { get; set; }
		public string Ldm_thumbnail { get; set; }
		public string UserPermissions { get; set; }
		public string UserRoles { get; set; }
		public string Connectors { get; set; }
		public string Self { get; set; }
		public string Invitations { get; set; }
		public string Users { get; set; }
		public string Projects { get; set; }
		public string Ldm { get; set; }
		public string Metadata { get; set; }
		public string Publicartifacts { get; set; }
		public string Templates { get; set; }
		public string Permissions { get; set; }
	}
}