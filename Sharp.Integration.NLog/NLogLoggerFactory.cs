using Sharp.Data.Log;

namespace Sharp.Integration.NLog {
    public class NLogLoggerFactory : ISharpLoggerFactory {
        public ISharpLogger GetLogger(string name) {
            return new NLogLogger(name);
        }
    }
}