using System;

namespace GoodDataService.Models
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