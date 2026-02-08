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

        int Destroyed_Player_Ships = 0;
        int Destroyed_Enemy_Ships = 0;
        int Missed_Player_Shots = 0;
        int Round_Count = 0;

        while (!isExit)
        {
            Console.Clear();
            Console.WriteLine("-----------------\r\nTura Przeciwnika\r\n-----------------");
            player_board.Display();
            Thread.Sleep(3000);
            if (player_board.Destroy_Random_Cell())
            {
                Destroyed_Player_Ships++;
            }//Przyszłościowo dodać zliczanie nietrafionych celów 2 gracza.
                Console.Clear();
            player_board.Display();
            Thread.Sleep(3000);
            Console.Clear();
            if (Destroyed_Player_Ships == 10)
            {
                Console.WriteLine("Niestety przegrałeś, Spróbój ponownie.");
                Console.WriteLine("Liczba trafień:"+ Destroyed_Enemy_Ships);
                Console.WriteLine("Liczba niecelnych strzałów" + Missed_Player_Shots);
                Console.WriteLine("Liczba rozegranych tur:" + Round_Count);
            }


            Console.WriteLine("-----------------\r\nTwoja Tura\r\n-----------------");
            gen_board.DisplayHidden();
            if (gen_board.Manual_Destroy_Cell())
            {
                Destroyed_Enemy_Ships++;
            }
            else
            {
                Missed_Player_Shots++;
            }
                gen_board.DisplayHidden();
            Thread.Sleep(3000);
            if (Destroyed_Enemy_Ships == 10)
            {
                Console.WriteLine("Gratulację, wygrałeś!");
                Console.WriteLine("Liczba trafień:" + Destroyed_Enemy_Ships);
                Console.WriteLine("Liczba niecelnych strzałów" + Missed_Player_Shots);
                Console.WriteLine("Liczba rozegranych tur:" + Round_Count);
            }
            Console.Clear();

            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.KeyChar == 'e')
            isExit = true;
            Round_Count++;
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