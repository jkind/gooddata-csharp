using System;

namespace GoodDataService
{
	public static class Extensions
	{
		public static string ExtractId(this string value, string replacePath)
		{
			return value.Replace(replacePath, string.Empty).Replace("/", string.Empty);
		}

		public static string ExtractObjectId(this string value)
		{
			var startIndex = value.LastIndexOf('/') + 1;
			return value.Substring(startIndex, value.Length - startIndex);
		}

		/// <summary>
		/// 	Use Universal time.
		/// </summary>
		/// <param name = "dateTime"></param>
		/// <returns></returns>
		public static double ToUnixTime(this DateTime dateTime)
		{
			return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
		}
	}
}