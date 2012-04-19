namespace GoodDataService.Models
{
	public class AuthenticationRequest
	{
		public PostUserLogin PostUserLogin { get; set; }
	}

	public class PostUserLogin
	{
		public string Login { get; set; }
		public string Password { get; set; }

		public string Remember
		{
			get { return "0"; }
		}
	}
}