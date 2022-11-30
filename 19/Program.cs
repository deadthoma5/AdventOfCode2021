using System.Linq;
using System.Numerics;
using Combinatorics.Collections;

public static class Globals
{
    public static bool debug = false;
    public static bool testingPart1 = false;
}

namespace Day19
{
    class Scanner
    {
        public int id = new int();                                   // this scanner's unique ID per input
        public HashSet<Vector3> beacons = new HashSet<Vector3>();    // this scanner's set of relative positions for detected beacons
        public Vector3 position = new Vector3();                     // this scanner's calculated position relative to scanner 0 (origin)

        // use matrix multiplication to rotate a set of beacon positions by a specified angle
        public void rotate(Matrix4x4 rot)
        {
            beacons = beacons.Select(n => Vector3.Transform(n, rot)).ToHashSet();
            beacons = beacons.Select(n => roundVectorComponents(n)).ToHashSet();
        }

        // shift a set of beacon positions by a specified position vector
        public void translate(Vector3 offset)
        {
            beacons = beacons.Select(x => Vector3.Add(x, offset)).ToHashSet();
        }

        // to improve debugging readability, round vector components to the nearest integer, since C#'s rotation matrices have float/double components
        private Vector3 roundVectorComponents(Vector3 vector)
        {
            Vector3 rounded = new Vector3((float)Math.Round((double)vector.X), (float)Math.Round((double)vector.Y), (float)Math.Round((double)vector.Z));
            return rounded;
        }

        // Scanner constructor method
        public Scanner(int _id, HashSet<Vector3> _beacons)
        {
            id = _id;
            beacons = _beacons;
            if (id == 0)
                position = new Vector3(0, 0, 0);
        }
    }

    class Program
    {
        // parse input: one chunk of text = one Scanner object
        static private List<Scanner> parse(string[] lines)
        {
            var last = lines.Last();
            var chunk = new List<string>();
            var chunks = new List<List<string>>();
            var scanners = new List<Scanner>();

            foreach (string line in lines)
            {
                if (line != String.Empty)
                    chunk.Add(line);
                
                if (line == String.Empty || line == last)
                {
                    chunks.Add(chunk);
                    chunk = new List<string>();
                }
            }

            foreach (var c in chunks)
            {
                var id = new int();
                var x = new int();
                var y = new int();
                var z = new int();
                HashSet<Vector3> beacons = new HashSet<Vector3>();

                foreach (var line in c)
                {
                    if (line.Split(" ")[0] == "---")
                    {
                        id = Convert.ToInt32(line.Split(" ")[2]);
                    }
                    else
                    {
                        x = Convert.ToInt32(line.Split(",")[0]);
                        y = Convert.ToInt32(line.Split(",")[1]);
                        z = Convert.ToInt32(line.Split(",")[2]);
                        beacons.Add(new Vector3(x, y, z));
                    }
                }

                scanners.Add(new Scanner(id, beacons));
            }

            return scanners;
        }

