using System;
using System.IO;

internal class Program
{
    static int[,] fleetGrid;
    static int[,] opponentFleetGrid;
    static char[,] targetTracker;
    static char[,] opponentTargetTracker;
    static int boatsPutDown;
    static int userHits;
    static int userMisses;
    static int opponentHits;
    static int opponentMisses;
    static string filename = "game.txt";
    static Random random = new Random();

    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the BattleBoats Game!");
        Console.WriteLine("What would you like to do? \n1. New Game \n2. Resume Game \n3. Instructions \n4. Quit Game \n");
        int userInput = Convert.ToInt32(Console.ReadLine());

        switch (userInput)
        {
            case 1:
                NewGame();
                break;

            case 2:
                ResumeGame();
                break;

            case 3:
                Instructions();
                break;

            case 4:
                ExitGame();
                break;
        }
    }

    static void FleetGrid()
    {
        fleetGrid = new int[8, 8];
        targetTracker = new char[8, 8];
        opponentTargetTracker = new char[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                fleetGrid[i, j] = 0;
                targetTracker[i, j] = ' ';
                opponentTargetTracker[i, j] = ' ';
            }
        }
    }

    static void ShowGrid()
    {
        Console.Clear();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write(fleetGrid[i, j] + "\t");
            }
            Console.WriteLine();
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write(targetTracker[i, j] + "\t");
            }
            Console.WriteLine();
        }    
    }

    static void randomCoordinates()
    {
        while (boatsPutDown < 5)
        {
            int x = random.Next(0, 7);
            int y = random.Next(0, 7);

            if (opponentFleetGrid[x,y] == 0)
            {
                opponentFleetGrid[x, y] = 1;
                boatsPutDown++;
            }

        }
    }

    static void OpponentFleetGrid()
    {
        opponentFleetGrid = new int[8, 8];
        randomCoordinates();
        ShowGrid();
    }

    static void PlayerGuess()
    {
        Console.WriteLine("Guess the x-coordinate: ");
        int xGuess = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Guess the y-coordinate: ");
        int yGuess = Convert.ToInt32(Console.ReadLine());

        if (opponentFleetGrid[xGuess, yGuess] == 1)
        {
            Console.WriteLine("Hit!");
            userHits++;
            targetTracker[xGuess, yGuess] = 'H';
        }

        else
        {
            Console.WriteLine("Miss!");
            userMisses++;
            targetTracker[xGuess, yGuess] = 'M';
        }
    }

    static void OpponentGuess()
    {
        int xGuess = random.Next(0, 7);
        int yGuess = random.Next(0, 7);

        if (fleetGrid[xGuess,yGuess] == 1)
        {
            Console.WriteLine("You have been hit!");
            opponentHits++;
            opponentTargetTracker[xGuess, yGuess] = 'H';
        }

        else
        {
            Console.WriteLine("Computer misses.");
            opponentMisses++;
            opponentTargetTracker[xGuess, yGuess] = 'M';
        }
    }

    static void SaveGame()
    {
        StreamWriter sw = new StreamWriter(filename);

        sw.WriteLine("User Fleet Grid: ");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                sw.Write(fleetGrid[i, j] + "\t");
            }
            sw.WriteLine();
        }

        sw.WriteLine("User Target Tracker: ");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                sw.Write(targetTracker[i, j] + "\t");
            }
            sw.WriteLine();
        }

        sw.WriteLine("Opponent Fleet Grid: ");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                sw.Write(opponentFleetGrid[i, j] + "\t");
            }
            sw.WriteLine();
        }

        sw.WriteLine("Opponent Target Tracker: ");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                sw.Write(opponentTargetTracker[i, j] + "\t");
            }
            sw.WriteLine();
        }

        sw.Close();
    }

    static void NewGame()
    {
        FleetGrid();
        while (boatsPutDown < 5)
        {
            Console.WriteLine("Enter x coordinate: ");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter y coordinate: ");
            int y = Convert.ToInt32(Console.ReadLine());
            if (fleetGrid[x,y] == 0)
            {
                fleetGrid[x, y] = 1;
                boatsPutDown++;
            }
        }

        Console.WriteLine("All boats placed on the grid!");
        ShowGrid();
        OpponentFleetGrid();

        while (userHits < 5 && opponentHits < 5)
        {
            PlayerGuess();
            OpponentGuess();
            SaveGame();

            if (userHits == 5)
            {
                Console.WriteLine("You win!!!");
            }

            if (opponentHits == 5)
            {
                Console.WriteLine("You lose!");
            }
        }

    }

    static void LoadGame()
    {
        StreamReader sr = new StreamReader(filename);

        Console.WriteLine("Your fleet grid: ");
        for (int i = 0; i < 8; i++)
        {
            string[] rowStuff = sr.ReadLine().Split("\t");
            for (int j = 0; j < 8; j++)
            {
                fleetGrid[i, j] = Convert.ToInt32(rowStuff[j]);
                Console.Write(fleetGrid[i, j] + "\t");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Your target tracker: ");
        for (int i = 0; i < 8; i++)
        {
            string[] rowStuff = sr.ReadLine().Split("\t");
            for (int j = 0; j < 8; j++)
            {
                targetTracker[i, j] = Convert.ToChar(rowStuff[j]);
                Console.Write(fleetGrid[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    static void ResumeGame()
    {
        LoadGame();
        OpponentFleetGrid();

        while (userHits < 5 && opponentHits < 5)
        {
            PlayerGuess();
            OpponentGuess();
            SaveGame();

            if (userHits == 5)
            {
                Console.WriteLine("You win!!!");
            }

            if (opponentHits == 5)
            {
                Console.WriteLine("You lose!");
            }
        }

    }

    static void Instructions()
    {
        Console.WriteLine("1. First you must place all five of your ships on different coordinates of your fleet grid.");
        Console.WriteLine("2. Then you must guess on the fleet grid the opponent's ships are.");
        Console.WriteLine("3. If you have a hit, then the target tracker will update with a \"H\" to show a hit");
        Console.WriteLine("4. If you miss, then the target tracker will update with a \"M\" to show a miss");
        Console.WriteLine("5. The game wil alternate between you and the oppponent until either you or the oppponent loses all of their ships, which determines a winner.");
    }

    static void ExitGame()
    {
        Console.WriteLine("Thanks for playing! See you next time!");
        Environment.Exit(0);
    }
}
