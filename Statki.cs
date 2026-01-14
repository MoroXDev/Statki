using System;
using System.Security.Cryptography;

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
}
