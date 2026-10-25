namespace SimpleChat.Services;

public class FileOperationLogger : IOperationLogger
{
    private readonly string _filePath = "operations.log";

    public void Log(string message)
    {
        string logMessage = $"[FILE] {DateTime.Now}: {message}\n";
        File.AppendAllText(_filePath, logMessage);
    }
}