        // Part 1: called by main program loop for rotating a candidate scanner to sync with another scanner already synced to scanner 0
        static private Tuple<Scanner,bool> syncRotation(Scanner reference, Scanner toSync)
        {
            bool isSynced = false;
            var bestRot = Matrix4x4.Identity;
            var bestDiffs = new int();
            var limit = new int();
            var flips = new Dictionary<string, Matrix4x4>();
            var rotations = new Dictionary<string, Matrix4x4>();
            var directions = new List<string>() { "x", "-x", "y", "-y", "z", "-z" };
            if (Globals.testingPart1)
                limit = 6;
            else
                limit = 12;

            flips["x"] = Matrix4x4.Identity;
            flips["-x"] = Matrix4x4.CreateRotationZ((float)Math.PI);
            flips["y"] = Matrix4x4.CreateRotationZ((float)Math.PI/2);
            flips["-y"] = Matrix4x4.CreateRotationZ((float)Math.PI*3/2);
            flips["z"] = Matrix4x4.CreateRotationY((float)Math.PI*3/2);
            flips["-z"] = Matrix4x4.CreateRotationY((float)Math.PI/2);

            rotations["x"] = Matrix4x4.CreateRotationX((float)Math.PI/2);
            rotations["-x"] = rotations["x"];
            rotations["y"] = Matrix4x4.CreateRotationY((float)Math.PI/2);
            rotations["-y"] = rotations["y"];
            rotations["z"] = Matrix4x4.CreateRotationZ((float)Math.PI/2);
            rotations["-z"] = rotations["z"];

            foreach (var direction in directions)
            {
                if (Globals.debug)
                {
                    Console.WriteLine($"\n============= Beacon: {toSync.id}, Direction: {direction} =============\n");
                }

                var rot = flips[direction];
                
                foreach (var _ in Enumerable.Range(0,4))
                {
                    Dictionary<Vector3,int> diffs = new Dictionary<Vector3, int>();
                    var tmp = new Scanner(toSync.id, toSync.beacons);
                    tmp.rotate(rot);

                    foreach (Vector3 b in reference.beacons)
                    {
                        foreach (Vector3 a in tmp.beacons)
                        {
                            if (diffs.ContainsKey(b-a))
                            {
                                diffs[b-a]++;
                            }
                            else
                                diffs.Add(b-a,1);
                        }
                    }

                    if (Globals.debug)
                        Console.WriteLine($"syncRotations score: {diffs.MaxBy(kvp => kvp.Value).Value}");

                    var offset = diffs.MaxBy(kvp => kvp.Value).Value;

                    if (offset > bestDiffs)
                    {
                        bestDiffs = offset;
                        bestRot = rot;
                        if (bestDiffs >= limit)
                            isSynced = true;
                    }
                    
                    if (isSynced)
                        break;

                    rot = Matrix4x4.Multiply(rotations[direction], rot);
                }

                if (isSynced)
                    break;
            }
            if (Globals.debug)
            {
                Console.WriteLine($"bestDiffs: {bestDiffs}");
                Console.WriteLine($"bestRot: {bestRot}");
            }

            var res = new Scanner(toSync.id, toSync.beacons);
            res.rotate(bestRot);
            return new Tuple<Scanner, bool>(res, isSynced);
        }

        // Part 1: called by main program loop for shifting a rotation-synced scanner to scanner 0's coordinate system
        static private Scanner syncTranslation(Scanner reference, Scanner toSync)
        {
            Scanner tmp = new Scanner(toSync.id, toSync.beacons);
            Dictionary<Vector3,int> diffs = new Dictionary<Vector3, int>();

            foreach (Vector3 b in reference.beacons)
            {
                foreach (Vector3 a in toSync.beacons)
                {
                    if (diffs.ContainsKey(b-a))
                    {
                        diffs[b-a]++;
                    }
                    else
                        diffs.Add(b-a,1);
                }
            }

            if (Globals.debug)
                Console.WriteLine($"syncTranslation diffs.MaxBy: {diffs.MaxBy(kvp => kvp.Value)}");
            var offset = diffs.MaxBy(kvp => kvp.Value).Key;
            tmp.position = offset;
            tmp.translate(offset);

            return tmp;
        }

        // Part 2: determine the maximum Manhattan Distance between any two scanners' positions
        private static int maxManhattanDistance(List<Vector3> positions)
        {
            if (Globals.debug)
            {
                foreach (var p in positions)
                {
                    Console.WriteLine($"scanner position: {p}");
                }
            }
            
            int maxManhattanDistance = 0;
            
            foreach (var a in positions)
            {
                foreach (var b in positions)
                {
                    if (a != b)
                    {
                        int d = (int)Math.Abs(b.X - a.X) + (int)Math.Abs(b.Y - a.Y) + (int)Math.Abs(b.Z - a.Z);
                        if (d > maxManhattanDistance)
                            maxManhattanDistance = d;
                    }
                }
            }

            return maxManhattanDistance;
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
            var scanners = parse(lines);
            
            // Part 1: Initialize variables
            var part1 = 0;
            Scanner mapped = new Scanner(-1, scanners[0].beacons);
            var candidates = new Queue<Scanner>();
            var positions = new List<Vector3>() {scanners[0].position};
        
            foreach (var s in scanners)
            {
                if (s.id != 0)
                    candidates.Enqueue(s);
            }
            
            // Part 1: Main program loop
            while (candidates.Count > 0)
            {
                var candidate = candidates.Dequeue();
                bool isSynced = false;
                (candidate, isSynced) = syncRotation(mapped, candidate);
                if (isSynced)
                {
                    candidate = syncTranslation(mapped, candidate);
                    positions.Add(candidate.position);
                    foreach (var b in candidate.beacons)
                        mapped.beacons.Add(b);
                }
                else
                    candidates.Enqueue(candidate);
            }

            part1 = mapped.beacons.Count;

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            var part2 = 0;

            // Part 2: Main program loop
            part2 = maxManhattanDistance(positions);

            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}