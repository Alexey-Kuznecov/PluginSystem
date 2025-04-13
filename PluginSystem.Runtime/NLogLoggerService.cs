
namespace PluginSystem.Runtime
{
    using NLog;
    using PluginSystem.Core;

    public class NLogLoggerService : ILoggerService
    {
        private readonly Logger _logger;

        public NLogLoggerService(string loggerName = "PluginLibrary")
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public void Trace(string message) => _logger.Trace(message);
        public void Debug(string message) => _logger.Debug(message);
        public void Info(string message) => _logger.Info(message);
        public void Warn(string message) => _logger.Warn(message);
        public void Error(string message, Exception? ex = null) => _logger.Error(ex, message);
        public void Fatal(string message, Exception? ex = null) => _logger.Fatal(ex, message);
    }
}
