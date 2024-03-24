using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace InterviewTestMid.LoggerFiles
{
    public class Logger : ILogger
    {
        private readonly IConfiguration _configuration;

        public Logger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void WriteLogMessage(string LogMessage)
        {
            if (string.IsNullOrEmpty(LogMessage))
                throw new ArgumentException("Log message not provided", "LogMessage");

            Debug.WriteLine(LogMessage);
        }

        public void WriteErrorMessage(Exception Ex)
        {
            if (Ex == null)
                throw new ArgumentException("Exception not provided", "Ex");

            Debug.WriteLine($"Error recieved: {Ex.Message}");
            Debug.WriteLine($"{Ex.StackTrace}");
        }


        public void StringListWriteInCsv(List<string> data)
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data is null or empty", nameof(data));

            string filePath = _configuration["ValidJsonFilePath"];
            string csvFilePath = Path.Combine(Path.GetDirectoryName(filePath), _configuration["CsvFile"]);

            if (string.IsNullOrEmpty(csvFilePath))
            {
                Debug.WriteLine("CSV File Path not found");
                return;
            }


            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                foreach (var line in data)
                {
                    writer.WriteLine(line);
                }
            }

            Debug.WriteLine($"CSV file written to: {csvFilePath}");
        }
    }
}
