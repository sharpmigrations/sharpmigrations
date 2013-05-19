using System;

namespace Sharp.Data.Log {
    public interface ISharpLoggerFactory {
        ISharpLogger LoggerFor(string keyName);
        ISharpLogger LoggerFor(Type type);
    }
}