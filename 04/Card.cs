using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    class Card
    {
        public int[][] grid = new int[5][];
        public bool[,] score = new bool[5, 5];
        public bool isWinner = false;

        public Card(string[] lines)
        {
            for (int row = 0; row < lines.Length; row++)
            {
                Regex r = new Regex(" +");
                grid[row] = r.Split(lines[row].Trim()).Select(n => Convert.ToInt32(n)).ToArray();
            }
            if (Globals.debug)
            {
                Console.WriteLine("Added a new card:");
                foreach (int[] row in grid)
                {
                    foreach (int col in row)
                        Console.Write(col + " ");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        // Return an array with the values from a specified row of a card
        public bool[] GetScoreRow(int rowNumber)
        {
            return Enumerable.Range(0, score.GetLength(0))
                .Select(x => score[rowNumber, x])
                .ToArray();
        }

        // Return an array with the values from a specified column of a card
        public bool[] GetScoreColumn(int colNumber)
        {
            return Enumerable.Range(0, score.GetLength(1))
                .Select(x => score[x, colNumber])
                .ToArray();
        }

        // Evaluate a card's rows and columns for a winner
        public void CheckWinner()
        {
            for (int i = 0; i < score.GetLength(0); i++)
            {
                if (GetScoreRow(i).Aggregate((a, b) => a & b) || GetScoreColumn(i).Aggregate((a, b) => a & b))
                    isWinner = true; 
            }
        }
    }
}