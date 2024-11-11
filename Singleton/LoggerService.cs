using NLog;

namespace Employee_System.Logging
{
    public class LoggerService
    {
        // Private static variable to hold the single instance of LoggerService
        private static readonly Lazy<LoggerService> _instance = new Lazy<LoggerService>(() => new LoggerService());

        // NLog logger instance
        private readonly NLog.ILogger _logger;

        // Private constructor to prevent direct instantiation
        private LoggerService()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        

        // Public static method to access the single instance
        public static LoggerService Instance => _instance.Value;

        // Wrapper methods for logging
        public void Info(string message) => _logger.Info(message);
        public void Warn(string message) => _logger.Warn(message);
        public void Error(string message, Exception ex = null) => _logger.Error(ex, message);
        public void Debug(string message) => _logger.Debug(message);
        public void Fatal(string message, Exception ex = null) => _logger.Fatal(ex, message);
    }
}
