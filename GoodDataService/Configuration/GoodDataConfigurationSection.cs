using System.Configuration;

namespace GoodDataService.Configuration
{
	public class GoodDataConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("passphrase", IsRequired = true)]
		public virtual string Passphrase
		{
			get { return (string) this["passphrase"]; }
			set { this["passphrase"] = value; }
		}

		[ConfigurationProperty("recipient", IsRequired = true)]
		public virtual string Recipient
		{
			get { return (string) this["recipient"]; }
			set { this["recipient"] = value; }
		}

		[ConfigurationProperty("url", IsRequired = true)]
		public virtual string Url
		{
			get { return (string) this["url"]; }
			set { this["url"] = value; }
		}

		[ConfigurationProperty("domain", IsRequired = true)]
		public virtual string Domain
		{
			get { return (string) this["domain"]; }
			set { this["domain"] = value; }
		}

		[ConfigurationProperty("ignoreSslErrors", IsRequired = true)]
		public virtual string IgnoreSslErrors
		{
			get { return (string) this["ignoreSslErrors"]; }
			set { this["ignoreSslErrors"] = value; }
		}

		[ConfigurationProperty("login", IsRequired = true)]
		public virtual string Login
		{
			get { return (string) this["login"]; }
			set { this["login"] = value; }
		}

		[ConfigurationProperty("password", IsRequired = true)]
		public virtual string Password
		{
			get { return (string) this["password"]; }
			set { this["password"] = value; }
		}

		public static GoodDataConfigurationSection GetConfig()
		{
			return ConfigurationManager.GetSection("gooddata") as GoodDataConfigurationSection;
		}
	}
}