using System;
using System.Text.RegularExpressions;

public static class Globals
{
    public static bool use_real_input = true;
    public static bool debug = true;
}

namespace Day13
{
    class Program
    {
        // Get the first section of input (grid coordinates)
        static public List<string> GetGridLines(string[] lines)
        {
            var coords_filter = new Regex(@"^\d+,\d+$");
            return lines.Where(n => coords_filter.IsMatch(n)).ToList();
        }

        // Build the grid from grid coordinates
        static public int[,] GetGrid(string[] lines)
        {
            var grid_lines = GetGridLines(lines);
            var xlist = new List<int>();
            var ylist = new List<int>();

            foreach (string line in grid_lines)
            {
                var split = line.Split(",");
                xlist.Add(Convert.ToInt32(split[0]));
                ylist.Add(Convert.ToInt32(split[1]));
            }
            
            if (Globals.debug)
                Console.WriteLine($"x max: {xlist.Max()}, y max: {ylist.Max()}");
            
            var grid = new int[xlist.Max() + 1, ylist.Max() + 1];
            for (int i = 0; i < xlist.Count; i++)
            {
                grid[xlist[i], ylist[i]] = 1;
            }

            if (Globals.debug)
            {
                Console.WriteLine($"Initial grid with dimensions: x={grid.GetLength(0)}, y={grid.GetLength(1)} :");
                ShowGrid(grid);
            }

            return grid;
        }

        // Display the current grid
        static public void ShowGrid(int[,] grid)
        {
            for (int y=0; y < grid.GetLength(1); y++)
            {
                for (int x=0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == 0)
                        Console.Write(".");
                    else if (grid[x, y] == 1)
                        Console.Write("#");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return;
        }

        // Get the second section of input (folding instructions)
        static public List<string> GetFoldsLines(string[] lines)
        {
            var folds_filter = new Regex(@"^fold along");
            return lines.Where(n => folds_filter.IsMatch(n)).ToList();
        }

        // Build a list of folding instructions
        static public List<Tuple<string, int>> GetFolds(string[] lines)
        {
            var folds_lines = GetFoldsLines(lines);
            var folds = new List<Tuple<string, int>>();
            foreach (string line in folds_lines)
            {
                var splits = line.Split(" ")[2].Split("=");
                folds.Add(Tuple.Create(splits[0], Convert.ToInt32(splits[1])));
            }
            return folds;
        }

        // Part 1: Perform an individual fold
        static public int[,] DoFold(int[,] grid, Tuple<string,int> fold)
        {
            if (Globals.debug)
                Console.WriteLine($"Fold: {fold}");

            // Fold along constant x (vertical line)
            if (fold.Item1 == "x")
            {
                var _grid = new int[fold.Item2, grid.GetLength(1)];
                for (int y = 0; y < _grid.GetLength(1); y++)
                {
                    for (int x = 0; x < _grid.GetLength(0); x++)
                    {
                        if (x <= 2*fold.Item2 - grid.GetLength(0))
                        {
                            Console.WriteLine("hello");
                            _grid[x, y] = grid[x, y];
                        }    
                        else
                            _grid[x, y] = grid[x, y] + grid[2*fold.Item2 - x, y];
                        
                        // Cleanup: cap a cell at a maximum of 1
                        if (_grid[x, y] > 1)
                            _grid[x, y] = 1;
                    }
                }
                grid = _grid;
            }
            // Fold along constant y (horizontal line)
            else if (fold.Item1 == "y")
            {
                var _grid = new int[grid.GetLength(0), fold.Item2];
                for (int y = 0; y < _grid.GetLength(1); y++)
                {
                    for (int x = 0; x < _grid.GetLength(0); x++)
                    {
                        if (y <= 2*fold.Item2 - grid.GetLength(1))
                            _grid[x, y] = grid[x, y];
                        else
                            _grid[x, y] = grid[x, y] + grid[x, 2*fold.Item2 - y];
                        
                        // Cleanup: cap a cell at a maximum of 1
                        if (_grid[x, y] > 1)
                            _grid[x, y] = 1;
                    }
                }
                grid = _grid;
            }

            if (Globals.debug)
                ShowGrid(grid);

            return grid;
        }

        // Part 2: Perform a series of folds
        static public int[,] DoAllFolds(int[,] grid, List<Tuple<string, int>> folds)
        {
            foreach (var fold in folds)
            {
                grid = DoFold(grid, fold);
            }
            return grid;
        }

        // Part 1: Count the number of dots in a grid        
        static public int CountDots(int[,] grid)
        {
            int sum = 0;
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    sum += grid[x, y];
                }
            }
            return sum;
        }

        // Main program entry point
        static public void Main(string[] args)
        {
            // Obtain input from file
            string[] lines;
            if (Globals.use_real_input)
                lines = File.ReadAllLines("input");
            else
                lines = File.ReadAllLines("input_test");

            // Part 1: Initialize variables
            var grid = GetGrid(lines);            
            var folds = GetFolds(lines);

            // Part 1: Main program loop
            grid = DoFold(grid, folds[0]);
 
            // Part 1: Calculate results
            int part1 = CountDots(grid);

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Reinitialize variables
            grid = GetGrid(lines);

            // Part 2: Main program loop
            grid = DoAllFolds(grid, folds);

            // Part 2: Display results        
            Console.WriteLine($"Part 2:");
            ShowGrid(grid);
        }
    }
}
