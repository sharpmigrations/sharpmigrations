using System;
using System.Configuration;

namespace Sharp.Data.Config {
	public static class ConfigFromAppConfig {
		private static DataConfigurationHandler _config;

		public static void LoadConfig() {
			_config = (DataConfigurationHandler) ConfigurationManager.GetSection(DataConfigurationHandler.DefaultSectionName);
			if (!String.IsNullOrEmpty(_config.DefaultSettings.DefaultConnectionString)) {
			    ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[_config.DefaultSettings.DefaultConnectionString];
			    DefaultConfig.ConnectionString = settings.ConnectionString;
			    DefaultConfig.DatabaseProvider = settings.ProviderName;
			}
		}
	}
}