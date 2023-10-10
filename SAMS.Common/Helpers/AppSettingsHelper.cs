using Microsoft.Extensions.Configuration;
using SAMS.Common.Extensions;
using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;
using System.IO;

namespace SAMS.Common.Helpers
{
	public class AppSettingsHelper
	{
		private static AppSettingsHelper _appSettings;
		public string ConnectionName { get; set; }
		public string AppConnection { get; set; }
		public SmtpSettings SmtpSettings { get; set; }
		public string FileUploadPathName { get; set; }
		public string CurrentSiteUrl { get; set; }
		public string CurrentApiUrl { get; set; }
		public string CurrentSiteDefaultUrl { get; set; }
		public bool JobsActivityStatus { get; set; }
		public List<string> CorsUrls { get; set; }
		public bool IsLocal { get; set; }

		public AppSettingsHelper(IConfiguration config)
		{
			ConnectionName = config.GetValue<string>("ConnectionName");
			AppConnection = config.GetValue<string>("ConnectionStrings:" + ConnectionName);
			FileUploadPathName = config.GetValue<string>("FileUploadPathName");
			JobsActivityStatus = config.GetValue<bool>("JobsActivityStatus");
			CurrentSiteUrl = config.GetValue<string>("CurrentSiteUrl");
			CurrentApiUrl = config.GetValue<string>("CurrentApiUrl");

			IsLocal = config.GetValue<bool>("IsLocal");
			CorsUrls = new List<string>();
			config.GetSection("CorsUrls").Bind(CorsUrls);

			SmtpSettings = config.GetSection("Smtp").Get<SmtpSettings>();

			_appSettings = this;
		}

		public static AppSettingsHelper Current
		{
			get
			{
				if (_appSettings == null)
				{
					_appSettings = GetCurrentSettings();
				}

				return _appSettings;
			}
		}

		public static AppSettingsHelper GetCurrentSettings()
		{
			var builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
							.AddEnvironmentVariables();

			return new AppSettingsHelper(builder.Build());
		}
	}

}
