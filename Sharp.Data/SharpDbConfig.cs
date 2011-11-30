using System;

namespace Sharp.Data {
    public class SharpDbConfig {
        public string DbProviderName { get; set; }
        public IDataProvider DataProvider { get; set; }
    	public Database Database { get; set; }
    	public DataClient DataClient { get; set; }
    }
}