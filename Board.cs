using System.ComponentModel.DataAnnotations;
using System.Drawing;
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

    public bool TryAddShipFromTo(int start_x, int start_y, int end_x, int end_y)
    {
        if (!TryGetShipLength(start_x, start_y, end_x, end_y, out int cell_count))
            return false;

        Vector2 dir = GetShipDir(start_x, start_y, end_x, end_y); //zle obliczanie kierunku
        if (TryAddShipCells(Math.Min(start_x, end_x), Math.Min(start_y, end_y), cell_count, dir))
            return true;

        return false;
    }

    /// <summary>
    /// zwraca bool i out int size przedstawiający ilość komórek wertykalnie i horyzontalnie
    /// </summary>
    /// <returns>Zwraca true jeżeli szerokość i wysokość nie są naraz większe od 1 lub false, gdy są np. dla statku 2x2</returns>
    public static bool TryGetShipLength(int start_x, int start_y, int end_x, int end_y, out int size)
    {
        int height = Math.Abs(end_y - start_y) + 1;
        int width = Math.Abs(end_x - start_x) + 1;

        size = Math.Max(width, height);

        if (width != 1 && height != 1)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Zwraca kierunek od rogu najbliższego (0, 0) do rogu najbliższego (9, 9), zakładając, że szerokość i wysokość nie są naraz większe od 1 np. statek 2x2
    /// </summary>
    /// <returns>Kierunek Dół (0, 1), Prawo (1, 0)</returns>
    public static Vector2 GetShipDir(int start_x, int start_y, int end_x, int end_y)
    {
        Vector2 dist = new(Math.Max(start_x, end_x) - Math.Min(start_x, end_x), Math.Max(start_y, end_y) - Math.Min(start_y, end_y));
        //do naprawy
        return Vector2.Clamp(dist, new(0, 0), new(1, 1));
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
    int n = 0;
    public void Display()
    {
        Console.Write("   ");
        Console.ResetColor();

        for (int i = 0; i < 10; i++)
        {
            Console.Write($"|{(char)(i + 'A')}|");
        }
        Console.WriteLine();

        for (int y = 0; y < 10; y++)
        {
            Console.Write($"|{y}|");

            for (int x = 0; x < 10; x++)
            {

                switch (value[y, x])
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
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
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