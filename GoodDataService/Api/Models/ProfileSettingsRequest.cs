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
}