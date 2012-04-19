using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoodDataService.Models
{
	public class CreateDomainUser
	{
		public AccountSetting AccountSetting { get; set; }
	}

	public class CreateDomainUserResponse
	{
		public AccountSettings AccountSettings { get; set; }
	}

	public class AccountSettings
	{
		public Paging Paging { get; set; }
		public List<AccountResponseSettingWrapper> Items { get; set; }
	}

	public class Paging
	{
		public string Offset { get; set; }
		public string Count { get; set; }
	}

	public class AccountSetting
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public string VerifyPassword { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CompanyName { get; set; }
		public string Country { get; set; }
		public string PhoneNumber { get; set; }
		public string Position { get; set; }
		public string Timezone { get; set; }
		public AccountSettingLinks Links { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
	}

	public class AccountResponseSettingWrapper
	{
		public AccountResponseSetting AccountSetting { get; set; }
	}

	public class AccountResponseSetting
	{
		[JsonIgnore]
		public string ProfileId
		{
			get { return Links.Self.ExtractId(ApiWrapper.PROFILE_URI); }
		}

		public string Login { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CompanyName { get; set; }
		public string Country { get; set; }
		public string PhoneNumber { get; set; }
		public string Position { get; set; }
		public string Timezone { get; set; }
		public AccountSettingLinks Links { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
	}


	public class AccountSettingLinks
	{
		public string Projects { get; set; }
		public string Self { get; set; }
	}
}