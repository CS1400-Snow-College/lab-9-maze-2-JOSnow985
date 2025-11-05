// Jaden Olvera, 11-4-25, Lab 9 - Maze 2
using System.Diagnostics;

Console.Clear();
Console.WriteLine("Welcome to... The MAZE\u2122!");
Console.WriteLine("Chatwyn, the Grand Portent Teller has tasked you with recovering magic gems from The MAZE\u2122!");
Console.WriteLine();
Console.WriteLine("Move around the maze with your arrow keys!");
Console.WriteLine("Avoid the bad guys (%) and collect the magic coins (^) to bring the gate down!");
Console.WriteLine("When the gate is down, get the gems ($) and use the magic exit (#) to escape!");
Console.WriteLine();
Console.Write("Press enter to start playing!  ");
Console.ReadKey();

//Get a timer started to track how long it takes the user
var runningTimer = Stopwatch.StartNew();

// Clean up console for printing the map
Console.Clear();

// Get the map from the file and print it
char[][] mapCharArray = File.ReadAllLines("map.txt").Select(fileStrings => fileStrings.ToCharArray()).ToArray();
foreach (char[] line in mapCharArray)
{
    foreach (char character in line)
        Console.Write(character);
    Console.WriteLine();
}
Console.WriteLine("Move around the maze with your arrow keys!");
Console.WriteLine("Avoid the bad guys (%) and collect the magic coins (^) to bring the gate down!");
Console.WriteLine("When the gate is down, get the gems ($) and use the magic exit (#) to escape!");

// Set up an int array to hold the initial x and y coordinates
int[] currentCoordinates = [0, 0];

// Some variables for tracking things during the game
ConsoleKey lastKey;
bool gameWon = false;
int playerScore = 0;

// Main loop, playing on the maze already printed
do {
    // Draw the score and timer to the right of the map
    Console.SetCursorPosition(mapCharArray[0].Length + 3, 1);
    Console.Write($"Score: {playerScore}");

    Console.SetCursorPosition(mapCharArray[0].Length + 3, 2);
    Console.Write($"Time: {runningTimer.Elapsed:mm\\:ss\\.f}");

    // If the player's score is enough, remove the gates blocking the gems and exit
    if (playerScore >= 1000)
        GateCrasher(mapCharArray);

    // Even if GateCrasher hasn't run yet, we need to move the cursor to where the game and the player needs it to be
    Console.SetCursorPosition(currentCoordinates[0],currentCoordinates[1]);


    // Wait for user to press a key, then decide if that's a possible move or not
    lastKey = Console.ReadKey(true).Key;
    switch (lastKey)
    {
        case ConsoleKey.UpArrow:
            if (TryMove(EntityType.player, currentCoordinates[0], currentCoordinates[1] - 1, mapCharArray) == true)
                currentCoordinates[1]--;
            break;
        case ConsoleKey.DownArrow:
            if (TryMove(EntityType.player, currentCoordinates[0], currentCoordinates[1] + 1, mapCharArray) == true)
                currentCoordinates[1]++;
            break;
        case ConsoleKey.LeftArrow:
            if (TryMove(EntityType.player, currentCoordinates[0] - 1, currentCoordinates[1], mapCharArray) == true)
                currentCoordinates[0]--;
            break;
        case ConsoleKey.RightArrow:
            if (TryMove(EntityType.player, currentCoordinates[0] + 1, currentCoordinates[1], mapCharArray) == true)
                currentCoordinates[0]++;
            break;
        default:
            break;
    }

    // Bad Guy's turn to move, then return cursor to current location
    badGuyMove(mapCharArray);
    Console.SetCursorPosition(currentCoordinates[0],currentCoordinates[1]);

    // If the player finds a score object, update score, remove it from the map array, print over it
    if ("$^".Contains(mapCharArray[Console.CursorTop][Console.CursorLeft]) == true)
    {
        if (mapCharArray[Console.CursorTop][Console.CursorLeft] == '^')
            playerScore += 100;
        else if (mapCharArray[Console.CursorTop][Console.CursorLeft] == '$')
            playerScore += 200;
        mapCharArray[Console.CursorTop][Console.CursorLeft] = ' ';
        Console.Write(' ');
        Console.SetCursorPosition(currentCoordinates[0],currentCoordinates[1]);
    }

    // If the cursor is on top of the # (exit) or % (bad guy), exit loop
    if ("#%".Contains(mapCharArray[Console.CursorTop][Console.CursorLeft]) == true)
    {
        if (mapCharArray[Console.CursorTop][Console.CursorLeft] == '#')
            gameWon = true;
        else if (mapCharArray[Console.CursorTop][Console.CursorLeft] == '%')
            gameWon = false;
        Thread.Sleep(500);
        break;
    }
} while (lastKey != ConsoleKey.Escape);

Console.Clear();

