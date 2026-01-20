class Statki
{
    char[,] playerBoard = new char[10, 10];

    public Statki()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                playerBoard[i, j] = '~';
            }
        }
    }

    public void Run()
    {
        generateboards();
        bool isExit = false;

        while (!isExit)
        {
            displayBoard();
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.KeyChar == 'e')
                isExit = true;
        }
    }

    void displayBoard()
    {
        Console.Clear();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write(playerBoard[i, j]);
            }
            Console.WriteLine();
        }
    }

    void generateboards()
    {
        int[] Rand_row = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] Rand_col = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Random rand = new Random();

        for (int type = 1; type < 5; type++)
        {
            //for (int cell_i = 1; cell_i < 5; cell_i++)
            //{
                
            //}
            for (int ships_i = 0; ships_i < type; ships_i++)
            {
                rand.Shuffle(Rand_row);
                rand.Shuffle(Rand_col);

                //for (int i = 0;i < 10;i++)
                //{
                //    for (int j = 0;j < 10;j++)
                //    {
                //        if ('~' == playerBoard[Rand_row[i], Rand_col[j]])
                //        {

                //        }

                //    }

                //}
            }
        }
    }

}


