// Jaden Olvera, 11-4-25, Lab 9 - Maze 2

Console.Clear();
Console.WriteLine("Welcome to a Maze Game!");
Console.WriteLine("Navigate a maze with your arrow keys and try to reach the \"*\"!");
Console.WriteLine("Avoid the bad guys (%) and collect the coins (^) for bonus points!");
Console.Write("Press enter to start playing!  ");
Console.ReadKey();

// Clean up console for printing the map
Console.Clear();

// Get the map from the file and print it
string[] mapRows = File.ReadAllLines("map.txt");
foreach (string row in mapRows)
{
    Console.WriteLine(row);
}
Console.WriteLine("Navigate with your arrow keys to reach the \"*\"!");
Console.WriteLine("Avoid the bad guys (%) and collect the coins (^) for bonus points!");