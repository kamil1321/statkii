using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Ship
    {
        public static bool SetShip(Board board, int lenght)
        {
            bool placed = false;
            Console.WriteLine("Podaj współrzędne statku o długości " + lenght + ":");
            string input = Console.ReadLine().ToUpper();
            int x ;
            int y ;

            if (input.Length > 1 && input[0] >= 'A' && input[0] <= 'J' && int.TryParse(input.Substring(1), out y) && y >= 1 && y <= 10)
            {
                x = (int)input[0] - 65;
                y--;
                int direction = 0;
                if (lenght != 1)
                {
                    Console.WriteLine("Podaj kierunek statku (0 - pionowo, 1 - poziomo): ");
                    string directionInput = Console.ReadLine();
                    try
                    {
                        direction = int.Parse(directionInput);
                    }
                    catch
                    {
                        Console.WriteLine("Błędny kierunek! Spróbuj ponownie.");
                        return SetShip(board, lenght);
                    }

                }
                if (direction != 0 && direction != 1)
                {
                    Console.WriteLine("Błędny kierunek! Spróbuj ponownie.");
                    return SetShip(board, lenght);
                }
                placed = board.CanPlaceShip(x, y, lenght, direction);
                if (placed)
                {
                    board.PlaceShip(x, y, lenght, direction);
                }
                else
                {
                    Console.WriteLine("Nie można umieścić statku w tym miejscu! Spróbuj ponownie.");
                    return SetShip(board, lenght);
                }

            }
            else
            {
                Console.WriteLine("Błędne współrzędne! Podaj poprawne współrzędne.");
            }

            return placed;
        }
    }
}
