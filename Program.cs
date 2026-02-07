class Program
{
    public static void Main()
    {
        Console.WriteLine("start");
        bool isExit = false;

        Board gen_board = new Board();
        gen_board.GenerateRandom();

        Board player_board = new Board();

        int Start_X = 0, Start_Y = 0, End_X = 0, End_Y = 0;
        int Ship_Count = 0;

        Dictionary<int, int> SizeCountPair = new Dictionary<int, int>();

        Console.WriteLine("Witaj użytkowniku!");
        Console.WriteLine("Podaj współrzędne aby rozstawić statki.");

        //Wybór statków gracza
        while (Ship_Count < 10)
        {
            Console.WriteLine("Podaj współrzędne początku statku (x y):");
            while (!int.TryParse(Console.ReadLine(), out Start_X) || !int.TryParse(Console.ReadLine(), out Start_Y))
            {
                Console.WriteLine("Podałeś współrzędne poza planszą lub nie wpisałeś liczby całkowitej, podaj prawidłowe współrzędne");
            }

            Console.WriteLine("Podaj współrzędne końca statku (x y):");
            while (int.TryParse(Console.ReadLine(), out End_X) && int.TryParse(Console.ReadLine(), out End_Y))
            {
                Console.WriteLine("Podałeś współrzędne poza planszą lub nie wpisałeś liczby całkowitej, podaj prawidłowe współrzędne");
            }
            //do dokończenia funkcji try get ship_lenght
            if (SizeCountPair[])
            {

            }
            if (player_board.TryAddShipFromTo(Start_X, Start_Y, End_X, End_Y))
            {
                Ship_Count++;    
            }
            else
            {
                Console.WriteLine("Statek nie może być na ukos!");
            }
        }
        Console.Clear();

        while (!isExit)
        {
            gen_board.Display();

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