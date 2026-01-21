using System.Drawing;

class Statki
{
    int[,] Gen_board = new int[10, 10];

    public Statki()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Gen_board[i, j] = '~';
            }
        }
    }

    public void Run()
    {
        //generateboards();
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
                Console.Write((char) Gen_board[i, j]);
                switch (Gen_board[i, j])
                {
                    case 1:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[ X ]");
                        break;
                    case 2:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[ S ]");
                        break;
                    case 3:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("[ O ]");
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write("[ ~ ]");
                        break;

                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }

    //    void generateboards()
    //    {
    //        int[] Rand_row = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    //        int[] Rand_col = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    //        Random rand = new Random();

    //        for (int ship_type = 1; ship_type < 5; ship_type++)
    //        {
    //            for (int ships_i = 0; ships_i < ship_type; ships_i++)
    //            {
    //                rand.Shuffle(Rand_row);
    //                rand.Shuffle(Rand_col);

    //                for (int i = 0; i < 10; i++)
    //                {
    //                    for (int j = 0; j < 10; j++)
    //                    {
    //                        if ('~' == Gen_board[Rand_row[i], Rand_col[j]])
    //                        {

    //                            int cell_count = 0;
    //                            switch (ship_type)
    //                            {
    //                                case 1:
    //                                    cell_count = 4;
    //                                    break;
    //                                case 2:
    //                                    cell_count = 3;
    //                                    break;
    //                                case 3:
    //                                    cell_count = 2;
    //                                    break;
    //                                case 4:
    //                                    cell_count = 1;
    //                                    break;
    //                            }
    //                            for (int dir = 0;dir < 4 ;dir++ )
    //                            {
    //                                for (int cell_idx = 0; cell_idx < cell_count; cell_idx++)
    //                                {
    //                                    switch (dir)
    //                                    {
    //                                        case 1: // LEFT
    //                                            if (Gen_board[Rand_row[i] - cell_idx, Rand_col[j]] == '~')
    //                                            {
    //                                                Gen_board[Rand_row[i] - cell_idx, Rand_col[j]] = 'S';
    //                                            }
    //                                            else
    //                                            {

    //                                            }
    //                                                break;
    //                                        case 2: // RIGHT
    //                                            if (Gen_board[Rand_row[i] + cell_idx, Rand_col[j]] == '~')
    //                                            {
    //                                                Gen_board[Rand_row[i] + cell_idx, Rand_col[j]] = 'S';
    //                                            }
    //                                            else
    //                                            {

    //                                            }
    //                                                break;
    //                                        case 3: // UP
    //                                            if (Gen_board[Rand_row[i]  ,Rand_col[j] - cell_idx] == '~')
    //                                            {
    //                                                Gen_board[Rand_row[i]  ,Rand_col[j] - cell_idx] = 'S';
    //                                            }
    //                                            else
    //                                            {

    //                                            }
    //                                                break;
    //                                        case 4: // DOWN
    //                                            if (Gen_board[Rand_row[i] - cell_idx, Rand_col[j] + cell_idx] == '~')
    //                                            {
    //                                                Gen_board[Rand_row[i] - cell_idx, Rand_col[j] + cell_idx] = 'S';
    //                                            }
    //                                            else
    //                                            {

    //                                            }
    //                                                break;
    //                                    }
    //                                }
    //                            }
    //                        }

    //                    }

    //                }
    //            }
    //        }
    //    }
}



