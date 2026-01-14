namespace Statki
{
    class Display
    {
        static void Run(char[,] playerBoard)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(playerBoard[i, j]);
                }
                Console.WriteLine();
            }
        }
    }

}
