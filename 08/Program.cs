using System.Linq;

public static class Globals
{
    public static bool debug = false;
}

namespace Day08
{
    class Program
    {
        // Translate a jumbled output word into a char digit using the decoder
        static public char Translate(string word, Dictionary<char, string> decoder)
        {
            foreach (var entry in decoder)
            {
                if (word.Length == entry.Value.Length && Intersect(word, entry.Value) == word.Length)
                    return entry.Key;
            }
            return ' ';
        }
        
        // Count the number of common chars between two strings
        static public int Intersect(string a, string b)
        {
            int count = 0;
            foreach (char letter in a)
            {
                if (b.Contains(letter))
                    count++;
            }
            return count;
        }

        // Build a decoder dictionary, deduced from the given signals
        static public Dictionary<char, string> GetDecoder(List<string> input)
        {
            var decoder = new Dictionary<char, string>();
            var tbd_235 = new List<string>();
            var tbd_069 = new List<string>();

            // Decode 1, 4, 7, 8
            foreach (string word in input)
                {
                    switch(word.Length) {
                        case 2:
                            decoder.Add('1', word);
                            break;
                        case 3:
                            decoder.Add('7', word);
                            break;
                        case 4:
                            decoder.Add('4', word);
                            break;
                        case 5:    // 2, 3, or 5
                            tbd_235.Add(word);
                            break;
                        case 6:    // 0, 6, or 9
                            tbd_069.Add(word);
                            break;
                        case 7:
                            decoder.Add('8', word);
                            break;
                    }
                }
            
            // Decode 2, 3, or 5
            foreach (string word in tbd_235)
            {
                if (Intersect(decoder['1'], word) == 2)
                    decoder.Add('3', word);
                else if (Intersect(decoder['4'], word) == 2)
                    decoder.Add('2', word);
                else
                    decoder.Add('5', word);
            }

            // Decode 0, 6, or 9
            foreach (string word in tbd_069)
            {
                if (Intersect(decoder['3'], word) == 5)
                    decoder.Add('9', word);
                else if (Intersect(decoder['1'], word) == 1)
                    decoder.Add('6', word);
                else
                    decoder.Add('0', word);
            }

            if (Globals.debug)
            {
                Console.WriteLine("Decoder:");
                for (int i = 0; i < decoder.Count; i++)
                    Console.WriteLine($"{i} -> {decoder[Convert.ToString(i)[0]]}");
                Console.WriteLine();
            }
            
            return decoder;
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
            var part1_output = lines.Select(n => n.Split('|')[1].Trim().Split(' ')).ToArray();
            
            // Part 1: Main program loop
            int part1 = 0;
            foreach (string[] line in part1_output)
            {
                foreach (string word in line)
                {
                    if (word.Length == 2 || word.Length == 4 || word.Length == 3 || word.Length == 7)
                        part1++;
                }
            }

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Parse input
            var part2_signal = lines.Select(n => n.Split('|')[0].Trim().Split(' ').ToList()).ToArray();
            var part2_output = lines.Select(n => n.Split('|')[1].Trim().Split(' ').ToList()).ToArray();

            // Part 2: Main program loop
            int part2 = 0;
            for (int i = 0; i < part2_signal.Length; i++)
            {
                var decoder = GetDecoder(part2_signal[i]);
                string output_string = string.Empty;
                foreach (string word in part2_output[i])
                    output_string += Translate(word, decoder);
                part2 += Convert.ToInt32(output_string);
            }

            // Part 2: Display results
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}