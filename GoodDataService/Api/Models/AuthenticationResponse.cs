namespace GoodDataService.Api.Models
{
	public class AuthenticationResponse
	{
		public UserLogin UserLogin { get; set; }
	}

	public class UserLogin
	{
		public string Profile { get; set; }
		public string State { get; set; }
	}
}