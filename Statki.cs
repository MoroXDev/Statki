<<<<<<< HEAD
﻿using System.Drawing;
=======
﻿using System.Numerics;
>>>>>>> ade4515f9f4b293eaeed181cf016678549834894

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

<<<<<<< HEAD
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
=======
    void generateboards()
    {
        int[] rand_row = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] rand_col = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Random rand = new Random();

        bool ships_fit = true;
        while (!ships_fit)
        {
            ships_fit = true;
            for (int ship_type = 1; ship_type < 5; ship_type++)
            {
                for (int ships_i = 0; ships_i < ship_type; ships_i++)
                {
                    rand.Shuffle(rand_row);
                    rand.Shuffle(rand_col);

                    if (!TryAddShipRandomized(ref rand_row, ref rand_col, ship_type))
                    {
                        ships_fit = false;
                        ship_type = 5; // break loop
                        ships_i = ship_type; // break loop
                    }
                }
            }
        }
    }

    bool TryAddShipRandomized(ref int[] rand_row, ref int[] rand_col, int ship_type)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (TryAddShipAt(rand_row[i], rand_col[j], ship_type))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool TryAddShipAt(int x, int y, int ship_type)
    {
        if (Gen_board[x, y] == '~')
        {
            int cell_count = 5 - ship_type;

            Vector2[] directions = new Vector2[]
            {
                new Vector2(-1, 0), // LEFT
                new Vector2(1, 0),  // RIGHT
                new Vector2(0, -1), // UP
                new Vector2(0, 1)   // DOWN
            };

            foreach (var dir in directions)
            {
                if (TryAddShipCells(x, y, cell_count, dir))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool TryAddShipCells(int x, int y, int cell_count, Vector2 dir)
    {
        Vector2[] correct_indexes = new Vector2[cell_count];
        for (int i = 0; i < cell_count; i++)
        {
            if (Gen_board[x + (int)dir.X * cell_count, y + (int)dir.Y * cell_count] == '~')
            {
                correct_indexes[i] = new Vector2(x + (int)dir.X * cell_count, y + (int)dir.Y * cell_count);
            }
            else
            {
                return false;
            }
        }

        foreach (var index in correct_indexes)
        {
            Gen_board[(int)index.X, (int)index.Y] = 'S';
        }
        return true;
    }
>>>>>>> ade4515f9f4b293eaeed181cf016678549834894
}



