using System;
using System.Linq;

public static class Globals
{
    public static bool debug = false;
}

namespace Day20
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
            var algoString = lines[0].ToArray();
            var algo = algoString.Select(n => Convert.ToInt32(n == '#')).ToArray();
            var imageStrings = lines.Skip(2).ToList();
 
            // Part 1: Initialize variables
            var steps = 2;
            var pad = (steps + 1) * 4;
            (int dim_row, int dim_col) = (imageStrings.Count, imageStrings[0].Length);
            int[,] image = new int[dim_row + pad, dim_col + pad];

            for (int i = 0; i < dim_row; i++)
            {
                for (int j = 0; j < dim_col; j++)
                {
                    image[i + pad / 2, j + pad / 2] = (imageStrings[i][j] == '#') ? 1 : 0;
                }
            }

            if (Globals.debug)
            {
                Console.WriteLine("Initial image:");
                show(image);
                Console.WriteLine();
            }

            // Part 1: Main loop
            foreach (var n in Enumerable.Range(1, steps))
            {
                image = enhance(image, algo);

                if (Globals.debug)
                {
                    Console.WriteLine($"After step {n}:");
                    show(image);
                    Console.WriteLine();
                }
            }

            var part1 = count(image, steps);
            
            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Re-initialize variables
            steps = 50;
            pad = (steps + 1) * 4;
            image = new int[dim_row + pad, dim_col + pad];

            for (int i = 0; i < dim_row; i++)
            {
                for (int j = 0; j < dim_col; j++)
                {
                    image[i + pad / 2, j + pad / 2] = (imageStrings[i][j] == '#') ? 1 : 0;
                }
            }

            // Part 2: Main loop
            foreach (var n in Enumerable.Range(1, steps))
            {
                image = enhance(image, algo);

                if (Globals.debug)
                {
                    Console.WriteLine($"After step {n}:");
                    show(image);
                    Console.WriteLine();
                }
            }
            
            var part2 = count(image, steps);

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }

        // Display image to screen
        private static void show(int[,] image)
        {
            (int dim_row, int dim_col) = (image.GetLength(0), image.GetLength(1));

            for (int i = 0; i < dim_row; i++)
            {
                for (int j = 0; j < dim_col; j++)
                {
                    Console.Write(image[i, j]);
                }
                Console.WriteLine();
            }
        }

        // enhance the image per algorithm
        private static int[,] enhance(int[,] image, int[] algo)
        {
            (int dim_row, int dim_col) = (image.GetLength(0), image.GetLength(1));
            int [,] workImage = new int[dim_row, dim_col];

            // deep-copy input image to temporary working image
            for (int i = 0; i < dim_row; i++)
            {
                for (int j = 0; j < dim_col; j++)
                {
                    workImage[i, j] = image[i, j];
                }
            }

            // evaluate every cell within bounds
            for (int i = 1; i < dim_row - 1; i++)
            {
                for (int j = 1; j < dim_col - 1; j++)
                {
                    var binString = string.Empty;

                    // evaluate 3x3 area around i,j
                    for (int di = -1; di <= 1; di++)
                    {
                        for (int dj = -1; dj <= 1; dj++)
                        {
                            binString += workImage[i + di, j + dj].ToString();
                        }
                    }

                    // update newImage cell at i,j per algorithm
                    image[i, j] = algo[bin2dec(binString)];
                }
            }

            return image;
        }

        // convert a binary string into a decimal number
        static private int bin2dec(string number)
        {
            return Convert.ToInt32(number, 2);
        }

        // sum "on" pixels within original image bounds, ignore counting any strange edge effects near border of working area
        static private int count(int[,] image, int steps)
        {
            int sum = 0;
            int pad = steps + 2;
            (int dim_row, int dim_col) = (image.GetLength(0), image.GetLength(1));

            for (int i = pad ; i < dim_row - pad; i++)
            {
                for (int j = pad; j < dim_col - pad; j++)
                {
                    sum += image[i, j];
                }
            }

            return sum;
        }
    }
}