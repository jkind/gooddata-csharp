using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoodDataService.Models
{
	public class UserResponse
	{
		public List<UserWrapper> Users { get; set; }
	}

	public class UserWrapper
	{
		public User User { get; set; }
	}

	public class User
	{
		[JsonIgnore]
		public string ProfileId
		{
			get { return Links.Self.ExtractId(Constants.PROFILE_URI); }
		}

		public UserContent Content { get; set; }
		public UserLinks Links { get; set; }
		public UserMeta Meta { get; set; }
	}

	public class UserContent
	{
		public string Email { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Phonenumber { get; set; }
		public string Status { get; set; }
		public string[] UserRoles { get; set; }
	}

	public class UserLinks
	{
		public string Roles { get; set; }
		public string Self { get; set; }
		public string Invitations { get; set; }
		public string Projects { get; set; }
		public string Permissions { get; set; }
	}

	public class UserMeta
	{
		public string Title { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
		public string Author { get; set; }
		public string Contributor { get; set; }
	}
}