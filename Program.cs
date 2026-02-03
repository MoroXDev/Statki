class Program
{
    public static void Main()
    {
        Console.WriteLine("start");
        bool isExit = false;

        Board gen_board = new Board();
        // gen_board.GenerateRandom();


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
    Water = 0,
    Hit = 1,
    Ship = 2,
    Miss = 3,
    OutOfBounds = 4
}

