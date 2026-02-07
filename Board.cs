using System.Numerics;
using System.Security.Cryptography.X509Certificates;
class Board
{
    CellState[,] value = new CellState[10, 10];
    readonly Vector2[] dirs_around = {
        new (1, 0),
        new (1, 1),
        new (0, 1),
        new (-1, 1),
        new (-1, 0),
        new (-1, -1),
        new (0, -1),
        new (1, -1),
    };

    public void GenerateRandom()
    {
        int[] rand_row = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] rand_col = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Random rand = new Random();

        bool ships_inserted;
        do
        {
            ships_inserted = true;
            restart();
            for (int ship_type = 1; ship_type < 5; ship_type++)
            {
                for (int ships_i = 0; ships_i < ship_type; ships_i++)
                {
                    rand.Shuffle(rand_row);
                    rand.Shuffle(rand_col);

                    if (!TryAddShipRandomized(ref rand_row, ref rand_col, ship_type, ref rand))
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

    bool TryAddShipRandomized(ref int[] rand_row, ref int[] rand_col, int ship_type, ref Random rand)
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (TryAddShipWithRandDir(rand_row[x], rand_col[y], ship_type, ref rand))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool TryAddShipWithRandDir(int x, int y, int ship_type, ref Random dir_rand)
    {
        if (GetCellAt(x, y) == CellState.Water)
        {
            int cell_count = 5 - ship_type;

            Vector2[] directions =
            {
                new (-1, 0), // LEFT
                new (1, 0),  // RIGHT
                new (0, -1), // UP
                new (0, 1)   // DOWN
            };

            dir_rand.Shuffle(directions);

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

    public bool TryAddShipCells(int x, int y, int cell_count, Vector2 dir)
    {
        Vector2[] correct_indexes = new Vector2[cell_count];

        for (int i = 0; i < cell_count; i++)
        {
            Vector2 next_pos = new(x + dir.X * i, y + dir.Y * i);
            if (GetCellAt(next_pos) != CellState.Water || IsShipAround(next_pos))
                return false;

            correct_indexes[i] = next_pos;
        }

        //dodaje statek pod koniec
        foreach (var index in correct_indexes)
        {
            value[(int)index.Y, (int)index.X] = CellState.Ship;
        }
        return true;
    }

    bool TryAddShipFromTo(int start_x, int start_y, int end_x, int end_y)
    {
        if (GetCellAt(start_x, start_y) == CellState.Water)
        {

        }
        return false;
    }

    public static bool TryGetShipLength(int start_x, int start_y, int end_x, int end_y, out int size)
    {
        int height = Math.Abs(end_y - start_y) + 1;
        int width = Math.Abs(end_x - start_x) + 1;
        
        if (width != 1 && height != 1)
        {
            size = width + height;
            return false; 
        }

        size = width + height;
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

    CellState GetCellAt(Vector2 pos)
    {
        if ((int)pos.X >= 0 && pos.X < 10 && pos.Y >= 0 && pos.Y < 10)
        {
            return value[(int)pos.Y, (int)pos.X];
        }
        return CellState.OutOfBounds;
    }

    bool IsShipAround(Vector2 pos)
    {
        foreach (var dir in dirs_around)
        {
            Vector2 index = new(pos.X + dir.X, pos.Y + dir.Y);
            if (GetCellAt(index) == CellState.Ship)
            {
                return true;
            }
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
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("|O|");
                        break;
                    case CellState.Hit:
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

    public void DisplayHidden()
    {
        Console.ResetColor();
        Console.Clear();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                switch (value[i, j])
                {
                    case CellState.Miss:
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("|O|");
                        break;
                    case CellState.Hit:
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

    void restart()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                value[i, j] = CellState.Water;
            }
        }
    }

    bool Kill(int x, int y)
    {
        if (GetCellAt(x, y) == CellState.Ship)
        {
            value[y, x] = CellState.Hit;
            return true; 
        }
        return false;
    }

    public Board()
    {
        restart();
    }
}