using System.Linq;

public static class Globals
{
    public static bool debug = false;
    public static Dictionary<string, Tuple<string, string>> cache = new Dictionary<string, Tuple<string, string>>();
}

namespace Day14
{
    class Program
    {
        static public Tuple<string, Dictionary<string, string>> ParseInput(string[] lines)
        {
            var template = lines[0];
            var instructions = new Dictionary<string, string>();

            lines = lines.Skip(2).ToArray();
            foreach (var line in lines)
            {
                var parts = line.Split(" -> ");
                instructions.Add(parts[0], parts[1]);
            }

            return new Tuple<string, Dictionary<string, string>>(template, instructions);
        }

        static public Dictionary<string, long> InitElems(string template, Dictionary<string, string> instructions)
        {
            var elems = new Dictionary<string, long>();

            foreach (var elem in instructions.Values)
            {
                if (!elems.ContainsKey(elem))
                    elems.Add(elem, 0);
            }

            foreach (var elem in template)
            {
                elems[elem.ToString()]++;
            }

            return elems;
        }
        
        static public Tuple<string, string> PairInsertion(string pair, Dictionary<string, string> instructions)
        {
            var left = string.Empty;
            left += pair[0];
            left += instructions[pair];

            var right = string.Empty;
            right += left[1];
            right += pair[1];

            return new Tuple<string, string>(left, right);
        }

        static public Dictionary<string, long> InitPairs(string template, Dictionary<string, string> instructions)
        {
            var pairs = new Dictionary<string, long>();

            foreach (var pair in instructions.Keys)
            {
                pairs.Add(pair, 0);
                
                if (!Globals.cache.ContainsKey(pair))
                {
                    Globals.cache.Add(pair, PairInsertion(pair, instructions));
                    if (Globals.debug)
                        Console.WriteLine($"Added {pair} -> {Globals.cache[pair]} to the cache.");
                }
            }

            for (int i = 0; i < template.Length - 1; i++)
            {
                var pair = template.Substring(i, 2);
                pairs[pair]++;
            }
            
            if (Globals.debug) Console.WriteLine();

            return pairs;
        }

        static public Tuple<Dictionary<string, long>, Dictionary<string, long>> ProcessPairs(Dictionary<string, long> pairs, Dictionary<string, string> instructions, Dictionary<string, long> elems)
        {
            var q = new Queue<KeyValuePair<string, long>>();

            foreach (var pair in pairs)
            {
                if (pair.Value > 0)
                    q.Enqueue(pair);
            }

            while (q.Count > 0)
            {
                var pair = q.Dequeue();
                var count = pair.Value;
                (var left, var right) = Globals.cache[pair.Key];
                elems[instructions[pair.Key]] += count;
                pairs[left] += count;
                pairs[right] += count;
                pairs[pair.Key] -= count;
                
                if (Globals.debug)
                    Console.WriteLine($"Processed {pair.Key}, elems[{instructions[pair.Key]}] = {elems[instructions[pair.Key]]}, pairs[{left}] = {pairs[left]}, pairs[{right}] = {pairs[right]}, pairs[{pair.Key}] = {pairs[pair.Key]}");
            }

            return new Tuple<Dictionary<string, long>, Dictionary<string, long>>(pairs, elems);
        }

        static public long Polymerize(string template, Dictionary<string, string> instructions, int steps)
        {
            var elems = InitElems(template, instructions);
            var pairs = InitPairs(template, instructions);

            if (Globals.debug)
            {
                Console.WriteLine($"Template:     {template}");
                foreach (var pair in pairs) Console.WriteLine($"    {pair.Key} {pair.Value}");
                foreach (var elem in elems) Console.WriteLine($"    {elem.Key} {elem.Value}");
                Console.WriteLine();
            }

            for (int i = 1; i <= steps; i++)
            {
                (pairs, elems) = ProcessPairs(pairs, instructions, elems);
                if (Globals.debug)
                {
                    Console.WriteLine($"After step {i}:");
                    foreach (var pair in pairs) Console.WriteLine($"    {pair.Key} {pair.Value}");
                    foreach (var elem in elems) Console.WriteLine($"    {elem.Key} {elem.Value}");
                    Console.WriteLine();
                }
            }

            return elems.Values.Max() - elems.Values.Min();
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

            // Parse Input
            (var template, var instructions) = ParseInput(lines);

            // Part 1: Main program loop
            long part1 = Polymerize(template, instructions, 10);

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Main program loop
            long part2 = Polymerize(template, instructions, 40);

            // Part 2: Display results
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
