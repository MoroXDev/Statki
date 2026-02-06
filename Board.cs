using System.Numerics;
class Board
{
    CellState[,] value = new CellState[10, 10];
    Vector2[] index_around = new Vector2[] {
    new Vector2(1, 0),
    new Vector2(1, 1),
    new Vector2(0, 1),
    new Vector2(-1, 1),
        new Vector2(-1, 0),
        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1),
    };

    public void GenerateRandom()
    {
        int[] rand_row = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] rand_col = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Random rand = new Random();

        bool ships_inserted;
        do
        {
            ships_inserted = true;
            init();
            for (int ship_type = 1; ship_type < 5; ship_type++)
            {
                for (int ships_i = 0; ships_i < ship_type; ships_i++)
                {
                    rand.Shuffle(rand_row);
                    rand.Shuffle(rand_col);

                    if (!TryAddShipRandomized(ref rand_row, ref rand_col, ship_type))
                    {
                        ships_inserted = false;
                        ship_type = 5; // break loop
                        ships_i = ship_type; // break loop
                    }
                }
            }
        }
        while (!ships_inserted);
    }

    bool TryAddShipRandomized(ref int[] rand_row, ref int[] rand_col, int ship_type)
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (TryAddShipAt(rand_row[x], rand_col[y], ship_type))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool TryAddShipAt(int x, int y, int ship_type)
    {
        if (GetCellAt(x, y) == CellState.Water)
        {
            int cell_count = 5 - ship_type;

            Vector2[] directions =
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
            Vector2 next_pos = new Vector2(x + (int)dir.X * i, y + (int)dir.Y * i);
            if (GetCellAt((int)next_pos.X, (int)next_pos.Y) == CellState.Water &&
                CheckCellsAround((int)next_pos.X, (int)next_pos.Y, CellState.Water))
            {
                correct_indexes[i] = next_pos;
            }
            else
            {
                return false;
            }
        }

        foreach (var index in correct_indexes)
        {
            value[(int)index.Y, (int)index.X] = CellState.Ship;
        }
        return true;
    }

    CellState GetCellAt(int x, int y)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            return value[y, x];
        }
        return CellState.OutOfBounds;
    }

    bool CheckCellsAround(int x, int y, CellState c_state)
    {
        if (GetCellAt(x + 1, y) == c_state ||
            GetCellAt(x + 1, y + 1) == c_state ||
            GetCellAt(x, y + 1) == c_state ||
            GetCellAt(x - 1, y + 1) == c_state ||
            GetCellAt(x - 1, y) == c_state ||
            GetCellAt(x - 1, y - 1) == c_state ||
            GetCellAt(x, y - 1) == c_state ||
            GetCellAt(x + 1, y - 1) == c_state)
        {
            return true;
        }

        return false;
    }

    public void Display()
    {
        Console.ResetColor();
        Console.Clear();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                switch (value[i, j])
                {
                    case CellState.Ship:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("|S|");
                        break;
                    case CellState.Miss:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("|X|");
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("|~|");
                        break;
                }
            }
            Console.WriteLine();
        }
    }

    void init()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                value[i, j] = CellState.Water;
            }
        }
        // value[0, 0] = (int)CellState.Ship; //teorytycznie zmiana w X ale zmienia się w Y, pierwszy wymiar oznacza rząd a drugi oznacza kolumne
        // value[1, 0] = (int)CellState.Ship;
    }

    public Board()
    {
        init();
    }
}