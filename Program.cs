using Statki;

char[,] playerBoard = new char[10, 10];

for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        playerBoard[i, j] = '~';
    }
}

bool isExit = false;

while (!isExit)
{
    
        ConsoleKeyInfo key = Console.ReadKey();
    Console.WriteLine();
    if (key.KeyChar == 'e')
        isExit = true;
}
