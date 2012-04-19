using System;

namespace GoodDataService.Models
{
	public class GoodDataApiException : Exception
	{
		public GoodDataApiException(string message)
			: base(message)
		{
		}
	}
}