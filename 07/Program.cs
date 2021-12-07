public static class Globals
{
    public static bool debug = false;
}

namespace Day07
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

            // Parse input
            var input = lines[0].Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            // Part 1: Calculate min fuel per final position @ constant fuel rate per step
            var max = input.Max();
            var measurements = new int[max+1];
            for (int i = 0; i <= max; i++)
                measurements[i] = input.Select(n => Math.Abs(n-i)).Sum();
            if (Globals.debug)
            {
                for (int i = 0; i <= max; i++)
                    Console.WriteLine($"{i}: {measurements[i]}");
            }

            // Part 1: Calculate results
            int part1 = measurements.Min();
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Calculate min fuel per final position @ linear fuel rate per step
            // Generalized formula: arithmetic series formula is sum S_n = (n/2) * [2 * a_1 + (n-1)d]
            // Application: e.g., n = 4 steps requires 1 + 2 + 3 + 4 fuel = 10 fuel
            //     with first term a_1 = 1, common difference d = 1, n = number of terms in the sum,
            //     S_n = [n^2 + n]/2 = [4^2 + 4]/2 = [16 + 4]/2 = 10
            measurements = new int[max+1];
            for (int i = 0; i <= max; i++)
            {
                measurements[i] = input.Select(n => (Math.Abs(n-i)*Math.Abs(n-i) + Math.Abs(n-i))/2).ToArray().Sum();
            }
            if (Globals.debug)
            {
                for (int i = 0; i <= max; i++)
                    Console.WriteLine($"{i}: {measurements[i]}");
            }

            // Part 2: Calculate results
            int part2 = measurements.Min();
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}