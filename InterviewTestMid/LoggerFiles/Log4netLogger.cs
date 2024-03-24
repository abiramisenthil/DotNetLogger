using log4net;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;


namespace InterviewTestMid.LoggerFiles
{
    public class Log4netLogger : Logger, ILogger
    {
        private readonly ILog _log;

        private readonly IConfiguration _configuration;

        public Log4netLogger(string loggerName, IConfiguration configuration) : base(configuration)
        {
            _log = LogManager.GetLogger(loggerName);
            _configuration = configuration;
        }

        public void WriteLogMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string formattedMessage = $"{timestamp}: {message}";
            _log.Debug(formattedMessage);

        }
      
    }
}
