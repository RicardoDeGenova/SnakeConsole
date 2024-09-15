using System;

public class ConsoleColorScope : IDisposable
{
    private readonly ConsoleColor _originalColor;

    public ConsoleColorScope(ConsoleColor newColor)
    {
        _originalColor = Console.ForegroundColor;

        Console.ForegroundColor = newColor;
    }

    public void Dispose()
    {
        Console.ForegroundColor = _originalColor;
    }
}