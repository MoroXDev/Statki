using System.Numerics;
class Board
{
    int[,] value = new int[10, 10];

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
        if (GetCellAt(x, y) == (int)CellState.Water)
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
            if (GetCellAt(x + (int)dir.X * cell_count, y + (int)dir.Y * cell_count) == (int)CellState.Water)
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
            value[(int)index.X, (int)index.Y] = (int)CellState.Ship;
        }
        return true;
    }

    int GetCellAt(int x, int y)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            return (char)value[x, y];
        }
        return (int)CellState.OutOfBounds;
    }

    public void Display()
    {
        Console.ResetColor();
        Console.Clear();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write((char)value[i, j]);
                switch (value[i, j])
                {
                    case (int)CellState.Hit:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("|X|");
                        break;
                    case (int)CellState.Ship:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("|S|");
                        break;
                    case (int)CellState.Miss:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("|O|");
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
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
                value[i, j] = (int)CellState.Water;
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