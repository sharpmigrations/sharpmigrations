using Sharp.Data.Log;

namespace Sharp.Integration.NLog {
    public class NLogLoggerFactory : ILoggerFactory {
        public ILogger GetLogger(string name) {
            return new NLogLogger(name);
        }
    }
}