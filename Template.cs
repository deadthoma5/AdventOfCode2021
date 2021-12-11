public static class Globals
{
    public static bool debug = true;
}

namespace DayXX
{
    class Program
    {
        // Main program entry point
        static public void Main(string[] args)
        {
            // Obtain input from file
            string[] lines;
            if (Globals.debug)
                lines = File.ReadAllLines("input_test");
            else
                lines = File.ReadAllLines("input");

            // Part 1: Main program loop
 
            // Part 1: Display results
            int part1 = 0;
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Display results
            int part2 = 0;           
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
