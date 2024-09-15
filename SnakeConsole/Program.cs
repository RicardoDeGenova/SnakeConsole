using SnakeConsole;
using System.Drawing;
using static SnakeConsole.Menu;
using static SnakeGame.Snake;

namespace SnakeGame;

class Program
{
    static void Main(string[] args)
    {
        var difficulty = Menu.ShowMenu();

        Console.CursorVisible = false;
        Game game = new Game(new Size(60, 20), difficulty);
        game.Run();
    }
}

class Game
{
    private int screenWidth;
    private int screenHeight;
    private Snake snake;
    private Food food;
    private int score = 0;
    private bool gameOver = false;
    private Random random = new Random();
    private int lastScore = -1;
    private int speed = 100;

    public Game(Size size, Difficulty difficulty)
    {
        Console.Clear();
        speed = (int)difficulty;

        screenWidth = size.Width;
        screenHeight = size.Height;
        snake = new Snake(screenWidth / 2, screenHeight / 2);
        food = new Food(screenWidth, screenHeight);
        food.Generate(snake);

        DrawBorders();
    }

    public void Run()
    {
        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                snake.ChangeDirection(key);
            }

            var direction = snake.Move();
            CheckWallAndFoodCollision();
            Draw();
            SleepAfterMovement(direction);
        }

        GameOver();
    }

    private void SleepAfterMovement(SnakeDirection direction)
    {
        Thread.Sleep(speed);

        if (direction == SnakeDirection.Down || direction == SnakeDirection.Up)
        {
            Thread.Sleep(speed/4);
        }
    }

    private void CheckWallAndFoodCollision()
    {
        var head = snake.GetHeadPosition();

        if (head.X <= 0 || head.X >= screenWidth || head.Y <= 0 || head.Y >= screenHeight
            || snake.HasSelfCollision())
        {
            gameOver = true;
            return;
        }

        if (head.X == food.Position.X && head.Y == food.Position.Y)
        {
            score++;
            snake.Grow();
            food.Generate(snake);
        }
    }

    private void Draw()
    {
        var tail = snake.GetPreviousTailPosition();
        if (tail != null)
        {
            Console.SetCursorPosition(tail.Value.X, tail.Value.Y);
            Console.Write(" ");
        }

        snake.Draw();

        food.Draw();

        if (score != lastScore)
        {
            UpdateScore();
            lastScore = score;
        }
    }

    private void DrawBorders()
    {
        for (int i = 0; i <= screenWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("#");
            Console.SetCursorPosition(i, screenHeight);
            Console.Write("#");
        }

        for (int i = 0; i <= screenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("#");
            Console.SetCursorPosition(screenWidth, i);
            Console.Write("#");
        }
    }

    private void UpdateScore()
    {
        using var _ = new ConsoleColorScope(ConsoleColor.Yellow);

        Console.SetCursorPosition(0, screenHeight + 1);
        Console.Write($"Score: {score}");
    }

    private void GameOver()
    {
        using (new ConsoleColorScope(ConsoleColor.Red))
        {
            Console.SetCursorPosition(screenWidth / 2 - 5, screenHeight / 2);
            Console.Write($"Game Over! Score: {score}");
        }

        Console.ReadKey();
    }
}

class Snake
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

class Food
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
        Console.Write("X");
    }
}