// Only print win screen if the user won
if (gameWon == true)
{
    // ASCII Art credit: patorjk.com/software/taag/
    string[] winScreen = File.ReadAllLines("winscreen.txt");
    foreach (string row in winScreen)
    {
        Console.WriteLine(row);
    }
    Console.WriteLine();

    //Stop our timer and print how long it took the user
    runningTimer.Stop();

    if (playerScore == 2800)
        Console.WriteLine("You return from The MAZE\u2122 with the magic gems!");
    else
        Console.WriteLine("You left some magic gems behind, for some reason, but you return from The MAZE\u2122 anyway.");
    Console.WriteLine("However, Chatwyn, the Grand Portent Teller, doesn't remember sending you to get gems...");
    Console.WriteLine();
    Console.WriteLine($"It only took you {runningTimer.Elapsed:mm\\:ss\\.f}!");
    Console.WriteLine($"Your score was: {playerScore}, great job!");
    Console.WriteLine("The MAZE\u2122 has been defeated...");
}
else if (gameWon == false)
{
    // ASCII Art credit: patorjk.com/software/taag/
    string[] winScreen = File.ReadAllLines("losescreen.txt");
    foreach (string row in winScreen)
    {
        Console.WriteLine(row);
    }
    Console.WriteLine();

    runningTimer.Stop();
    Console.WriteLine("Oh no! A bad guy got you!");
    Console.WriteLine("Your magic coins have been confiscated and returned to The MAZE\u2122, but at least you're alive!");
    Console.WriteLine();
    Console.WriteLine($"You were in The MAZE\u2122 for {runningTimer.Elapsed:mm\\:ss\\.f}");
    Console.WriteLine($"Your score was: {playerScore}");
    Console.WriteLine("The MAZE\u2122 will wait for your return...");
}
else
{
    runningTimer.Stop();
    Console.WriteLine($"You were in The MAZE\u2122 for {runningTimer.Elapsed:mm\\:ss\\.f}");
    Console.WriteLine($"Your score was: {playerScore}");
    Console.WriteLine("The MAZE\u2122 will wait for your return...");
}

// Method for checking if an attempted move is valid
static bool TryMove(EntityType entity, int targetX, int targetY, char[][] grid)
{
    if (targetX < 0)
        return false;
    else if (targetX > grid[0].Length - 1)
        return false;
    else if (targetY < 0)
        return false;
    else if (targetY > grid.Length - 1)
        return false;
    else if (entity == EntityType.player && !" #$%^".Contains(grid[targetY][targetX]))
        return false;
    else if (entity == EntityType.nonplayer && !" #$%".Contains(grid[targetY][targetX]))
        return false;
    else
        return true;
}

static void GateCrasher(char[][] map)
{
    var gatesToCrash = charLocator(map, '|');
    foreach (var(row, column) in gatesToCrash)
    {
        Console.SetCursorPosition(column, row);
        Console.Write(' ');
        map[row][column] = ' ';
    }
}

static void badGuyMove(char[][] array)
{
    // Get an array of all bad guys on the map
    var badGuyLocations = charLocator(array, '%');

    // Give every bad guy on the map a chance to stay still or move
    foreach (var (row, column) in badGuyLocations)
    {
        int currentRow = row;
        int currentColumn = column;

        // Make a list of the potential destinations for the bad guy
        var viableDestinations = new List<(int row, int column)>();

        for (int rowToCheck = -1; rowToCheck <= 1; rowToCheck++)
        {
            for (int columnToCheck = -1; columnToCheck <= 1; columnToCheck++)
            {
                int targetRow = currentRow + rowToCheck;
                int targetColumn = currentColumn + columnToCheck;

                // Move method checks for 
                if (TryMove(EntityType.nonplayer, targetColumn, targetRow, array) == false)
                    continue;

                viableDestinations.Add((targetRow, targetColumn));
            }
        }

        // Pull a random set of coordinates out of the list of possible ones
        var randomized = new Random();
        var chosenDestination = viableDestinations[randomized.Next(0, viableDestinations.Count)];
        int rowMovingTo = chosenDestination.row;
        int columnMovingTo = chosenDestination.column;

        // Erase old % and remove it from the map array
        Console.SetCursorPosition(currentColumn, currentRow);
        Console.Write(' ');
        array[currentRow][currentColumn] = ' ';

        // Print new % and add it to the map array
        Console.SetCursorPosition(columnMovingTo, rowMovingTo);
        Console.Write('%');
        array[rowMovingTo][columnMovingTo] = '%';
    }
}

static (int row, int column)[] charLocator(char[][] grid, char target)
{
    var locations = new List<(int row, int column)>();

    for (int rowIndex = 0; rowIndex < grid.Length; rowIndex++)
    {
        char[] row = grid[rowIndex];
        for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
        {
            if (row[columnIndex] == target)
            {
                locations.Add((rowIndex, columnIndex));
            }
        }
    }

    return locations.ToArray();
}

enum EntityType {player, nonplayer}