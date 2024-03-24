namespace InterviewTestMid.LoggerFiles
{
    public interface ILogger
    {
        void WriteLogMessage(string logMessage);
        void WriteErrorMessage(Exception ex);
        void StringListWriteInCsv(List<string> data);
    }
}
