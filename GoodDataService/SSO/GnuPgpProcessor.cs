using System;
using System.IO;
using System.Text;
using Starksoft.Cryptography.OpenPGP;

namespace GoodDataService.SSO
{
	/// <summary>
	/// </summary>
	/// <see cref = "http://sourceforge.net/projects/starksoftopenpg/" />
	public class GnuPgpProcessor
	{
		private readonly GnuPG gpg;

		public GnuPgpProcessor()
		{
			gpg = new GnuPG();
			gpg.HomePath = @"C:\gnupg";
		}

		/// <summary>
		/// 	Will Encrypt any OpenPGP data
		/// </summary>
		/// <param name = "recipient"></param>
		/// <param name = "unencryptedText"></param>
		/// <returns></returns>
		public string Encrypt(string recipient, string unencryptedText)
		{
			// if no recipient is specified then don't do the encryption
			if (string.IsNullOrWhiteSpace(recipient))
				throw new ArgumentNullException();

			// create two memory stream - one to hold the unencrypted data and the other stream holds the encrypted data.
			using (var unencrypted = new MemoryStream(Encoding.ASCII.GetBytes(unencryptedText)))
			using (var encrypted = new MemoryStream())
			{
				// create a new GnuPG object
				gpg.OutputType = OutputTypes.AsciiArmor;
				gpg.Recipient = recipient;
				gpg.Encrypt(unencrypted, encrypted);
				using (var reader = new StreamReader(encrypted))
				{
					encrypted.Position = 0;
					return reader.ReadToEnd();
				}
			}
		}

		public string Decrypt(string passphrase, string encryptedText)
		{
			if (string.IsNullOrWhiteSpace(passphrase))
				throw new ArgumentNullException();

			using (var unencrypted = new MemoryStream())
			using (var encrypted = new MemoryStream(Encoding.ASCII.GetBytes(encryptedText)))
			{
				gpg.Passphrase = passphrase;
				gpg.OutputType = OutputTypes.AsciiArmor;
				gpg.Decrypt(encrypted, unencrypted);

				using (var reader = new StreamReader(unencrypted))
				{
					unencrypted.Position = 0;
					return reader.ReadToEnd();
				}
			}
		}

		public string Sign(string passphrase, string unencryptedText)
		{
			if (string.IsNullOrWhiteSpace(passphrase) || string.IsNullOrWhiteSpace(unencryptedText))
				throw new ArgumentNullException();

			using (var unsigned = new MemoryStream(Encoding.ASCII.GetBytes(unencryptedText)))
			using (var signed = new MemoryStream())
			{
				// create a new GnuPG object
				gpg.OutputType = OutputTypes.AsciiArmor;
				gpg.Passphrase = passphrase;
				gpg.Sign(unsigned, signed);

				using (var reader = new StreamReader(signed))
				{
					signed.Position = 0;
					return reader.ReadToEnd();
				}
			}
		}

		public void Verify(string encryptedText)
		{
			if (string.IsNullOrWhiteSpace(encryptedText))
				throw new ArgumentNullException();

			using (var signed = new MemoryStream(Convert.FromBase64String(encryptedText)))
			{
				gpg.Verify(signed);
			}
		}
	}
}