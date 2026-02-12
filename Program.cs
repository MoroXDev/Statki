public class Program
{
    static void Main()
    {
    GAME_START:

        Console.WriteLine(Board.Logo);

        Console.Title = "Statki";

        Board gen_board = new Board();
        gen_board.GenerateRandom();

        Board player_board = new Board();

        //player_board.GenerateRandom();
        player_board.Manual_Ships_Init();

        int Destroyed_Player_Ships = 0;
        int Destroyed_Enemy_Ships = 0;
        int Missed_Player_Shots = 0;
        int Round_Count = 0;

        while (true)
        {
            Round_Count++;
            Console.Clear();
            Console.WriteLine("-----------------\r\nPrzeciwnik Strzela\r\n-----------------");
            player_board.Display();
            Thread.Sleep(3000);

            bool player_board_hit;
            do
            {
                player_board_hit = player_board.Destroy_Random_Ship();
                if (player_board_hit)
                    Destroyed_Player_Ships++;

                Console.Clear();
                Console.WriteLine("-----------------\r\nPrzeciwnik Strzelił\r\n-----------------");
                player_board.Display();
                Thread.Sleep(3000);

                if (Destroyed_Player_Ships == 20)
                {
                    Console.WriteLine("Niestety przegrałeś, Spróbuj ponownie.");
                    goto END_SCREEN;
                }
            }
            while (player_board_hit);


            Console.Clear();
            Console.WriteLine("-----------------\r\nTy Strzelasz\r\n-----------------");
            gen_board.DisplayHidden();

            bool gen_board_hit;
            do
            {
                gen_board_hit = gen_board.Manual_Destroy_Ship(out bool is_ship_destroyed);

                if (gen_board_hit)
                    Destroyed_Enemy_Ships++;
                else
                    Missed_Player_Shots++;

                Console.Clear();
                Console.WriteLine("-----------------\r\nTy Strzeliłeś\r\n-----------------");
                gen_board.DisplayHidden();
                if (gen_board_hit)
                {
                    if (is_ship_destroyed)
                        Console.WriteLine("Zatopiony!");
                    else
                        Console.WriteLine("Trafiony!");
                }
                else
                    Console.WriteLine("Pudło!");

                if (Destroyed_Enemy_Ships == 20)
                {
                    Console.WriteLine("Gratulację, wygrałeś!");
                    goto END_SCREEN;
                }
            }
            while (gen_board_hit);
            Thread.Sleep(3000);

            Console.Clear();
        }
    END_SCREEN:
        Console.WriteLine("Liczba trafień:" + Destroyed_Enemy_Ships);
        Console.WriteLine("Liczba niecelnych strzałów:" + Missed_Player_Shots);
        Console.WriteLine("Liczba rozegranych tur:" + Round_Count);
        Console.WriteLine("Naciśnij 'Escape', aby wyjść z gry, lub 'Enter', by spróbować ponownie.");
        ConsoleKeyInfo key_info = Console.ReadKey();

        while (key_info.Key != ConsoleKey.Enter && key_info.Key != ConsoleKey.Escape)
        {
            Console.WriteLine("Zły przycisk, naciśnij 'Escape', aby wyjść z gry, lub 'Enter', by spróbować ponownie.");
            key_info = Console.ReadKey();
        }

        if (key_info.Key == ConsoleKey.Enter)
        {
            goto GAME_START;
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

enum HitState
{
    Hit,
    Miss,
    Occupied
}


