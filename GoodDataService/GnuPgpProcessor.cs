using System;
using System.IO;
using System.Text;
using Starksoft.Cryptography.OpenPGP;

namespace GoodDataService
{
	/// <summary>
	/// </summary>
	/// <see cref = "http://sourceforge.net/projects/starksoftopenpg/" />
	public class GnuPgpProcessor
	{
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
				var gpg = new GnuPG();
				gpg.OutputType = OutputTypes.AsciiArmor;
				gpg.Recipient = recipient;

				try
				{
					gpg.Encrypt(unencrypted, encrypted);
					using (var reader = new StreamReader(encrypted))
					{
						encrypted.Position = 0;
						return reader.ReadToEnd();
					}
				}
				catch (Exception ex)
				{
					throw;
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
				var gpg = new GnuPG();
				gpg.Passphrase = passphrase;
				gpg.OutputType = OutputTypes.AsciiArmor;

				try
				{
					gpg.Decrypt(encrypted, unencrypted);

					using (var reader = new StreamReader(unencrypted))
					{
						unencrypted.Position = 0;
						return reader.ReadToEnd();
					}
				}
				catch (GnuPGBadPassphraseException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw;
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
				var gpg = new GnuPG();
				gpg.OutputType = OutputTypes.AsciiArmor;
				gpg.Passphrase = passphrase;
				try
				{
					gpg.Sign(unsigned, signed);

					using (var reader = new StreamReader(signed))
					{
						signed.Position = 0;
						return reader.ReadToEnd();
					}
				}
				catch (GnuPGBadPassphraseException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		public void Verify(string encryptedText)
		{
			if (string.IsNullOrWhiteSpace(encryptedText))
				throw new ArgumentNullException();

			// create two memory stream - one to hold the unencrypted data and the other 
			// stream holds the encrypted data.  We can use any System.IO stream such as 
			// FileStream but for this demo we will use a memory stream
			using (var signed = new MemoryStream(Convert.FromBase64String(encryptedText)))
			{
				// create a new GnuPG object
				var gpg = new GnuPG();

				try
				{
					// sign the data using IO Streams - any type of input and output IO Stream can be used
					// as long as the source (input) stream can be read and the destination (output) stream 
					// can be written to
					gpg.Verify(signed);
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
	}
}