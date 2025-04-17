using PluginSystem.Abstractions.Commands;

public class ConsoleOutput : IConsoleOutput
{
    public void Write(string message)
    {
        Console.Write(message);
    }
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
    public void WriteError(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }
}