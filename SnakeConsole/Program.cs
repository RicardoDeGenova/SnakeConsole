using SnakeConsole;
using System.Drawing;

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

