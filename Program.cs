using System.ComponentModel.DataAnnotations;
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

        player_board.GenerateRandom();
        //player_board.Manual_Ships_Init();

        while (!isExit)
        {
            Console.Clear();
            player_board.Display();
            Thread.Sleep(4000);
            player_board.Destroy_Random_Cell();
            Console.Clear();
            player_board.Display();
            Thread.Sleep(4000);
            Console.Clear();
            gen_board.DisplayHidden();
            gen_board.Manual_Destroy_Cell();
            gen_board.DisplayHidden();
            Thread.Sleep(4000);
            

            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.KeyChar == 'e')
            isExit = true;
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