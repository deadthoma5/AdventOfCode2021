using System.Linq;

public static class Globals
{
    public static bool debug = false;
}

namespace Day16
{
    class Program
    {
        public static long sumVersions = 0;
        // Convert hexadecimal string to binary string
        static private string hex2bin(string hexString)
        {
            string binaryString = String.Join(String.Empty,
                hexString.Select(
                    c => Convert.ToString(Convert.ToInt64(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );
            return binaryString;
        }

        // Convert binary string to decimal number
        static private long bin2dec(string binaryString)
        {
            return Convert.ToInt64(binaryString, 2);
        }

        static private (string, long) parse(string input)
        {
            var version = bin2dec(input.Substring(0, 3));
            sumVersions += version;
            if (Globals.debug)
            {
                Console.WriteLine($"version bin: {input.Substring(0, 3)}");
                Console.WriteLine($"version dec: {version}");
            }
            input = input.Substring(3);

            var typeID = bin2dec(input.Substring(0, 3));
            if (Globals.debug)
            {
                Console.WriteLine($"typeID bin: {input.Substring(0, 3)}");
                Console.WriteLine($"typeID dec: {typeID}");
            }
            input = input.Substring(3);
            
            if (typeID == 4)
            {
                string value = String.Empty;
                while (true)
                {
                    var start = input.Substring(0, 1);
                    value += input.Substring(1, 4);
                    if (Globals.debug)
                    {
                        Console.WriteLine($"Literal value piece starting bit: {start}");
                        Console.WriteLine($"Literal value piece four bits: {input.Substring(1, 4)}");
                    }
                    input = input.Substring(5);
                    if (start == "0")
                    {
                       if (Globals.debug)
                        {
                            Console.WriteLine($"Literal value bin: {value}");
                            Console.WriteLine($"Literal value dec: {bin2dec(value)}");
                        } 
                        break;
                    }
                }
                return (input, bin2dec(value));
            }
            else
            {
                var lengthTypeID = input.Substring(0, 1);
                var subpacketsValues = new List<Int64>();
                if (Globals.debug)
                    Console.WriteLine($"lengthTypeID: {lengthTypeID}");
                input = input.Substring(1);
                if (lengthTypeID == "0")
                {
                    var lengthSubpackets = Convert.ToInt32(bin2dec(input.Substring(0, 15)));
                    input = input.Substring(15);
                    var subpackets = input.Substring(0,lengthSubpackets);
                    if (Globals.debug)
                    {
                        Console.WriteLine($"lengthSubpackets bin: {input.Substring(0, 15)}");
                        Console.WriteLine($"lengthSubpackets dec: {lengthSubpackets}");
                        Console.WriteLine($"subpackets: {subpackets}");
                    }
                    while (subpackets.Length > 0)
                    {
                        (var s, var v) = parse(subpackets);
                        subpackets = s;
                        subpacketsValues.Add(v);
                    }
                    input = input.Substring(lengthSubpackets);
                }
                else
                {
                    var numberSubpackets = Convert.ToInt32(bin2dec(input.Substring(0, 11)));
                    if (Globals.debug)
                        Console.WriteLine($"numberSubpackets: {numberSubpackets}");
                    input = input.Substring(11);
                    foreach (int i in Enumerable.Range(1, numberSubpackets))
                    {
                        (var s, var v) = parse(input);
                        input = s;
                        subpacketsValues.Add(v);
                    }
                }
                switch (typeID)
                {
                    case 0:
                        return (input, subpacketsValues.Sum());
                    case 1:
                        return (input, subpacketsValues.Aggregate((total, next) => total * next));
                    case 2:
                        return (input, subpacketsValues.Min());
                    case 3:
                        return (input, subpacketsValues.Max());
                    case 5:
                        if (subpacketsValues[0] > subpacketsValues[1])
                            return (input, 1);
                        else
                            return (input, 0);
                    case 6:
                        if (subpacketsValues[0] < subpacketsValues[1])
                            return (input, 1);
                        else
                            return (input, 0);
                    case 7:
                        if (subpacketsValues[0] == subpacketsValues[1])
                            return (input, 1);
                        else
                            return (input, 0);
                }
            }
            Console.WriteLine("We're not supposed to be here. Something bad happened.");
            return (String.Empty, 0);
        }

        // Main program entry point
        static public void Main(string[] args)
        {
            // Obtain input from file
            string line;
            string[] lines;
            if (Globals.debug)
                lines = File.ReadAllLines("input_test3");
            else
                lines = File.ReadAllLines("input");
            line = lines[0];
            if (Globals.debug)
                Console.WriteLine($"Input as hex: {line}");

            // Convert hexadecimal input into binary
            line = hex2bin(line);

            if (Globals.debug)
                Console.WriteLine($"Input as bin: {line}");
            
            // Parse input
            (var a, var b) = parse(line);
            //(var a, var b) = parse(hex2bin("9C0141080250320F1802104A08"));

            // Part 1: Initialize variables
            var part1 = sumVersions;

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            var part2 = b;

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}