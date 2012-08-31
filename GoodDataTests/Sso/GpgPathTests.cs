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
			string folder = new GpgPath().GetExeFolder();
			string fullPath = Path.Combine(folder, "gpg.exe");
			Console.Out.WriteLine(folder);
			Assert.IsTrue(File.Exists(fullPath));
		}
	}
}