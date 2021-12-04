public static class Globals
{
    public static bool debug = true;
}

class Day03
{
    static int[] AddBinaryDigits(int[] tally, string line)
    {
        if (Globals.debug)
        {
            Console.WriteLine($"Line to add: {line}");
        }
        for (int i = 0; i < line.Length; i++)
        {
            if (Globals.debug)
            {
                Console.WriteLine($"Position: {i}, Current Tally: {tally[i]}, Current Line Value: {Char.GetNumericValue(line[i])}");
            }
            tally[i] += (int)Char.GetNumericValue(line[i]);
        }
        return tally;
    }

    static string CalcGamma(int[] tally, int datapoints)
    {
        int threshold = datapoints/2;
        string gamma = string.Empty;
        foreach (int tallydigit in tally)
        {
            if (tallydigit > threshold)
                gamma += '1';
            else
                gamma += '0';
        }
        return gamma;
    }

    static string CalcEpsilon(string gamma)
    {
        string epsilon = FlipBits(gamma);
        return epsilon;
    }

    static string FlipBits(string original)
    {
        string flipped = string.Empty;
        foreach (char digit in original)
        {
            if (digit == '1')
                flipped += '0';
            else
                flipped += '1';
        }
        return flipped;
    }

    static string CalcO2(string[] lines, int position)
    {     
        if (lines.Length <= 1)
            return lines[0];
        
        int tally = 0;
        int threshold = lines.Length/2;
        char majorbit;
        if (lines.Length % 2 != 0)
            threshold++;
        foreach (string line in lines)
        {
            if (line[position] == '1')
                tally++;
        }
        if (Globals.debug)
        {
            Console.WriteLine($"Lines: {lines.Length}, Threshold: {threshold}, Tally: {tally}");
        }
        if (tally >= threshold)
            majorbit = '1';
        else
            majorbit = '0';
        if (Globals.debug)
        {
            Console.WriteLine($"Major bit at position {position}: {majorbit}");
        }
        
        List<string> newlines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i][position] == majorbit)
                newlines.Add(lines[i]);
        }
        if (Globals.debug)
        {
            foreach (string newline in newlines)
            {
                Console.WriteLine($"Added string: {newline}");
            }
            
        }
        return CalcO2(newlines.ToArray(), position + 1);
    }

    static string CalcCO2(string[] lines, int position)
    {
        if (lines.Length <= 1)
            return lines[0];
        
        int tally = 0;
        int threshold = lines.Length/2;
        char minorbit;
        if (lines.Length % 2 != 0)
            threshold++;
        foreach (string line in lines)
        {
            if (line[position] == '1')
                tally++;
        }
        if (Globals.debug)
        {
            Console.WriteLine($"Lines: {lines.Length}, Threshold: {threshold}, Tally: {tally}");
        }
        if (tally < threshold)
            minorbit = '1';
        else
            minorbit = '0';
        if (Globals.debug)
        {
            Console.WriteLine($"Minor bit at position {position}: {minorbit}");
        }
        
        List<string> newlines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i][position] == minorbit)
                newlines.Add(lines[i]);
        }
        if (Globals.debug)
        {
            foreach (string newline in newlines)
            {
                Console.WriteLine($"Added string: {newline}");
            }
            
        }
        return CalcCO2(newlines.ToArray(), position + 1);
    }

    static int CalcPart1(string[] lines)
    {
        int[] tally = new int[lines[0].Length];
        foreach (string line in lines)
            tally = AddBinaryDigits(tally, line);
        if (Globals.debug)
        {
            Console.Write("Tally Result: ");
            foreach (int i in tally)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
        }

        string gammastring = CalcGamma(tally, lines.Length);
        int gamma = Convert.ToInt32(gammastring, 2);
        if (Globals.debug)
        {
            Console.WriteLine($"Gamma string: {gammastring}");
            Console.WriteLine($"Gamma value: {gamma}");
        }
        string epsilonstring = CalcEpsilon(gammastring);
        int epsilon = Convert.ToInt32(epsilonstring, 2);
        if (Globals.debug)
        {
            Console.WriteLine($"Epsilon string: {epsilonstring}");
            Console.WriteLine($"Epsilon value: {epsilon}");
        }
        return gamma * epsilon;
    }

    static int CalcPart2(string[] lines)
    {
        string O2string = CalcO2(lines, 0);
        int O2 = Convert.ToInt32(O2string, 2);
        if (Globals.debug)
        {
            Console.WriteLine($"O2 string: {O2string}");
            Console.WriteLine($"O2 value: {O2}");
        }
        
        string CO2string = CalcCO2(lines, 0);
        int CO2 = Convert.ToInt32(CO2string, 2);
        if (Globals.debug)
        {
            Console.WriteLine($"CO2 string: {CO2string}");
            Console.WriteLine($"CO2 value: {CO2}");
        }
        return O2 * CO2;
    }

    static public void Main(string[] args)
    {
        string[] lines;
        if (Globals.debug)
            lines = File.ReadAllLines("input_test");
        else
            lines = File.ReadAllLines("input");
        
        int part1 = CalcPart1(lines);
        Console.WriteLine($"Part 1: {part1}");

        int part2 = CalcPart2(lines);
        Console.WriteLine($"Part 2: {part2}");
    }
}