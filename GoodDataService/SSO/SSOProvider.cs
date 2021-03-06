﻿using System;
using System.Diagnostics;
using System.Web;
using GoodDataService.Configuration;

namespace GoodDataService.SSO
{
	public class SsoProvider
	{
		private System.Object lockThis = new System.Object();
		public SsoProvider()
		{
			Config = GoodDataConfigurationSection.GetConfig();
		}

		public GoodDataConfigurationSection Config { get; set; }

		public string GenerateToken(string email, int validaityOffsetInMinutes = 10)
		{

			var userData = CreateUserData(email);
			Trace.Write(userData);
			
	        lock (lockThis)
	        {
	        	var gpg = new GnuPgpProcessor();
	        	var signedData = gpg.Sign(Config.Passphrase, userData);
	        	Trace.Write(signedData);
	        	var encryptedData = gpg.Encrypt(Config.Recipient, signedData);
	        	Trace.Write(encryptedData);
	        	return EncodeUserData(encryptedData);
	        }
		}


		private static string CreateUserData(string email, int validaityOffsetInMinutes = 10)
		{
			return "{\"email\":\"" + email + "\",\"validity\":" +
			       Math.Round(DateTime.UtcNow.AddMinutes(validaityOffsetInMinutes).ToUnixTime()) + "}";
		}

		private static string EncodeUserData(string input)
		{
			return HttpUtility.UrlEncode(input);
		}
	}
}