using System;

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
        for (int i = 0; i < 10; i++)
        {
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

    public void MarkSurroundingAsMiss(int x, int y)
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if (array[i, j] == '-')
                    {
                        array[i, j] = 'O';
                    }
                }
            }
        }
    }
}

class Player
{
    public Board ShipsBoard { get; }
    public Board TargetBoard { get; }
    public int NumberOfShips { get; private set; }
    public int NumberOfTarget { get; private set; }

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
            if (CheckShipSunk(shipsBoard, x, y))
            {
                TargetBoard.MarkSurroundingAsMiss(x, y);
            }
        }
        else
        {
            Console.WriteLine("Pudło!");
            targetBoard[x, y] = 'O';
        }

        if (CheckShipSunk(shipsBoard, x, y))
        {
            ShipsBoard.MarkSurroundingAsMiss(x, y);
        }
    }

    private bool CheckShipSunk(char[,] shipsBoard, int x, int y)
    {
        if (x < 0 || x >= 10 || y < 0 || y >= 10)
            return false;

        char shipType = shipsBoard[x, y];
        if (shipType == '-')
            return false;

        int size = 0;

        for (int i = x; i < 10 && shipsBoard[i, y] == shipType; i++)
        {
            size++;
        }

        for (int i = x - 1; i >= 0 && shipsBoard[i, y] == shipType; i--)
        {
            size++;
        }

        for (int j = y + 1; j < 10 && shipsBoard[x, j] == shipType; j++)
        {
            size++;
        }

        for (int j = y - 1; j >= 0 && shipsBoard[x, j] == shipType; j--)
        {
            size++;
        }

        bool isSunk = size == (shipType - '0');

        if (isSunk)
        {
            ShipsBoard  .MarkSurroundingAsMiss(x, y);
        }

        return isSunk;
    }
}

class Game
{
    static Player player1;
    static Player player2;
    static int currentPlayer = 1;

    static void Main(string[] args)
    {
        player1 = new Player();
        player2 = new Player();

        PlaceShips(player1);
        PlaceShips(player2);

        while (player1.NumberOfShips > 0 && player2.NumberOfShips > 0)
        {
            DisplayBoards();

            Console.WriteLine($"Gracz {currentPlayer} kolejka");
            Console.WriteLine("Wprowadź współrzędne strzału (np. A5): ");
            string input = Console.ReadLine().ToUpper();
            int x = -1;
            int y = -1;

            if (input.Length > 1 && input[0] >= 'A' && input[0] <= 'J' && int.TryParse(input.Substring(1), out y) && y >= 1 && y <= 10)
            {
                x = (int)input[0] - 65;
                y--;
                if (currentPlayer == 1)
                    player1.FireShot(player2, x, y);
                else
                    player2.FireShot(player1, x, y);
                SwitchPlayer();
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
        Board plansza = player.ShipsBoard;
        Random rand = new Random();
        for (int i = 4; i >= 1; i--)
        {
            for (int j = 0; j < 5 - i; j++)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = rand.Next(10);
                    int y = rand.Next(10);
                    int direction = rand.Next(2);
                    if (plansza.CanPlaceShip(x, y, i, direction))
                    {
                        plansza.PlaceShip(x, y, i, direction);
                        placed = true;
                    }
                }
            }
        }
    }

    static void DisplayBoards()
    {
        Console.WriteLine("\nPlansza celu gracza 1:");
        player2.TargetBoard.Display();

        Console.WriteLine("\nPlansza celu gracza 2:");
        player1.TargetBoard.Display();
    }

    public static void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }
}
