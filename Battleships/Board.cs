using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Board
    {
        public char[,] array;

        public Board()
        {
            array = new char[10, 10];
            InitializeBoards();
        }

        public void InitializeBoards()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    array[i, j] = '-';
                }
            }
        }

        public void Display()
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)(i + 65) + " ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool CanPlaceShip(int x, int y, int size, int direction)
        {
            if (direction == 0)
            {
                if (x + size > 10)
                    return false;
                for (int i = x; i < x + size; i++)
                {
                    if (array[i, y] != '-')
                        return false;
                }
            }
            else
            {
                if (y + size > 10)
                    return false;
                for (int j = y; j < y + size; j++)
                {
                    if (array[x, j] != '-')
                        return false;
                }
            }

            for (int i = x - 1; i <= x + size; i++)
            {
                for (int j = y - 1; j <= y + size; j++)
                {
                    if (i >= 0 && i < 10 && j >= 0 && j < 10)
                    {
                        if (array[i, j] != '-')
                            return false;
                    }
                }
            }
            return true;
        }

        public void PlaceShip(int x, int y, int size, int direction)
        {
            if (direction == 0)
            {
                for (int i = x; i < x + size; i++)
                {
                    array[i, y] = 'S';
                }
            }
            else
            {
                for (int j = y; j < y + size; j++)
                {
                    array[x, j] = 'S';
                }
            }
        }
    }
}
