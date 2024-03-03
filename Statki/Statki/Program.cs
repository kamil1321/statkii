using System;

class Plansza
{
    public char[,] tablica;

    public Plansza()
    {
        tablica = new char[10, 10];
        InitializeBoards();
    }

    public void InitializeBoards()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tablica[i, j] = '-';
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write(tablica[i, j] + " ");
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
                if (tablica[i, y] != '-')
                    return false;
            }
        }
        else
        {
            if (y + size > 10)
                return false;
            for (int j = y; j < y + size; j++)
            {
                if (tablica[x, j] != '-')
                    return false;
            }
        }

        for (int i = x - 1; i <= x + size; i++)
        {
            for (int j = y - 1; j <= y + size; j++)
            {
                if (i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if (tablica[i, j] != '-')
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
                tablica[i, y] = 'S';
            }
        }
        else
        {
            for (int j = y; j < y + size; j++)
            {
                tablica[x, j] = 'S';
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
                    if (tablica[i, j] == '-')
                    {
                        tablica[i, j] = 'O';
                    }
                }
            }
        }
    }
}

class Gracz
{
    public Plansza PlanszaStatkow { get; }
    public Plansza PlanszaCelu { get; }
    public int LiczbaStatkow { get; private set; }
    public int LiczbaTrafien { get; private set; }

    public Gracz()
    {
        PlanszaStatkow = new Plansza();
        PlanszaCelu = new Plansza();
        LiczbaStatkow = 20;
        LiczbaTrafien = 0;
    }

    public void FireShot(Gracz przeciwnik, int x, int y)
    {
        char[,] shipsBoard = przeciwnik.PlanszaStatkow.tablica;
        char[,] targetBoard = PlanszaCelu.tablica;

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
            przeciwnik.LiczbaStatkow--;
            przeciwnik.LiczbaTrafien++;
            Game.SwitchPlayer();
            if (CheckShipSunk(shipsBoard, x, y))
            {
                PlanszaCelu.MarkSurroundingAsMiss(x, y);
            }
        }
        else
        {
            Console.WriteLine("Pudło!");
            targetBoard[x, y] = 'O';
        }

        if (CheckShipSunk(shipsBoard, x, y))
        {
            PlanszaStatkow.MarkSurroundingAsMiss(x, y);
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
            PlanszaStatkow.MarkSurroundingAsMiss(x, y);
        }

        return isSunk;
    }
}

class Game
{
    static Gracz player1;
    static Gracz player2;
    static int currentPlayer = 1;

    static void Main(string[] args)
    {
        player1 = new Gracz();
        player2 = new Gracz();

        PlaceShips(player1);
        PlaceShips(player2);

        while (player1.LiczbaStatkow > 0 && player2.LiczbaStatkow > 0)
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
        Console.WriteLine($"Gracz {(player1.LiczbaStatkow == 0 ? 2 : 1)} wygrał!");
        Console.ReadLine();
    }

    static void PlaceShips(Gracz gracz)
    {
        Plansza plansza = gracz.PlanszaStatkow;
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
        player2.PlanszaCelu.Display();

        Console.WriteLine("\nPlansza celu gracza 2:");
        player1.PlanszaCelu.Display();
    }

    public static void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }
}
