using System.Linq;

public static class Globals
{
    public static bool debug = false;
}

namespace Day16
{
    class Program
    {
        public static int sumVersions = 0;
        // Convert hexadecimal string to binary string
        static private string hex2bin(string hexString)
        {
            string binaryString = String.Join(String.Empty,
                hexString.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );
            return binaryString;
        }

        // Convert binary string to decimal number
        static private int bin2dec(string binaryString)
        {
            return Convert.ToInt32(binaryString, 2);
        }

        static private string parse(string input)
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
            }
            else
            {
                var lengthTypeID = input.Substring(0, 1);
                if (Globals.debug)
                    Console.WriteLine($"lengthTypeID: {lengthTypeID}");
                input = input.Substring(1);
                if (lengthTypeID == "0")
                {
                    var lengthSubpackets = bin2dec(input.Substring(0, 15));
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
                        subpackets = parse(subpackets);
                    }
                    input = input.Substring(lengthSubpackets);
                }
                else
                {
                    var numberSubpackets = bin2dec(input.Substring(0, 11));
                    if (Globals.debug)
                        Console.WriteLine($"numberSubpackets: {numberSubpackets}");
                    input = input.Substring(11);
                    foreach (int i in Enumerable.Range(1, numberSubpackets))
                    {
                        input = parse(input);
                    }
                }
            }
            return input;
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
            parse(line);

            // Part 1: Initialize variables
            var part1 = sumVersions;

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            var part2 = 0;

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}