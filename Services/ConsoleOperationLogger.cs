namespace SimpleChat.Services;

public class ConsoleOperationLogger : IOperationLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[CONSOLE] {message}");
    }
}