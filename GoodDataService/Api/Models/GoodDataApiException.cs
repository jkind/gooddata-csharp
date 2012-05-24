using System;

namespace GoodDataService.Api.Models
{
	[Serializable]
	public class GoodDataApiException : Exception
	{
		public GoodDataApiException(string message)
			: base(message)
		{
		}
	}
}