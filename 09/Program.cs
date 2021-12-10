using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

public static class Globals
{
    public static bool debug = false;
}

namespace Day09
{
    class Program
    {
        // Find nearest horizontal and vertical neighbor points, respecting grid boundaries. Omit height=9 points for Part 2.
        static public List<Point> GetNeighbors(int[,] grid, Point p, bool isPart2)
        {
            var neighbors = (from x in Enumerable.Range(p.X - 1, 3)
                            from y in Enumerable.Range(p.Y - 1, 3)
                            where (x >= 0) && (x < grid.GetLength(0)) && (y >= 0) && (y < grid.GetLength(1))
                            select new Point(x, y)).Where(n => n != p && (Math.Abs(n.X - p.X) + Math.Abs(n.Y - p.Y)) < 2).ToList();
            
            if (isPart2)
                neighbors = neighbors.Where(n => grid[n.X, n.Y] < 9).ToList();

            return neighbors;
        }

        // Part 2: Given a low point, find all contiguous points inside height=9 or boundary points.
        // Note: I used the Breadth First Search (BFS) Algorithm
        // Reference: https://favtutor.com/blogs/breadth-first-search-python
        static public int GetBasinSize(int[,] grid, Point lowpoint)
        {
            var visited = new List<Point>();
            var queue = new Queue<Point>();
            queue.Enqueue(lowpoint);

            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();

                foreach (Point n in GetNeighbors(grid, p, true))
                {
                    if (!visited.Contains(n))
                    {
                        visited.Add(n);
                        queue.Enqueue(n);
                    }
                }
            }
                
            return visited.Count;
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

            // Part 1: Parse input
            (int dim_x, int dim_y) = (lines[0].Length, lines.Length);
            int [,] grid = new int[dim_x, dim_y];
            for (int j = 0; j < dim_y; j++)
            {
                for (int i = 0; i < dim_x; i++)
                {
                    grid[i, j] = Convert.ToInt32(lines[j][i].ToString());
                }
            }

            // Part 1: Main program loop
            int part1 = 0;
            var lowpoints = new List<Point>();
            for (int x = 0; x < dim_x; x++)
            {
                for (int y = 0; y < dim_y; y++)
                {
                    var p = new Point(x, y);
                    var neighbors = GetNeighbors(grid, p, false);
                    if (neighbors.All(n => grid[p.X, p.Y] < grid[n.X, n.Y]))
                    {
                        var lowpoint = p;
                        int risk = grid[lowpoint.X, lowpoint.Y] + 1;
                        part1 += risk;
                        lowpoints.Add(lowpoint);
                        if (Globals.debug)
                        {
                            Console.WriteLine($"Low point: {lowpoint} = {grid[lowpoint.X, lowpoint.Y]}, added risk level = {risk}");
                            foreach (Point n in neighbors) Console.WriteLine($"    neighbor: {n}");
                            Console.WriteLine();
                        }                        
                    }
                }
            }

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            var basins = new Dictionary<Point, int>();

            // Part 2: Main program loop
            if (Globals.debug) Console.WriteLine("\n============================\n");

            foreach (Point lp in lowpoints)
            {
                basins.Add(lp, GetBasinSize(grid, lp));
            }

            // Part 2: Calculate results            
            basins = basins.OrderByDescending(entry => entry.Value).ToDictionary(entry => entry.Key, entry => entry.Value);

            if (Globals.debug)
            {
                foreach (var entry in basins)
                    Console.WriteLine($"Low point: {entry.Key} = {grid[entry.Key.X, entry.Key.Y]}, basin size = {basins[entry.Key]}");
                Console.WriteLine();
            }

            var basins_sizes_top3 = basins.Take(3).Select(entry => entry.Value).ToList();            
            var part2 = basins_sizes_top3.Aggregate((a, b) => a * b);

            // Part 2: Display results
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}