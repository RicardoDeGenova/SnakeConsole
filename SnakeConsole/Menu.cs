namespace SnakeConsole;

using System;

class Menu
{
    public enum Difficulty
    {
        Easy = 100,
        Medium = 50,
        Hard = 15
    }

    public static Difficulty ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("===== SNAKE GAME =====");
        Console.WriteLine("1. Play Game");
        Console.WriteLine("2. Quit");
        Console.WriteLine("======================");
        Console.Write("Please select an option: ");

        while (true)
        {
            var input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.D1 || input == ConsoleKey.NumPad1)
            {
                return ShowDifficultyMenu();
            }
            else if (input == ConsoleKey.D2 || input == ConsoleKey.NumPad2)
            {
                Environment.Exit(0);  // Quit the game
            }
            else
            {
                Console.WriteLine("Invalid option. Please select again.");
            }
        }
    }

    private static Difficulty ShowDifficultyMenu()
    {
        Console.Clear();
        Console.WriteLine("===== SELECT DIFFICULTY =====");
        Console.WriteLine("1. Easy");
        Console.WriteLine("2. Medium");
        Console.WriteLine("3. Hard");
        Console.WriteLine("=============================");
        Console.Write("Please select a difficulty: ");

        while (true)
        {
            var input = Console.ReadKey(true).Key;
            switch (input)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return Difficulty.Easy;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return Difficulty.Medium;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return Difficulty.Hard;
                default:
                    Console.WriteLine("Invalid option. Please select again.");
                    break;
            }
        }
    }
}
