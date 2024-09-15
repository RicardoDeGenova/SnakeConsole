namespace SnakeConsole;

public class Snake
{
    public enum SnakeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private List<(int X, int Y)> body;
    private SnakeDirection direction;
    private SnakeDirection lastDirection;
    private (int X, int Y)? previousTail;

    public Snake(int startX, int startY)
    {
        body = new List<(int X, int Y)> { (startX, startY) };
        direction = SnakeDirection.Right;
        lastDirection = direction;
        previousTail = null;
    }

    public void ChangeDirection(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W when lastDirection != SnakeDirection.Down:
                direction = SnakeDirection.Up;
                break;
            case ConsoleKey.S when lastDirection != SnakeDirection.Up:
                direction = SnakeDirection.Down;
                break;
            case ConsoleKey.A when lastDirection != SnakeDirection.Right:
                direction = SnakeDirection.Left;
                break;
            case ConsoleKey.D when lastDirection != SnakeDirection.Left:
                direction = SnakeDirection.Right;
                break;
        }
    }

    public SnakeDirection Move()
    {
        var head = body.First();

        switch (direction)
        {
            case SnakeDirection.Right: head.X++; break;
            case SnakeDirection.Left: head.X--; break;
            case SnakeDirection.Up: head.Y--; break;
            case SnakeDirection.Down: head.Y++; break;
        }

        previousTail = body.Last();

        body.Insert(0, head);

        body.RemoveAt(body.Count - 1);

        lastDirection = direction;

        return direction;
    }

    public void Grow()
    {
        body.Add(body.Last());
    }

    public (int X, int Y) GetHeadPosition() => body.First();
    public (int X, int Y)? GetPreviousTailPosition() => previousTail;

    public bool HasSelfCollision() => body.Skip(1).Any(segment => segment == body.First());

    public void Draw()
    {
        using var _ = new ConsoleColorScope(ConsoleColor.Green);

        Console.SetCursorPosition(body[0].X, body[0].Y);
        Console.Write("O");

        for (int i = 1; i < body.Count; i++)
        {
            Console.SetCursorPosition(body[i].X, body[i].Y);
            Console.Write("o");
        }
    }
}
