using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Game
    {
        static Player player1;
        static Player player2;
        static int currentPlayer = 1;

        static void Main(string[] args)
        {
            player1 = new Player();
            player2 = new Player();

            PlaceShips(player1);
            Console.Clear();
            PlaceShips(player2);
            Console.Clear();
            Console.WriteLine("Kliknij dowolny klawisz aby przejsc do tury pierwszego gracza...");
            Console.ReadLine();
            Console.Clear();

            while (player1.NumberOfShips > 0 && player2.NumberOfShips > 0)
            {
                if (currentPlayer == 1)
                    DisplayBoards(player1);
                else
                    DisplayBoards(player2);
                Console.WriteLine($"Gracz {currentPlayer} kolejka");
                Console.WriteLine("Wprowadź współrzędne strzału (np. A5): ");
                string input = Console.ReadLine().ToUpper();
                int x;
                int y;

                if (input.Length > 1 && input[0] >= 'A' && input[0] <= 'J' && int.TryParse(input.Substring(1), out y) && y >= 1 && y <= 10)
                {
                    x = (int)input[0] - 65;
                    y--;
                    int tempPlayer = currentPlayer;
                    if (currentPlayer == 1)
                        player1.FireShot(player2, x, y);
                    else
                        player2.FireShot(player1, x, y);
                    SwitchPlayer();
                    if (currentPlayer != tempPlayer)
                    {
                        Console.WriteLine($"Kliknij dowolny klawisz aby przejsc do tury gracza {currentPlayer}...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Kliknij dowolny klawisz aby kontynuować...");
                        Console.ReadLine();
                    }

                }
                else
                {
                    Console.WriteLine("Błędne współrzędne! Podaj poprawne współrzędne.");
                }
            }
            Console.WriteLine($"Gracz {(player1.NumberOfShips == 0 ? 2 : 1)} wygrał!");
            Console.ReadLine();
        }

        static void PlaceShips(Player player)
        {
            bool placed;
            Board board = player.ShipsBoard;

            placed = false;
            Console.Clear();
            board.Display();
            while (!placed)
            {
                placed = Ship.SetShip(board, 4);
            }

            for (int i = 0; i < 2; i++)
            {
                placed = false;
                Console.Clear();
                board.Display();
                while (!placed)
                {
                    placed = Ship.SetShip(board, 3);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                placed = false;
                Console.Clear();
                board.Display();
                while (!placed)
                {
                    placed = Ship.SetShip(board, 2);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                placed = false;
                Console.Clear();
                board.Display();
                while (!placed)
                {
                    placed = Ship.SetShip(board, 1);
                }
            }


        }

        static void DisplayBoards(Player shootingPlayer)
        {
            Console.Clear();
            Console.WriteLine("\nPlansza celu gracza:");
            shootingPlayer.TargetBoard.Display();

            Console.WriteLine("\nPlansza statków gracza:");
            shootingPlayer.ShipsBoard.Display();
        }

        public static void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }
    }
}
