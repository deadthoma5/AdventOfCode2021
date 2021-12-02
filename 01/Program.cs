/*
 * Part 1
 */

string[] lines = File.ReadAllLines("input");

int prev = 0;
int count_part1 = 0;

foreach(string line in lines)
{
    if (prev == 0)
    {
        prev = Int32.Parse(line);
        continue;
    }
    else
    {
        int current = Int32.Parse(line);
        if (current - prev > 0)
            count_part1++;
        prev = current;
    }
}

Console.WriteLine(count_part1);


/*
 * Part 2
 * Shortcut: current window - prev window = (x3 + x2 + x1) - (x2 + x1 + x0) = x3 - x0
 */

int count_part2 = 0;

for (int i = 3; i < lines.Length; i++)
{
    if (Int32.Parse(lines[i]) - Int32.Parse(lines[i-3]) > 0)
        count_part2++;
}

Console.WriteLine(count_part2);