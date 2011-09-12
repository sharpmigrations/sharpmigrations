using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sharp.Data.Config {
    public class DataConfigurationHandler : ConfigurationSection {

        public static readonly string DefaultSectionName = "sharp.data";
        
        [ConfigurationProperty("defaultSettings", IsRequired = true)]
        public DataSettings DefaultSettings {
            get {
                return (DataSettings)this["defaultSettings"];
            }
        }
    }
}
