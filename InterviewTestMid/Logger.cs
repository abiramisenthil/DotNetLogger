using System.Diagnostics;

namespace InterviewTestMid
{
    internal class Logger
    {

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
    }
}
