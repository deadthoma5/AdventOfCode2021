using System;
using System.Drawing;

public static class Globals
{
    public static bool debug = false;
}

namespace Day11
{
    class Program
    {
        // Parse input to build initial grid configuration
        static public int[,] GetGrid(string[] lines)
        {
            (int dim_x, int dim_y) = (lines[0].Length, lines.Length);
            int[,] grid = new int[dim_x, dim_y];
            for (int y = 0; y < dim_y; y++)
            {
                for (int x = 0; x < dim_x; x++)
                {
                    grid[x, y] = Convert.ToInt32(lines[y][x].ToString());
                }
            }
            return grid;
        }

        // Display the current grid to screen
        static public void ShowGrid(int[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Console.Write(grid[x,y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return;
        }

        // Borrowed from Day 9. Return a list of a point's neighbors. Modification: include the point itself in the list.
        static public List<Point> GetNeighborsAndSelf(int[,] grid, Point p)
        {
            var neighbors = (from x in Enumerable.Range(p.X - 1, 3)
                            from y in Enumerable.Range(p.Y - 1, 3)
                            where (x >= 0) && (x < grid.GetLength(0)) && (y >= 0) && (y < grid.GetLength(1))
                            select new Point(x, y)).ToList();
            return neighbors;
        }

        // Borrowed from Day 9's Breadth First Search (BFS) algorithm. Modifications: increment self+neighbor points and added recursion to continue flashing points with each pass of the grid.
        static public int Flash(int[,] grid, List<Point> flashed)
        {
            var flashqueue = new Queue<Point>();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    var p = new Point(x, y);
                    if (grid[p.X, p.Y] > 9 && !flashed.Contains(p))
                    {
                        flashqueue.Enqueue(p);
                        if (Globals.debug)
                            Console.WriteLine($"Queued {p} for flashing");
                    }                    
                }
            }
            
            // Recursion base case: exit when there's no more queued flashpoints
            if (flashqueue.Count == 0)
            {
                foreach (Point p in flashed)
                    grid[p.X, p.Y] = 0;
                return flashed.Count;
            }
                
            while (flashqueue.Count > 0)
            {
                if (Globals.debug)
                    Console.WriteLine($"Flash queue count: {flashqueue.Count}");
                Point flashpoint = flashqueue.Dequeue();
                flashed.Add(flashpoint);
                if (Globals.debug)
                    Console.WriteLine($"Flashing: {flashpoint}");
                foreach (Point n in GetNeighborsAndSelf(grid, flashpoint))
                {
                    grid[n.X, n.Y]++;
                    if (Globals.debug)
                        Console.WriteLine($"Incremented: {n}");
                }
            }
            if (Globals.debug)
                Console.WriteLine();
            return Flash(grid, flashed);
        }

        // Increment the value of each point in the grid and trigger flashes
        static public int Step(int[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    grid[x, y]++;
                }
            }
            return Flash(grid, new List<Point>());
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
            
            // Part 1: Initialize variables
            var grid = GetGrid(lines);
            if (Globals.debug)
            {
                Console.WriteLine("Before any steps:");
                ShowGrid(grid);
            }
                
            // Part 1: Main program loop
            int part1 = 0;
            int part1_steps = 100;
            for (int i = 1; i <= part1_steps; i++)
            {
                part1 += Step(grid);
                if (Globals.debug)
                {
                    Console.WriteLine($"After step {i}:");
                    if (i == 195) ShowGrid(grid);
                }
            }
 
            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");
            if (Globals.debug)
                Console.WriteLine();

            // Part 2: Initialize variables
            grid = GetGrid(lines);
            if (Globals.debug)
            {
                Console.WriteLine("Before any steps:");
                ShowGrid(grid);
            }

            // Part 2: Main program loop
            int part2 = 0;
            int flashes = 0;
            int gridsize = grid.GetLength(0) * grid.GetLength(1);
            do
            {
                part2++;
                flashes = Step(grid);
            } while (flashes != gridsize);

            // Part 2: Display results        
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}