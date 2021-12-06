public static class Globals
{
    public static bool debug = false;
}

namespace Day06
{
    class Program
    {
        // Process initial state from input
        static public long[] InitDueDays(int size, int[] seed)
        {
            long[] days = new long[size];
            foreach (int d in seed)
                days[d]++;
            return days;
        }

        // Elapse a day. Fish with ready to spawn get moved up 6+1 days, new fish are added at 8+1 days. Zero out completed days since we'll sum the array values later.
        static public long[] Evolve(long[] dueDays, int maxDays, int period_oldfish, int period_newfish)
        {
            for (int d = 0; d < maxDays; d++)
            {                
                if (Globals.debug)
                    Console.WriteLine($"Index {d}, Count: {dueDays[d]}");
                dueDays[d + period_oldfish] += dueDays[d]; // after spawning, requeue up today's fish 6+1 days later
                dueDays[d + period_newfish] += dueDays[d]; // queue up new fish 8+1 days later
                dueDays[d] = 0;
            }
            return dueDays;
        }

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
            var seed = lines[0].Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            // Initialize common problem paramters
            int period_oldfish = 6 + 1; // 6 days for old fish + 1 since 0th day is included
            int period_newfish = 8 + 1; // 8 days for new fish + 1 since 0th day is included

            // Part 1: Initialize problem parameter variables and data structure
            int maxDays = 80;
            long[] dueDays = InitDueDays(maxDays + period_newfish, seed);

            // Part 1: Main program loop
            dueDays = Evolve(dueDays, maxDays, period_oldfish, period_newfish);

            // Part 1: Calculate results
            long part1 = dueDays.Sum();;
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Reinitialize problem parameter variables and data structure
            maxDays = 256;
            dueDays = InitDueDays(maxDays + period_newfish, seed);

            // Part 2: Main program loop
            dueDays = Evolve(dueDays, maxDays, period_oldfish, period_newfish);

            // Part 2: Calculate results
            long part2 = dueDays.Sum();
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}