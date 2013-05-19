using System;

namespace Sharp.Data.Log {
    public class NoLoggingLoggerFactory : ISharpLoggerFactory {
        private static readonly ISharpLogger Nologging = new NoLoggingInternalLogger();
        public ISharpLogger LoggerFor(string keyName) {
            return Nologging;
        }

        public ISharpLogger LoggerFor(Type type) {
            return Nologging;
        }
    }
}