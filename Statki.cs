using System.Numerics;

class Statki
{
    char[,] Gen_board = new char[10, 10];

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
                Console.Write(Gen_board[i, j]);
            }
            Console.WriteLine();
        }
    }

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

    bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < 10 && y >= 0 && y < 10;
    }
}


