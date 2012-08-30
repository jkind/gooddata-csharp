using System;
using System.IO;
using GoodDataService.SSO;
using NUnit.Framework;

namespace GoodDataTests.SSO
{
	[TestFixture]
	public class GpgPathTests
	{
		[Test, Explicit]
		public void GpgIsInstalled()
		{
			string exePath = new GpgPath().GetExeFullPath();
			Console.Out.WriteLine(exePath);
			Assert.IsTrue(File.Exists(exePath));
		}
	}
}