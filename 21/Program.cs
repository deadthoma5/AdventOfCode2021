using System;
using System.Linq;

public static class Globals
{
    public static bool debug = false;
}

namespace Day21
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
            var p1_start = Convert.ToInt32(lines[0].Split(" ")[4]);
            var p2_start = Convert.ToInt32(lines[1].Split(" ")[4]);
 
            // Part 1: Deterministic Dice
            var part1_limit = 1000;
            var part1 = simulate_Deterministic_Dice(p1_start, p2_start, part1_limit);

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Dirac Dice
            var part2_limit = 21;
            var part2 = simulate_Dirac_Dice(p1_start, p2_start, part2_limit);

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }

        // Part 1: Deterministic Dice game
        private static int simulate_Deterministic_Dice(int p1_start, int p2_start, int limit)
        {
            bool isOver = false;
            int player = 1;
            int p1_position = p1_start;
            int p2_position = p2_start;
            int p1_score = 0;
            int p2_score = 0;
            Dictionary<string, int> die = new Dictionary<string, int> {
                {"rolls", 0},
                {"value", 1},
                {"max", 100},
            };

            while (!isOver)
            {
                int sum = 0;

                foreach (var _ in Enumerable.Range(1,3))
                {
                    if (Globals.debug)
                        Console.WriteLine($"Roll: {die["value"]}");
                    
                    sum += die["value"];
                    die["rolls"]++;
                    die["value"]++;
                    if (die["value"] > 100)
                        die["value"] = 1;
                }

                if (player == 1)
                {
                    p1_position = (p1_position + sum - 1) % 10 + 1;
                    p1_score += p1_position;
                }
                else
                {
                    p2_position = (p2_position + sum - 1) % 10 + 1;
                    p2_score += p2_position;
                }
                
                if (Globals.debug)
                {
                    Console.WriteLine($"Player: {player}");
                    Console.WriteLine($"Rolls: {sum}");
                    if (player == 1)
                    {
                        Console.WriteLine($"Position: {p1_position}");
                        Console.WriteLine($"Score: {p1_score}");
                    }
                    else
                    {
                        Console.WriteLine($"Position: {p2_position}");
                        Console.WriteLine($"Score: {p2_score}");
                    }
                    Console.WriteLine();
                }

                if (p1_score >= limit || p2_score >= limit)
                    isOver = true;

                player = (player == 1) ? 2 : 1;
            }

            if (player == 1)
                return p1_score * die["rolls"];
            else
                return p2_score * die["rolls"];
        }

        // Helper function to generate frequencies of possible outcomes of rolling 3 three-sided dice for Part 2
        private static Dictionary<int, int> generateRolls()
        {
            var rolls = new Dictionary<int, int>();

            foreach (int i in Enumerable.Range(1, 3))
            {
                foreach (int j in Enumerable.Range(1, 3))
                {
                    foreach (int k in Enumerable.Range(1, 3))
                    {
                        var sum = i + j + k;
                        if (rolls.ContainsKey(sum))
                            rolls[sum]++;
                        else
                            rolls.Add(sum, 1);
                    }
                }
            }
            
            return rolls;
        }

        // Part 2: Dirac Dice game
        // Reference: https://www.reddit.com/r/adventofcode/comments/rl6p8y/comment/hphbuhb/?utm_source=share&utm_medium=web2x&context=3
        private static long simulate_Dirac_Dice(int p1_start, int p2_start, int limit)
        {
            var possible_rolls = generateRolls();
            long p1_wins = 0;
            long p2_wins = 0;
            var initial_state = new Tuple<int, int, int, int>(p1_start, p2_start, 0, 0);
            Dictionary<Tuple<int, int, int, int>, long> states = new Dictionary<Tuple<int, int, int, int>, long> {
                {initial_state, 1},
            };

            while (states.Count > 0)
            {
                var new_states = new Dictionary<Tuple<int, int, int, int>, long>();
                foreach (var state in states.Keys)
                {
                    var count = states[state];
                    var (p1_position, p2_position, p1_score, p2_score) = state;
                    foreach (var p1_roll in possible_rolls.Keys)
                    {
                        var new_p1_position = (p1_position + p1_roll - 1) % 10 + 1;
                        var new_p1_score = p1_score + new_p1_position;
                        if (new_p1_score >= limit)
                            p1_wins += count * possible_rolls[p1_roll];
                        else
                            foreach (var p2_roll in possible_rolls.Keys)
                            {
                                var new_p2_position = (p2_position + p2_roll - 1) % 10 + 1;
                                var new_p2_score = p2_score + new_p2_position;
                                if (new_p2_score >= limit)
                                    p2_wins += count * possible_rolls[p1_roll] * possible_rolls[p2_roll];
                                else
                                {
                                    var new_state = new Tuple<int, int, int, int>(new_p1_position, new_p2_position, new_p1_score, new_p2_score);
                                    if (new_states.ContainsKey(new_state))
                                        new_states[new_state] += count * possible_rolls[p1_roll] * possible_rolls[p2_roll];
                                    else
                                        new_states.Add(new_state, count * possible_rolls[p1_roll] * possible_rolls[p2_roll]);
                                }
                            }
                    }
                }
                states = new_states;
            }

            return Math.Max(p1_wins, p2_wins);
        }
    }
}