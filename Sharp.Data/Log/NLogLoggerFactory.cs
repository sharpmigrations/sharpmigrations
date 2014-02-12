using System;

namespace Sharp.Data.Log {
    public class NLogLoggerFactory : ISharpLoggerFactory {
        private static readonly Type LogManagerType = System.Type.GetType("NLog.LogManager, NLog");
        private static readonly Func<string, object> GetLoggerByNameDelegate;

        static NLogLoggerFactory() {
            var method = LogManagerType.GetMethod("GetLogger", new[] { typeof(string) });
            GetLoggerByNameDelegate = name => method.Invoke(null, new [] { name });
        }

        public ISharpLogger LoggerFor(string keyName) {
            return new NLogLogger(GetLoggerByNameDelegate(keyName));
        }

        public ISharpLogger LoggerFor(Type type) {
            return new NLogLogger(GetLoggerByNameDelegate(type.FullName));
        }
    }
}