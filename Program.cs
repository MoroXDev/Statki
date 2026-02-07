class Program
{
    public static void Main()
    {
        Console.WriteLine("start");
        bool isExit = false;

        Board gen_board = new Board();
        gen_board.GenerateRandom();

        int Start_X = 0, Start_Y = 0, End_X = 0, End_Y = 0;
        Random random = new Random();
        int Ship_Count = 0;
        //Statki gracza

        Console.WriteLine("Witaj użytkowniku!");
        Console.WriteLine("Podaj współrzędne aby rozstawić statki.");

        while (Ship_Count < 10)
        {
            Console.WriteLine("Podaj współrzędne początku statku (x y):");
            while (!int.TryParse(Console.ReadLine(), out Start_X) || !int.TryParse(Console.ReadLine(), out Start_Y))
            {
                Console.WriteLine("Podałeś nieprawidłowe współrzędne, podaj prawidłowe współrzędne");
            }

            Console.WriteLine("Podaj współrzędne końca statku (x y):");
            while (int.TryParse(Console.ReadLine(), out End_X) && int.TryParse(Console.ReadLine(), out End_Y))
            {
                Console.WriteLine("Podałeś nieprawidłowe współrzędne, podaj prawidłowe.");
            }
            //do dokończenia funkcji try get ship_lenght
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