using static SnakeConsole.Menu;
using System.Drawing;
using static SnakeConsole.Snake;

namespace SnakeConsole;

public class Game
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

                CheckIfGameWasPaused(key);

                snake.ChangeDirection(key);
            }

            var direction = snake.Move();
            CheckWallAndFoodCollision();
            Draw();
            SleepAfterMovement(direction);
        }

        GameOver();
    }

    private void WriteAt(int x, int y, string text)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(text);
    }

    private void CheckIfGameWasPaused(ConsoleKey key)
    {
        if (key != ConsoleKey.P) return;

        using (new ConsoleColorScope(ConsoleColor.Yellow))
        {
            var left = screenWidth / 2 - 15;
            var top = (screenHeight / 2) - 1;

            WriteAt(left, top++, "======= SNAKE GAME PAUSED =======");
            WriteAt(left, top++, "        1. Resume Game");
            WriteAt(left, top++, "        2. Quit");
            WriteAt(left, top++, $"  ==== CURRENT SCORE: {score: 0} ====  ");

            Console.SetCursorPosition(0, screenHeight + 2);
        }

        while (true)
        {
            var input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.D1 || input == ConsoleKey.NumPad1)
            {
                Console.Clear();
                DrawBorders();
                UpdateScore();
                return;
            }
            else if (input == ConsoleKey.D2 || input == ConsoleKey.NumPad2)
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid option. Please select again.");
            }
        }
    }

    private void SleepAfterMovement(SnakeDirection direction)
    {
        Thread.Sleep(speed);

        if (direction == SnakeDirection.Down || direction == SnakeDirection.Up)
        {
            Thread.Sleep(speed / 4);
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

        Console.SetCursorPosition(0, screenHeight + 2);
        Console.ReadKey();
    }
}
