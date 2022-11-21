using System.Linq;
using System.Text.RegularExpressions;

public static class Globals
{
    public static bool debug = false;
}

namespace Day17
{
    public class Bounds
    {
        public int xmin;
        public int xmax;
        public int ymin;
        public int ymax;
        public Bounds(int a, int b, int c, int d)
        {
            xmin = a;
            xmax = b;
            ymin = c;
            ymax = d;
        }
    }

    class Program
    {
        // Part 1:
        // Use math to find ymax since X and Y coordinates are independent.
        // Max vy to get into target area from the origin is the bottom of the target area (bounds.ymin) in one time step.
        // The previous time step will be one less in magnitude, crossing y=0 in the downward direction.
        // Since acceleration is constant, vy0 = the same magnitude of velocity in the opposite (upward) direction.
        static private int calcYMax(Bounds bounds)
        {
            int ymax = 0;
            int vy0 = (Math.Abs(bounds.ymin) - 1);
            foreach (int v in Enumerable.Range(1,vy0))    // or ymax = sum(1..vy0) = vy0 * (vy0 + 1) / 2
                ymax += v;
            return ymax;
        }

        // Check if in target bounds
        static private bool isInTarget(int x, int y, Bounds bounds)
        {
            if (x >= bounds.xmin && x <= bounds.xmax && y >= bounds.ymin && y <= bounds.ymax)
                return true;
            else
                return false;
        }

        // Check if missed target and is out of bounds
        static private bool isOutOfBounds(int x, int y, int vx, Bounds bounds)
        {
            if ((x > bounds.xmax) || (vx == 0 && y < bounds.ymin))
                return true;
            else
                return false;
        }

        static private bool simulation(int vx, int vy, Bounds bounds)
        {
            int x = 0;
            int y = 0;
            int t = 0;
            
            while (true)
            {
                if (Globals.debug)
                    Console.WriteLine($"t: {t}, x: {x}, y: {y}, vx: {vx}, vy: {vy}");

                x += vx;
                y += vy;

                if (vx > 0)
                    vx -= 1;
                else if (vx < 0)
                    vx += 1;

                vy -= 1;
                t++;

                if (isInTarget(x, y, bounds))
                {
                    if (Globals.debug)
                        Console.WriteLine($"Target hit at t: {t}, x: {x}, y: {y}, vx: {vx}, vy: {vy}");
                    return true;
                }

                if (isOutOfBounds(x, y, vx, bounds))
                {
                    if (Globals.debug)
                        Console.WriteLine($"Missed target with t: {t}, x: {x}, y: {y}, vx: {vx}, vy: {vy}");
                    return false;
                }

                if (t > 1000)
                {
                    if (Globals.debug)
                        Console.WriteLine($"Unknown error. Simulation timed out with t: {t}, x: {x}, y: {y}, vx: {vx}, vy: {vy}");
                    return false;
                }
            }
            
        }

        // Main program entry point
        static public void Main(string[] args)
        {
            // Obtain input from file
            string line;
            string[] lines;
            if (Globals.debug)
                lines = File.ReadAllLines("input_test");
            else
                lines = File.ReadAllLines("input");
            line = lines[0];
            if (Globals.debug)
                Console.WriteLine($"Input: {line}");

            // Parse input
            string pattern = @"(target area: x=)(-?\d+)(..)(-?\d+)(, y=)(-?\d+)(..)(-?\d+)";
            Regex rg = new Regex(pattern);
            MatchCollection matches = rg.Matches(line);
            var xmin = Convert.ToInt32(matches[0].Groups[2].Value);
            var xmax = Convert.ToInt32(matches[0].Groups[4].Value);
            var ymin = Convert.ToInt32(matches[0].Groups[6].Value);
            var ymax = Convert.ToInt32(matches[0].Groups[8].Value);

            // Part 1: Initialize variables
            Bounds bounds = new Bounds(xmin, xmax, ymin, ymax);
            if (Globals.debug)
                Console.WriteLine($"Target Area: xmin={bounds.xmin}, xmax={bounds.xmax}, ymin={bounds.ymin}, ymax={bounds.ymax}");
            var part1 = 0;

            // Part 1: Math
            part1 = calcYMax(bounds);

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            List<Tuple<int, int>> validVelocities = new List<Tuple<int, int>>();
            int part2 = 0;

            // Part 2: Main program loop
            for (int vx=0; vx <= bounds.xmax; vx++)
            {
                for (int vy=Math.Abs(bounds.ymin); vy >= bounds.ymin; vy--)
                {
                    //Console.WriteLine($"candidate: (vx0,vy0) = ({vx},{vy})");
                    if (simulation(vx, vy, bounds))
                    {
                        validVelocities.Add(Tuple.Create(vx, vy));
                        if (Globals.debug)
                            Console.WriteLine($"Valid initial velocity: ({vx},{vy})");
                    }
                }
            }

            part2 = validVelocities.Count;

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}