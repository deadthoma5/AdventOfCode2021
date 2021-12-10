public static class Globals
{
    public static bool debug = false;
}

namespace Day10
{
    class Program
    {
        // Flip an opening bracket to its closing bracket complement
        public static char Flip(char symbol)
        {
            switch (symbol)
            {
                case '(':
                    return ')';
                case '[':
                    return ']';
                case '{':
                    return '}';
                case '<':
                    return '>';
                default:
                    return ' ';
            }
        }

        // Part 1: Return a symbol's poitn value
        public static int Points_Part1(char symbol)
        {
            switch (symbol)
            {
                case ')':
                    return 3;
                case ']':
                    return 57;
                case '}':
                    return 1197;
                case '>':
                    return 25137;
                default:
                    return 0;
            }
        }

        // Part 2: Return a symbol's point value
        public static int Points_Part2(char symbol)
        {
            switch (symbol)
            {
                case ')':
                    return 1;
                case ']':
                    return 2;
                case '}':
                    return 3;
                case '>':
                    return 4;
                default:
                    return 0;
            }
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

            // Part 1: Main program loop
            int part1 = 0;
            var incompletes = new List<string>();
            foreach (string line in lines)
            {
                var openers = new Stack<char>();
                bool isCorrupt = false;
                foreach (char symbol in line)
                {
                    switch (symbol)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case '<':
                            openers.Push(symbol);
                            if (Globals.debug) Console.WriteLine($"Pushed '{symbol}' to stack");
                            break;
                        default:
                            char p = openers.Pop();
                            if (Globals.debug)
                                Console.WriteLine($"Pop: {p}, Flip_pop: {Flip(p)}, Symbol: {symbol}, Test: {Flip(p) == symbol}");
                            if (Flip(p) != symbol)
                            {
                                isCorrupt = true;
                                if (Globals.debug)
                                    Console.WriteLine($"Expected {Flip(p)}, but found {symbol} instead");
                                part1 += Points_Part1(symbol);
                            }
                            break;
                    }
                }
                if (!isCorrupt)
                {
                    char[] incArray = openers.ToArray();
                    string incString = new string(incArray);
                    incompletes.Add(incString);
                }
            }

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");
            if (Globals.debug)
                Console.WriteLine();

            // Part 2: Main program loop
            var scores = new List<long>();    // Int32 wasn't big enough to hold Part 2's answer
            foreach (string line in incompletes)
            {
                long score = 0;
                foreach (char symbol in line)
                {
                    score *= 5;
                    if (Globals.debug)
                        Console.WriteLine($"Multiply by 5: score = {score}. Adding value of '{Flip(symbol)}' = {Points_Part2(Flip(symbol))}");
                    score += Points_Part2(Flip(symbol));
                    if (Globals.debug)
                        Console.WriteLine($"Total score is now {score}");
                }
                scores.Add(score);
            }

            // Part 2: Calculate results
            if (Globals.debug)
            {
                Console.WriteLine();
                Console.WriteLine("Scores:");
                foreach (int score in scores) Console.WriteLine(score);
            }

            scores.Sort();
            long part2 = scores[scores.Count/2];

            // Part 2: Display results            
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}