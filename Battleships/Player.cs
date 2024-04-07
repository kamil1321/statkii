using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{

    class Player
    {
        public Board ShipsBoard;
        public Board TargetBoard;
        public int NumberOfShips;
        public int NumberOfTarget;

        public Player()
        {
            ShipsBoard = new Board();
            TargetBoard = new Board();
            NumberOfShips = 20;
            NumberOfTarget = 0;
        }

        public void FireShot(Player opponent, int x, int y)
        {
            char[,] shipsBoard = opponent.ShipsBoard.array;
            char[,] targetBoard = TargetBoard.array;

            if (targetBoard[x, y] != '-')
            {
                Console.WriteLine("Strzelałeś już w tym miejscu!");
                Game.SwitchPlayer();
                return;
            }

            if (shipsBoard[x, y] == 'S')
            {
                Console.WriteLine("Trafiony!");
                targetBoard[x, y] = 'X';
                opponent.NumberOfShips--;
                opponent.NumberOfTarget++;
                Game.SwitchPlayer();
            }
            else
            {
                Console.WriteLine("Pudło!");
                targetBoard[x, y] = 'O';
            }


        }
    }
}
