using System.Numerics;
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

    Vector2[] directions ={
        new (-1, 0), // LEFT
        new (1, 0),  // RIGHT
        new (0, -1), // UP
        new (0, 1)   // DOWN
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

    public bool Destroy_Random_Ship()
    {
        Random rand = new Random();
        int x, y;
        HitState hit_state;
        do
        {
            x = rand.Next(0, 9);
            y = rand.Next(0, 9);
            hit_state = Try_Destroy_Cell(x, y);
        }
        while (hit_state == HitState.Occupied);

        return hit_state == HitState.Hit;
    }

    public bool TryAddShipWithRandDir(int x, int y, int ship_type, ref Random dir_rand)
    {
        if (GetCellAt(x, y) == CellState.Water)
        {
            int cell_count = 5 - ship_type;



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

    public bool Manual_Destroy_Ship(out bool is_ship_destroyed)
    {
        while (true)
        {
            Console.WriteLine("Podaj współrzędne ataku (A-J 1-10):");
            ReceiveCordinatesInput(out int x, out int y);
            HitState hit_state = Try_Destroy_Cell(x, y);
            if (hit_state == HitState.Occupied)
            {
                Console.WriteLine("Już strzelałeś w to pole! Spróbuj ponownie.");
            }
            else
            {
                if (hit_state == HitState.Hit)
                    is_ship_destroyed = IsShipDestroyed(new Vector2(x, y));
                else
                    is_ship_destroyed = false;

                return hit_state == HitState.Hit;
            }
        }
    }

    public bool IsShipDestroyed(Vector2 pos)
    {
        foreach (var dir in directions)
        {
            CellState c_state;
            int i = 1;
            do
            {
                c_state = GetCellAt(pos + dir * i);
                if (c_state == CellState.Ship)
                    return false;
                i++;
            }
            while (c_state == CellState.Hit);
        }

        return true;
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

    public void Display()
    {
        Console.Write("    ");
        Console.ResetColor();

        for (int i = 0; i < 10; i++)
        {
            Console.Write($"|{(char)(i + 'A')}|");
        }
        Console.WriteLine();

        for (int y = 0; y < 10; y++)
        {
            Console.ResetColor();
            Console.Write($"|{y + 1}|".PadLeft(4));

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
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("|O|");
                        break;
                    case CellState.Hit:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
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
        Console.ResetColor();
    }

    void ReceiveCordinatesInput(out int x_out, out int y_out)
    {
        x_out = 0;
        y_out = 0;
        bool valid_input = false;
        do
        {
            Console.Write("(A-J):");
            bool is_letter_valid = char.TryParse(Console.ReadLine(), out char x);
            Console.Write("(1-10):");
            bool is_number_valid = int.TryParse(Console.ReadLine(), out int y);
            if (is_letter_valid && is_number_valid)
            {
                x = char.ToUpper(x);
                if (x >= 'A' && x <= 'J' && y >= 1 && y <= 10)
                {
                    x_out = x - 'A';
                    y_out = y - 1;
                    valid_input = true;
                }
                else
                {
                    Console.WriteLine("Podałeś współrzędne poza planszą. Spróbuj ponownie.");
                }
            }
            else
            {
                Console.WriteLine("Podałeś nieprawidłowe współrzędne. Spróbuj ponownie.");
            }
        } while (!valid_input);
    }

    void DisplayAvailableShips(ref Dictionary<int, int> Size_AvailableCount)
    {
        int i = 0;
        foreach (var size_count in Size_AvailableCount.Reverse())
        {
            i++;
            Console.Write(i + ".");
            for (int count = 0; count < size_count.Value; count++)
            {
                for (int size = 0; size < size_count.Key; size++)
                {
                    Console.Write("|X|");
                }
                Console.Write("   ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void Manual_Ships_Init()
    {
        int Start_X = 0, Start_Y = 0, End_X = 0, End_Y = 0;
        int Ship_Count = 0;


        Dictionary<int, int> Size_AvailableCount = new Dictionary<int, int>()
        {
            { 4, 1 },
            { 3, 2 },
            { 2, 3 },
            { 1, 4 }
        };


        Console.WriteLine("Witaj użytkowniku!");
        Console.WriteLine("Podaj współrzędne aby rozstawić statki.");


        while (Ship_Count < 10)
        {
            Console.WriteLine("Dostępne Statki:");
            DisplayAvailableShips(ref Size_AvailableCount);
            Display();


            Console.WriteLine("Podaj współrzędne Startu (A-J 1-10):");
            ReceiveCordinatesInput(out Start_X, out Start_Y);
            Console.WriteLine("Podaj współrzędne Końca (A-J 1-10):");
            ReceiveCordinatesInput(out End_X, out End_Y);

            bool ship_added = false;
            if (Board.TryGetShipLength(Start_X, Start_Y, End_X, End_Y, out int size))
            {
                if (Size_AvailableCount.TryGetValue(size, out int count))
                {
                    if (count > 0)
                    {
                        if (TryAddShipFromTo(Start_X, Start_Y, End_X, End_Y))
                        {
                            Size_AvailableCount[size]--;
                            Ship_Count++;
                            ship_added = true;
                        }
                        else
                        {
                            Console.WriteLine("Statek koliduje z statkami lub wychodzi poza plansze!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nie ma już dostępnej wielkości statku: " + size + " Wstaw statek innej wielkości.");
                    }
                }
                else
                {
                    Console.WriteLine("Wstawiłeś złą wielkość statku, wybierz poprawną wielkość statku.");
                }

            }
            else
            {
                Console.WriteLine("Statek jest za wielki, jego szerokość i wysokość są naraz większe od 1 pola, spróbuj ponownie.");
            }

            if (!ship_added)
            {
                Console.WriteLine("Kliknij cokolwiek, aby kontynuować");
                Console.ReadKey();
            }
            Console.Clear();
        }
    }

    public void DisplayHidden()
    {
        Console.Write("    ");
        Console.ResetColor();

        for (int i = 0; i < 10; i++)
        {
            Console.Write($"|{(char)(i + 'A')}|");
        }
        Console.WriteLine();

        for (int i = 0; i < 10; i++)
        {
            Console.ResetColor();
            Console.Write($"|{i + 1}|".PadLeft(4));
            for (int j = 0; j < 10; j++)
            {
                switch (value[i, j])
                {
                    case CellState.Miss:
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("|O|");
                        break;
                    case CellState.Hit:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
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
            Console.ResetColor();
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

    HitState Try_Destroy_Cell(int x, int y)
    {
        if (GetCellAt(x, y) == CellState.Ship)
        {
            value[y, x] = CellState.Hit;
            return HitState.Hit;
        }
        else if (GetCellAt(x, y) == CellState.Miss || GetCellAt(x, y) == CellState.Hit)
        {
            return HitState.Occupied;
        }
        value[y, x] = CellState.Miss;
        return HitState.Miss;
    }

    public Board()
    {
        restart();
    }

    public static string Logo = @"
  ____  _____  _    _____  _  __ ___
 / ___||_   _|/ \  |_   _|| |/ /|_ _|
 \___ \  | | / _ \   | |  | ' /  | |
  ___) | | |/ ___ \  | |  | . \  | |
 |____/  |_/_/   \_\ |_|  |_|\_\ |_|
";
}


