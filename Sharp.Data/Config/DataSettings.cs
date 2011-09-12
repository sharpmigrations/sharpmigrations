using System.Configuration;

namespace Sharp.Data.Config {
    public class DataSettings : ConfigurationElement {
        [ConfigurationProperty("DefaultConnectionStringName", IsRequired = true)]
        public string DefaultConnectionString {
            get { return (string) this["DefaultConnectionStringName"]; }
            set { this["DefaultConnectionStringName"] = value; }
        }
    }
}