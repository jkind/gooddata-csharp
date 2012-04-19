namespace GoodDataService.Models
{
	public class ErrorDto
	{
		public Error Error { get; set; }
	}

	public class Error
	{
		public string ErrorClass { get; set; }
		public string Trace { get; set; }
		public string Component { get; set; }
		public string ErrorId { get; set; }
		public string ErrorCode { get; set; }
		public string Message { get; set; }
		public string[] Parameters { get; set; }
	}
}