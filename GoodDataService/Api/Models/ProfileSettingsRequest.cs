namespace GoodDataService.Api.Models
{
	public class ProfileSettingsRequest
	{
		public static ProfileSettingsRequest CreateUSFormat()
		{
			return new ProfileSettingsRequest
			       	{
			       		ProfileSetting = new ProfileSetting
			       		                 	{
			       		                 		Separators = new Separators
			       		                 		             	{
			       		                 		             		Decimal = ".",
			       		                 		             		Thousand = ","
			       		                 		             	}
			       		                 	}
			       	};
		}

		public ProfileSetting ProfileSetting { get; set; }
	}

	public class ProfileSetting
	{
		public Separators Separators { get; set; }

	}
	public class Separators
	{
		public string Thousand { get; set; }
		public string Decimal { get; set; }
	}
		
}