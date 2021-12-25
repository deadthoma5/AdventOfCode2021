public static class Globals
{
    public static bool debug = false;
}

namespace Day15
{
    class Program
    {
        static public Node[][] InitMap1()
        {
            // Obtain input from file
            string[] lines;
            if (Globals.debug)
                lines = File.ReadAllLines("input_test");
            else
                lines = File.ReadAllLines("input");

            // Parse input into a map grid
            Node[][] map1 = lines
                .Select((row, i) => row
                    .Select((n, j) => new Node { X = j, Y = i, Risk = Convert.ToInt32(n.ToString()) })
                    .ToArray())
                .ToArray();
            
            if (Globals.debug)
            {
                Console.WriteLine("Map 1:");
                for (int row = 0; row < map1.Length; row++)
                {
                    for (int col = 0; col < map1.Length; col++)
                    {
                        Console.Write(map1[row][col].Risk);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            return map1;
        }

        static public Node[][] InitMap2(Node[][] map1)
        {
            // Parse input into a map grid
            Node[][] map2 = new Node[map1.Length * 5][];
            for (int i = 0; i < map2.Length; i++)
                map2[i] = new Node[map1.Length * 5];

            for (int shifty = 0; shifty < 5; shifty++)
            {
                for (int shiftx = 0; shiftx < 5; shiftx++)
                {
                    for (int y = 0; y < map1.Length; y++)
                    {
                        for (int x = 0; x < map1.Length; x++)
                        {
                            var newY = y + shifty * map1.Length;
                            var newX = x + shiftx * map1.Length;
                            var newRisk = (map1[y][x].Risk + shifty + shiftx - 1) % 9 + 1;
                            map2[newY][newX] = new Node() { X = newX, Y = newY, Risk = newRisk };
                        }
                    }
                }
            }

            if (Globals.debug)
            {
                Console.WriteLine("\n\nMap 2:");
                for (int row = 0; row < map2.Length; row++)
                {
                    for (int col = 0; col < map2.Length; col++)
                    {
                        Console.Write(map2[row][col].Risk);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            return map2;
        }

        // Dijkstra's algorithm
        static public int Dijkstra(Node[][] map, Node start, Node target)
        {
            // C# PriorityQueue pops lowest priority item in the queue
            var toVisit = new PriorityQueue<Node, int>();

            // Starting Node is free
            map[start.Y][start.X].Cost = 0;
            toVisit.Enqueue(map[start.Y][start.X], map[start.Y][start.X].Cost);

            while (toVisit.Count > 0)
            {
                var curr = toVisit.Dequeue();
                curr.Visited = true;

                // Find current node's neighbors if they haven't been visited yet
                var neighbors = curr.GetNeighbors(map);
                foreach (var neighbor in neighbors)
                {
                    var tentativeCost = curr.Cost + neighbor.Risk;

                    // If it's more expensive to get to the neighbor from this Node compared to other possible paths, skip this path
                    if (tentativeCost >= neighbor.Cost)
                        continue;
                    else
                    {
                        // Update the neighbor's Cost with the new cheapest Cost
                        neighbor.Cost = tentativeCost;

                        // The neighbor's Parent is now this Node as it's the new cheapest path
                        neighbor.Parent = curr;

                        // Queue up the neighbor to visit later
                        toVisit.Enqueue(neighbor, neighbor.Cost);
                    }
                }
            }

            if (Globals.debug)
            {
                Console.WriteLine($"Target: X = {target.X}, Y = {target.Y}");
                var debug_parent = map[target.Y][target.X].Parent;
                Console.WriteLine($"Parent: X = {debug_parent.X}, Y = {debug_parent.Y}");
                do
                {
                    Console.WriteLine($"Parent: X = {debug_parent.X}, Y = {debug_parent.Y}");
                    debug_parent = map[debug_parent.Y][debug_parent.X].Parent;
                } while (debug_parent.X != start.X || debug_parent.Y != start.Y);
                Console.WriteLine($"Start: X = {debug_parent.X}, Y = {debug_parent.Y}");
                Console.WriteLine();
            }

            return map[target.Y][target.X].Cost;
        }

        // Main program entry point
        static public void Main(string[] args)
        {
            // Part 1: Initialize variables
            var map1 = InitMap1();     // input map from file
            var start = new Node { X = 0, Y = 0};
            var target = new Node { X = map1.Length - 1, Y = map1.Length - 1 };

            // Part 1: Main program loop
            int part1 = Dijkstra(map1, start, target);

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            var map2 = InitMap2(map1);     // new map based on extending Part 1
            start = new Node { X = 0, Y = 0};
            target = new Node { X = map2.Length - 1, Y = map2.Length - 1 };

            // Part 2: Display results
            int part2 = Dijkstra(map2, start, target);           
            Console.WriteLine($"Part 2: {part2}");
        }
    }

    class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Risk { get; set; }    // The value from input
        public int Cost { get; set; } = int.MaxValue;    // Treat int.MaxValue as "infinity"
        public bool Visited { get; set; }
        public Node Parent { get; set; }

        public List<Node> GetNeighbors(Node[][] map)
        {
            var neighbors = new List<Node>();
            
            if (Y > 0 && !map[Y - 1][X].Visited)
                neighbors.Add(map[Y - 1][X]);    // up
            if (Y < map.Length - 1 && !map[Y + 1][X].Visited)
                neighbors.Add(map[Y + 1][X]);    // down
            if (X > 0 && !map[Y][X - 1].Visited)
                neighbors.Add(map[Y][X - 1]);    // left
            if (X < map.Length - 1 && !map[Y][X + 1].Visited)
                neighbors.Add(map[Y][X + 1]);    // right
            
            return neighbors;
        }
    }
}

// Dijkstra's algorithm references:
// https://en.wikipedia.org/wiki/Dijkstra's_algorithm
// https://www.udacity.com/blog/2021/10/implementing-dijkstras-algorithm-in-python.html
