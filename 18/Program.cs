﻿using System.Linq;

public static class Globals
{
    public static bool debug = false;
}

namespace Day18
{
    class Program
    {
        static private List<(char type, int value)> Parse(string line)
        {
            var list = new List<(char type, int value)>();

            foreach (var c in line)
            {
                if (c == ',') continue;
                var type = c switch
                {
                    '[' or ']' => c,
                    _ => '0'
                };
                var value = type switch
                {
                    '[' or ']' => 0,
                    _ => c - '0'
                };
                list.Add((type, value));
            }

            return list;
        }

        static private int Magnitude(List<(char type, int value)> sum)
        {
        restart:
            for (var i = 0; i < sum.Count - 2; i++)
            {
                var c = sum[i].type;
                if (c == '0' && sum[i+1].type == '0')
                {
                    var m = sum[i].value * 3 + sum[i+1].value * 2;
                    sum[i] = ('0', m);                   
                    sum.RemoveRange(i+1, 2);
                    sum.RemoveAt(i-1);
                    goto restart;
                }
            }

            return sum[0].value;
        }

        static private List<(char type, int value)> Reduce(List<(char type, int value)> sum)
        {
        restart:
            var leftParens = 0;
            for(var i = 0; i < sum.Count; i++)
            {
                var v = sum[i].value;
                var c = sum[i].type;

                leftParens = c switch { '[' => leftParens + 1, ']' => leftParens - 1, _ => leftParens };

                if (leftParens == 5)
                {
                    Explode(i);

                    goto restart;
                }
            }

            for(var i = 0; i < sum.Count; i++)
            {
                var v = sum[i].value;

                if (v > 9)
                {   
                    Split(i);

                    goto restart;
                }
            }

            return sum;

            void Explode(int i)
            {
                // explode left
                var leftIndex = sum.GetRange(0, i).FindLastIndex(t => t.type == '0');
                if (leftIndex != -1)
                {
                    sum[leftIndex] = ('0', sum[leftIndex].value + sum[i+1].value);
                }
                else
                {
                    sum[i+1] = ('0', 0);
                }

                // explode right
                var rightIndex = sum.FindIndex(i+4, t => t.type == '0');
                if (rightIndex != -1)
                {
                    sum[rightIndex] = ('0', sum[rightIndex].value + sum[i+2].value);
                }
                else
                {
                    sum[i+2] = ('0', 0);
                }

                sum[i] = ('0', 0);
                sum.RemoveRange(i+1, 3);
            }
        
            void Split(int i)
            {
                var div = sum[i].value / 2f;
                var left = (int)Math.Floor(div);
                var right = (int)Math.Ceiling(div);
                sum[i] = (']', 0);
                sum.Insert(i, ('0', right));
                sum.Insert(i, ('0', left));
                sum.Insert(i, ('[', 0));
            }
        }

        static private List<(char type, int value)> Sum(List<(char type, int value)> a, List<(char type, int value)> b)
        {
            var list = new List<(char type, int value)>();
            
            list.Add(('[', 0));
            list.AddRange(a);
            list.AddRange(b);
            list.Add((']', 0));

            return list;
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
            var snailfishes = lines.Select(Parse).ToList();

            // Part 1: Initialize variables
            var part1 = Magnitude(snailfishes.Aggregate((s,sf) => Reduce(Sum(s,sf))));

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            int part2 = (from a in snailfishes
                     from b in snailfishes
                     where a != b
                     select Magnitude(Reduce(Sum(a, b))))
                     .DefaultIfEmpty()
                     .Max();

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}