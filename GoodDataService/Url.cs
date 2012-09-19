using System.Text;
using Newtonsoft.Json;

namespace GoodDataService
{
	internal static class Url
	{
		public static string Combine(params string[] urlParts)
		{
			if (urlParts == null)
				return null;

			var sb = new StringBuilder();
			foreach (var part in urlParts)
			{
				if (string.IsNullOrEmpty(part))
					continue;
				var trimmedPart = part.TrimEnd('/');
				if (!string.IsNullOrEmpty(trimmedPart))
				{
					if (sb.Length > 0)
					{
						sb.Append("/");
						trimmedPart = trimmedPart.TrimStart('/');
					}

					sb.Append(trimmedPart);
				}
			}
			return sb.ToString();
		}
	}
}