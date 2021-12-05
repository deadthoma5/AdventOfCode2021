using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Numerics;

public static class Globals
{
    public static bool debug = false;
}

namespace Day05
{
    class Program
    {
        // Extract the four coordinates from each line of input
        static public List<Dictionary<string, int>> ParseInput(string[] lines)
        {
            var commands = new List<Dictionary<string, int>>();
            var r = new Regex("[0-9]+");
            foreach (string line in lines)
            {
                var parts = r.Matches(line);
                var command = new Dictionary<string, int>()
                {
                    {"x1", Convert.ToInt32(parts[0].Value)},
                    {"y1", Convert.ToInt32(parts[1].Value)},
                    {"x2", Convert.ToInt32(parts[2].Value)},
                    {"y2", Convert.ToInt32(parts[3].Value)},
                };
                commands.Add(command);
            }
            return commands;
        }

        // Translate initial and final coordinates into an equivalent unit movement vector with quantity of steps
        static public (Vector2, Vector2, int) ParseCommand(Dictionary<string, int> c, int part)
        {
            if (Globals.debug)
                Console.WriteLine($"Command: ({c["x1"]},{c["y1"]}) -> ({c["x2"]},{c["y2"]})");

            var initial = new Vector2(new float[] {c["x1"], c["y1"]});
            var final = new Vector2(new float[] {c["x2"], c["y2"]});
            var diff = final - initial;
            if (Globals.debug)
                Console.WriteLine($"Difference vector: ({diff.X}, {diff.Y})");

            var steps = (int)Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y));
            var heading = diff / steps;
            if ((part == 1) && (heading.Length() > 1))
            {
                steps = -1;
                if (Globals.debug)
                    Console.WriteLine($"Part 1 specified. Disabling diagonal movement.");
            }
            if (Globals.debug)
                Console.WriteLine($"Heading vector: ({heading.X}, {heading.Y}), Steps: {steps}");

            return (initial, heading, steps);
        }

        // Execute given movement command, incrementing cells' values along the path
        static public int[,] UpdateGrid(int[,] grid, Vector2 initial, Vector2 heading, int steps)
        {
            Vector2 position = initial;
            for (int i = 0; i <= steps; i++)
            {
                grid[(int)position.X, (int)position.Y]++;
                if (Globals.debug)
                    Console.WriteLine($"Incrementing grid at position: ({position.X}, {position.Y}), New grid value: {grid[(int)position.X, (int)position.Y]}");
                position += heading;
            }
            return grid;
        }

        // Determine number of cells that meet or exceed the given threshold (2)
        static public int EvaluateGrid(int[,] grid)
        {
            int threshold = 2, sum = 0;
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] >= threshold)
                        sum++;
                }
            }
            return sum;
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

            // Parse input into a list of movement commands
            var commands = ParseInput(lines);
            
            // Part 1: Build grid based on largest coordinates from commands
            var dim_x = Math.Max(commands.Max(c => c["x1"]), commands.Max(c => c["x2"])) + 1;
            var dim_y = Math.Max(commands.Max(c => c["y1"]), commands.Max(c => c["y2"])) + 1;
            var grid1 = new int[dim_x, dim_y];

            // Part 1: Main program loop
            foreach (var command in commands)
            {
                // Translate a movement command into unit vectors and quantity of steps
                var (initial, heading, steps) = ParseCommand(command, 1);

                // Execute command to move and increment traveled cells
                grid1 = UpdateGrid(grid1, initial, heading, steps);

                if (Globals.debug)
                    Console.WriteLine();
            }

            // Part 1: Calculate results
            int part1 = EvaluateGrid(grid1);
            Console.WriteLine($"Part 1: {part1}");
            if (Globals.debug)
                Console.WriteLine("\n####################\n");

            // Part 2: Build a new grid
            var grid2 = new int[dim_x, dim_y];

            // Part 2: Main program loop
            foreach (var command in commands)
            {
                // Translate a movement command into unit vectors and quantity of steps
                var (initial, heading, steps) = ParseCommand(command, 2);

                // Execute command to move and increment traveled cells
                grid2 = UpdateGrid(grid2, initial, heading, steps);

                if (Globals.debug)
                    Console.WriteLine();
            }

            // Part 2: Calculate results
            int part2 = EvaluateGrid(grid2);
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}