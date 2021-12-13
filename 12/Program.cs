public static class Globals
{
    public static bool debug = false;
    public static Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
    public static HashSet<List<string>> all_paths = new HashSet<List<string>>();
}

namespace Day12
{
    class Program
    {
        // Build the caves graph from input
        static public Dictionary<string, List<string>> GetGraph(string[] lines)
        {
            var graph = new Dictionary<string, List<string>>();
            foreach (string line in lines)
            {
                string[] split = line.Split('-');
                var node = split[0];
                var destination = split[1];

                // Add a graph edge for a given entry
                if (!graph.ContainsKey(node))
                    graph.Add(node, new List<string>());
                graph[node].Add(destination);

                // Add a graph edge for the reverse of a given entry
                if (!graph.ContainsKey(destination))
                    graph.Add(destination, new List<string>());
                graph[destination].Add(node);
            }
            return graph;
        }

        // Part 1: Determine all possible paths without revisiting small caves
        // Reference: Depth First Search (DFS) algorithm: https://www.askpython.com/python/examples/depth-first-search-algorithm
        static public void GetPaths(string node, HashSet<string> visited, List<string> path)
        {
            path.Add(node);

            if (node.ToLower() == node)
                visited.Add(node);
            
            if (node == "end")    // Recursion base case: we've reached the path's end at "end"
                Globals.all_paths.Add(new List<string>(path));
            else
            {
                foreach (var destination in Globals.graph[node])
                {
                    if (!visited.Contains(destination))
                        GetPaths(destination, visited, path);
                }
            }

            // backtrack a step when unfolding a recursive layer, to continue down another path
            path.RemoveAt(path.Count - 1);
            if (node.ToLower() == node)
                visited.Remove(node);

            return;
        }

        // Part 2: Determine all possible paths with the ability to revisit one small cave twice
        // Note: Modified DFS algorithm from Part 1. Lots of debugging output since it was hard for me to figure out and get right.
        static public void GetPaths2(string node, HashSet<string> visited, List<string> path, bool visited_small = false)
        {
            path.Add(node);
            if (Globals.debug) Console.WriteLine($"node {node} added to path");
            if (node.ToLower() == node)
            {
                if (node == "start" || node == "end")
                {
                    visited.Add(node);
                    if (Globals.debug) Console.WriteLine($"start || end: adding {node} to visited");
                }
                else if (visited_small)
                {
                    visited.Add(node);
                    if (Globals.debug) Console.WriteLine($"visited_small was true. added {node} to visited");
                }
                
                // if a small cave has been visited before
                if (path.Where(n => n !=null && n.StartsWith(node)).Count() > 1)
                {
                    visited_small = true;
                    visited.Add(node);
                    if (Globals.debug) Console.WriteLine($"visited_small set to true and added {node} to visited");

                    // once a small cave has been visited twice, other small caves become ineligible for revisiting
                    foreach (string lowernode in path)
                    {
                        if (lowernode.ToLower() == lowernode && lowernode != node && lowernode != "start" && lowernode != "end")
                        {
                            visited.Add(lowernode);
                            if (Globals.debug) Console.WriteLine($"since visited_small is now true, correcting visited by adding {lowernode}");
                        }
                    }
                }
            }
  
            if (node == "end")    // Recursion base case: we've reached the path's end at "end"
            {
                Globals.all_paths.Add(new List<string>(path));
                if (Globals.debug) Console.WriteLine($"Added Path: {string.Join(" ", path)}");
            }
            else
            {
                foreach (var destination in Globals.graph[node].OrderBy(n => n))
                {
                    // This cleanup was key -- when unfolding each recursive call, sometimes a small cave would be eligible again for revisiting
                    if (visited.Contains(destination) && destination.ToLower() == destination && visited_small == false && destination != "start" && destination != "end")
                        visited.Remove(destination);
                    if (Globals.debug) Console.WriteLine($"considering destination {destination}, node: {node}, visited: {string.Join(" ", visited)}, path: {string.Join(" ", path)}, visited_small: {visited_small}");
                    if (!visited.Contains(destination))
                    {
                        if (Globals.debug) Console.WriteLine($"node: {node}, destination: {destination}, visited: {string.Join(" ", visited)}, path: {string.Join(" ", path)}, visited_small: {visited_small}");
                        GetPaths2(destination, visited, path, visited_small);
                    }
                }
            }

            // backtrack a step when unfolding a recursive layer, to continue down another path
            path.RemoveAt(path.Count - 1);
            if (node.ToLower() == node)
            {
                visited.Remove(node);
                if (Globals.debug) Console.WriteLine($"removed {node} from visited");
            }

            return;
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
            
            // Parse input
            Globals.graph = GetGraph(lines);
            if (Globals.debug)
            {
                Console.WriteLine("Edges:");
                foreach (var entry in Globals.graph)
                    foreach (var destination in entry.Value)
                        Console.WriteLine($"    {entry.Key} -> {destination}");
                Console.WriteLine();
            }
            
            // Part 1: Initialize variables
            var visited = new HashSet<string>();
            var path = new List<string>();

            // Part 1: Main program loop
            // Reference: Depth First Search (DFS) algorithm: https://favtutor.com/blogs/depth-first-search-python
            GetPaths("start", visited, path);
            if (Globals.debug)
            {
                Console.WriteLine("Paths:");
                foreach (var p in Globals.all_paths) Console.WriteLine("    " + string.Join(",", p));
                Console.WriteLine();
            }

            // Part 1: Calculate results
            int part1 = Globals.all_paths.Count;
            
            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine();

            // Part 2: Initialize variables
            visited = new HashSet<string>();
            var visited_lower = new HashSet<string>();
            path = new List<string>();
            Globals.all_paths = new HashSet<List<string>>();

            // Part 2: Main program loop
            GetPaths2("start", visited, path);
            if (Globals.debug)
            {
                Console.WriteLine("Paths:");
                foreach (var p in Globals.all_paths) Console.WriteLine("    " + string.Join(",", p));
                Console.WriteLine();
            }

            // Part 2: Calculate results
            int part2 = Globals.all_paths.Count;

            // Part 2: Display results
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
