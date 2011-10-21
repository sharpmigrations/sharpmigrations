namespace Sharp.Data {
    public class SharpDbConfig {
        public string DbProviderName { get; set; }
        public IDataProvider DataProvider { get; set; }
        public Dialect Dialect { get; set; }
    }
}