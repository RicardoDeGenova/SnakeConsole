using static SnakeConsole.Menu;
using System.Drawing;
using static SnakeConsole.Snake;
using System.Text;
using System.ComponentModel.DataAnnotations;

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

    public Game(Size size, Difficulty difficulty)
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;
        
        Speed = (int)difficulty;

        screenWidth = size.Width;
        screenHeight = size.Height;
        snake = new Snake(screenWidth / 2, screenHeight / 2);
        food = new Food(screenWidth, screenHeight);
        food.Generate(snake);

        DrawBorders();
    }

    private int speed;
    private int Speed
    {
        get => speed;
        set
        {
            if (value < 10)
            {
                speed = 10;
                return;
            }

            speed = value;
        }
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
        Thread.Sleep(Speed);

        if (direction == SnakeDirection.Down || direction == SnakeDirection.Up)
        {
            Thread.Sleep(Speed / 4);
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

            if (score % 2 == 0 && score != 0)
                Speed--;
        }
    }

    private void DrawBorders()
    {
        var consoleRectangle = new ConsoleRectangle(screenWidth, screenHeight, new Point(0, 0), ConsoleColor.White);
        consoleRectangle.Draw();
    }

    private void UpdateScore()
    {
        using var _ = new ConsoleColorScope(ConsoleColor.Yellow);

        Console.SetCursorPosition(0, screenHeight + 2);
        Console.Write($"Score: {score}");
    }

    private void GameOver()
    {
        using (new ConsoleColorScope(ConsoleColor.Red))
        {
            Console.SetCursorPosition(screenWidth / 2 - 5, screenHeight / 2);
            Console.Write($"Game Over! Score: {score}");
        }

        Console.SetCursorPosition(0, screenHeight + 3);
        Console.ReadKey();
    }
}
