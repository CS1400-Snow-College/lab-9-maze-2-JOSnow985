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
            if (TryMove(currentCoordinates[0], currentCoordinates[1] - 1, mapCharArray) == true)
                currentCoordinates[1]--;
            break;
        case ConsoleKey.DownArrow:
            if (TryMove(currentCoordinates[0], currentCoordinates[1] + 1, mapCharArray) == true)
                currentCoordinates[1]++;
            break;
        case ConsoleKey.LeftArrow:
            if (TryMove(currentCoordinates[0] - 1, currentCoordinates[1], mapCharArray) == true)
                currentCoordinates[0]--;
            break;
        case ConsoleKey.RightArrow:
            if (TryMove(currentCoordinates[0] + 1, currentCoordinates[1], mapCharArray) == true)
                currentCoordinates[0]++;
            break;
        default:
            break;
    }
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
static bool TryMove(int targetX, int targetY, char[][] grid)
{
    if (targetX < 0)
        return false;
    else if (targetX > grid[0].Length - 1)
        return false;
    else if (targetY < 0)
        return false;
    else if (targetY > grid.Length - 1)
        return false;
    else if (!" #$%^".Contains(grid[targetY][targetX]))
        return false;
    else
        return true;
}