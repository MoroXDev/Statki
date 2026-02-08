using System.Numerics;

class Program
{
    public static void Main()
    {
        Console.Title = "Statki";
        bool isExit = false;

        Board gen_board = new Board();
        gen_board.GenerateRandom();

        Board player_board = new Board();

        player_board.Manual_Ships_Init();

        int x = 0;
            int y = 0;

        while (!isExit)
        {
            Console.Clear();
            // gen_board.Display();
            player_board.Display();
            Console.WriteLine($"{x} {y}");

            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.KeyChar == 'e')
                isExit = true;
            // else if (key.Key == ConsoleKey.UpArrow)
            // y--;
            // else if (key.Key == ConsoleKey.DownArrow)
            // y++;
            // else if (key.Key == ConsoleKey.LeftArrow)
            // x--;
            // else if (key.Key == ConsoleKey.RightArrow)
            // x++;
        }
    }
}

enum CellState
{
    Water,
    Ship,
    Miss,
    Hit,
    OutOfBounds,
}