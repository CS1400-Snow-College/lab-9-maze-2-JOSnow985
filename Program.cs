// Jaden Olvera, 11-4-25, Lab 9 - Maze 2

Console.Clear();
Console.WriteLine("Welcome to a Maze Game!");
Console.WriteLine("Navigate a maze with your arrow keys and try to reach the \"#\"!");
Console.WriteLine("Avoid the bad guys (%) and collect the coins (^) to unlock the vault!");
Console.Write("Press enter to start playing!  ");
Console.ReadKey();

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
Console.WriteLine("Navigate with your arrow keys to reach the \"#\"!");
Console.WriteLine("Avoid the bad guys (%) and collect the coins (^) to unlock the vault!");

// Set up an int array to hold the initial x and y coordinates
int[] currentCoordinates = [0, 0];
Console.SetCursorPosition(currentCoordinates[0],currentCoordinates[1]);

// Get ready to keep track of what the last key pressed was
ConsoleKey lastKey;

// Create a bool to track win or loss, true for win false for loss
bool gameWon = false;

// Main loop, playing on the maze already printed
do {
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
    badGuyMove(mapCharArray);
    Console.SetCursorPosition(currentCoordinates[0],currentCoordinates[1]);

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

// Find bad guys in array, 
// check spaces around bad guys, 
// randomly select viable spot, 
// erase bad guy from current position in array and on board
// write % to new position in array and on board
// Might not need to return cursor position to player's location

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