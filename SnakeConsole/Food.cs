namespace SnakeConsole;

public class Food
{
    private int screenWidth;
    private int screenHeight;
    public (int X, int Y) Position { get; private set; }
    private Random random = new Random();

    public Food(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;
    }

    public void Generate(Snake snake)
    {
        do
        {
            Position = (random.Next(1, screenWidth - 1), random.Next(1, screenHeight - 1));
        } while (snake.GetHeadPosition() == Position);
    }

    public void Draw()
    {
        using var _ = new ConsoleColorScope(ConsoleColor.Yellow);

        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write("♥");
    }
}
