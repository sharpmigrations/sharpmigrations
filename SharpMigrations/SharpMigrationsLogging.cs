using Microsoft.Extensions.Logging;
using SharpData.Log;

namespace SharpMigrations
{
    public static class SharpMigrationsLogging {
        public static ILoggerFactory LoggerFactory {
            get => SharpDataLogging.LoggerFactory;
            set => SharpDataLogging.LoggerFactory = value;
        }
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        public static ILogger CreateLogger(string category) => LoggerFactory.CreateLogger(category);
    }
}
