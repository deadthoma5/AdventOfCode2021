/*
 * Part 1
 */

bool debug = false;
string[] lines;
if (debug)
    lines = File.ReadAllLines("input_test");
else
    lines = File.ReadAllLines("input");

int position = 0;
int depth = 0;
int part1 = 0;

foreach(string line in lines)
{
    string[] lineParts = line.Split(' ');
    string command = lineParts[0];
    int value = Int32.Parse(lineParts[1]);
    if (command == "forward")
        position += value;
    else
    {
        if (command == "down")
            depth += value;
        else
        {
            if (command == "up")
            depth -= value;
        }
    }
}

part1 = position * depth;
Console.WriteLine(part1);


/*
 * Part 2
 */

position = 0;
depth = 0;
int aim = 0;
int part2 = 0;

foreach(string line in lines)
{
    string[] lineParts = line.Split(' ');
    string command = lineParts[0];
    int value = Int32.Parse(lineParts[1]);
    if (command == "forward")
    {
        position += value;
        depth += value * aim;
        if (debug)
        {
            Console.WriteLine("command: {0}, value: {1}, position: {2}, depth: {3}, aim: {4}", command, value, position, depth, aim);
        }
    }
    else
    {
        if (command == "down")
            aim += value;
        else
        {
            if (command == "up")
            aim -= value;
        }
    }
}

part2 = position * depth;
Console.WriteLine(part2);