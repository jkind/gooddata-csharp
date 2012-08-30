﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace GoodDataService.SSO
{
	internal sealed class GpgPath
	{
		private const string ExeFolderPathKey = "GoodDataService.SSO.GpgPath.ExeFolder";
		private const string HomeFolderKey = "GoodDataService.SSO.GpgPath.HomeFolder";

		private static readonly IFolderLocations[] PossibleExeFolders = new IFolderLocations[]
			                                                                {
				                                                                new ConfigValue(),
				                                                                new PathEnvironmentVariable(),
				                                                                new RegistryValue(RegistryHive.LocalMachine,
				                                                                                  RegistryView.Default,
				                                                                                  @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\GnuPG",
				                                                                                  "InstallLocation"),
				                                                                new RegistryValue(RegistryHive.LocalMachine,
				                                                                                  RegistryView.Default,
				                                                                                  @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\GPG4Win",
				                                                                                  "InstallLocation"),
				                                                                new RegistryValue(RegistryHive.LocalMachine,
				                                                                                  RegistryView.Default,
				                                                                                  @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\GPG4Win",
				                                                                                  "InstallLocation"),
				                                                                new ExplicitPath(@"C:\Program Files\GNU\GnuPG"),
				                                                                new ExplicitPath(@"C:\Program Files (x86)\GNU\GnuPG")
				                                                                ,
			                                                                };

		private static string PathOrDefault(string fileName)
		{
			return PossibleExeFolders
				.SelectMany(p => p.ResolveFolders())
				.Where(folder => folder != null)
				.Select(folder => Path.Combine(folder, fileName))
				.FirstOrDefault(File.Exists);
		}

		public string GetExeFullPath()
		{
			string path = PathOrDefault("gpg.exe") ?? PathOrDefault("gpg2.exe");
			if (default(string) == path)
			{
				string searchedLocations = string.Join(Environment.NewLine,
				                                       PossibleExeFolders.SelectMany(p => p.ResolveFolders()).Where(
					                                       s => !string.IsNullOrWhiteSpace(s)).Distinct());
				throw new FileNotFoundException(
					string.Format("gpg.exe/gpg2.exe could not be found at the following locations. Make sure it is installed. {0}{1}",
					              Environment.NewLine, searchedLocations));
			}
			return Path.GetFullPath(path);
		}

		public string GetHomeFolderPath()
		{
			string[] values = ConfigurationManager.AppSettings.GetValues(HomeFolderKey);
			if (null == values || string.IsNullOrWhiteSpace(values[0]))
				return @"C:\gnupg";

			return values[0];
		}

		#region Nested type: ConfigValue

		private class ConfigValue : IFolderLocations
		{
			#region IFolderLocations Members

			public IEnumerable<string> ResolveFolders()
			{
				return ConfigurationManager.AppSettings.GetValues(ExeFolderPathKey) ?? new string[0];
			}

			#endregion
		}

		#endregion

		#region Nested type: ExplicitPath

		private class ExplicitPath : IFolderLocations
		{
			private readonly string _folderPath;

			public ExplicitPath(string folderPath)
			{
				_folderPath = folderPath;
			}

			#region IFolderLocations Members

			public IEnumerable<string> ResolveFolders()
			{
				yield return _folderPath;
			}

			#endregion
		}

		#endregion

		#region Nested type: IFolderLocations

		private interface IFolderLocations
		{
			IEnumerable<string> ResolveFolders();
		}

		#endregion

		#region Nested type: PathEnvironmentVariable

		private class PathEnvironmentVariable : IFolderLocations
		{
			#region IFolderLocations Members

			public IEnumerable<string> ResolveFolders()
			{
				try
				{
					return Environment.GetEnvironmentVariable("PATH")
						.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
				}
				catch (Exception)
				{
					return new string[0];
				}
			}

			#endregion
		}

		#endregion

		#region Nested type: RegistryValue

		private class RegistryValue : IFolderLocations
		{
			private readonly RegistryHive _hive;
			private readonly string _path;
			private readonly string _value;
			private readonly RegistryView _view;

			public RegistryValue(RegistryHive hive, RegistryView view, string path, string value)
			{
				_value = value;
				_hive = hive;
				_view = view;
				_path = path;
			}

			#region IFolderLocations Members

			public IEnumerable<string> ResolveFolders()
			{
				using (RegistryKey baseKey = RegistryKey.OpenBaseKey(_hive, _view))
				{
					RegistryKey subKey = null;
					try
					{
						subKey = baseKey.OpenSubKey(_path);
						return new[] {(string) subKey.GetValue(_value, string.Empty)};
					}
					catch (Exception)
					{
						return new string[0];
					}
					finally
					{
						if (subKey != null)
							subKey.Dispose();
					}
				}
			}

			#endregion
		}

		#endregion
	}
}