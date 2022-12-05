using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

public static class Globals
{
    public static bool debug = false;
}

namespace Day22
{
    class Cuboid {
        public int x0;
        public int x1;
        public int y0;
        public int y1;
        public int z0;
        public int z1;

        public int sign;

        public Cuboid(int _sign, int _x0, int _x1, int _y0, int _y1, int _z0, int _z1) {
            sign = _sign;
            x0 = _x0;
            x1 = _x1;
            y0 = _y0;
            y1 = _y1;
            z0 = _z0;
            z1 = _z1;
        }

        public long volume() {
            return Convert.ToInt64(x1 - x0 + 1) * Convert.ToInt64(y1 - y0 + 1) * Convert.ToInt64(z1 - z0 + 1) * Convert.ToInt64(sign);
        }
        public void print() {
            Console.WriteLine($"sign: {sign}, (x0, x1, y0, y1, z0, z1): ({x0}, {x1}, {y0}, {y1}, {z0}, {z1}), volume: {volume()}");
        }
    }
    class Program
    {
        // Main program entry point
        static public void Main(string[] args)
        {
            // Obtain input from file
            string[] lines;
            if (Globals.debug)
                lines = File.ReadAllLines("input_test_large");
            else
                lines = File.ReadAllLines("input");

            // Part 1: Initialize variables
            var boundary = 50;
            var dim = 2 * boundary + 1;
            var grid = new Dictionary<(int, int, int), int>();
            foreach (var x in Enumerable.Range(-boundary, dim))
                foreach (var y in Enumerable.Range(-boundary, dim))
                    foreach (var z in Enumerable.Range(-boundary, dim)) {
                        var pos = (x, y, z);
                        grid[pos] = 0;
                    }

            // Part 1: Main loop
            foreach (var line in lines) {
                (int sign, int x0, int x1, int y0, int y1, int z0, int z1) = parse(line);
                (x0, x1, y0, y1, z0, z1) = bounds(x0, x1, y0, y1, z0, z1, boundary);

                for (int x = x0; x <= x1; x++)
                    for (int y = y0; y <= y1; y++)
                        for (int z = z0; z <= z1; z++) {
                            var pos = (x, y, z);
                            if (sign > 0)
                                grid[pos] = sign;
                            else
                                grid[pos] = 0;
                        }
            }
            
            var part1 = sum(grid, boundary, dim);

            // Part 1: Display results
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Initialize variables
            long part2 = 0;
            var cubes = new Dictionary<Tuple<int, int, int, int, int, int>, int>();

            // Part 2: Main loop
            foreach (var line in lines) {
                (int nsign, int nx0, int nx1, int ny0, int ny1, int nz0, int nz1) = parse(line);
                var new_coord = new Tuple<int, int, int, int, int, int>(nx0, nx1, ny0, ny1, nz0, nz1);
                var new_cube = new Cuboid(nsign, nx0, nx1, ny0, ny1, nz0, nz1);
                var new_cubes = new Dictionary<Tuple<int, int, int, int, int, int>, int>();

                foreach (var existing_coord in cubes.Keys) {
                    (var ex0, var ex1, var ey0, var ey1, var ez0, var ez1) = existing_coord;
                    var existing_cube = new Cuboid(cubes[existing_coord], ex0, ex1, ey0, ey1, ez0, ez1);
                    if (intersects(new_cube, existing_cube)) {
                        var i_cube = intersect(new_cube, existing_cube);
                        var i_coord = new Tuple<int, int, int, int, int, int>(i_cube.x0, i_cube.x1, i_cube.y0, i_cube.y1, i_cube.z0, i_cube.z1);
                        if (new_cubes.ContainsKey(i_coord))
                            new_cubes[i_coord] += i_cube.sign;
                        else
                            new_cubes.Add(i_coord, i_cube.sign);
                    }
                }

                if (new_cube.sign > 0) {
                    if (new_cubes.ContainsKey(new_coord))
                        new_cubes[new_coord] += new_cube.sign;
                    else
                        new_cubes.Add(new_coord, new_cube.sign);
                }

                foreach (var coord in new_cubes.Keys) {
                    if (cubes.ContainsKey(coord))
                        cubes[coord] += new_cubes[coord];
                    else
                        cubes.Add(coord, new_cubes[coord]);
                }               
            }

            foreach (var coord in cubes.Keys) {
                (var x0, var x1, var y0, var y1, var z0, var z1) = coord;
                var cube = new Cuboid(cubes[coord], x0, x1, y0, y1, z0, z1);

                part2 += cube.volume();
            }
            
            // Part 2: Display results         
            Console.WriteLine($"Part 2: {part2}");
        }

        // Parse a line of input into sign and coords
        private static (int, int, int, int, int, int, int) parse(string input) {
            string pattern = @"(off|on)( x=)(-?\d+)(..)(-?\d+)(,y=)(-?\d+)(..)(-?\d+)(,z=)(-?\d+)(..)(-?\d+)";
            Regex rg = new Regex(pattern);
            MatchCollection matches = rg.Matches(input);

            var sign = (matches[0].Groups[1].Value == "on") ? 1 : -1;
            var x0 = Convert.ToInt32(matches[0].Groups[3].Value);
            var x1 = Convert.ToInt32(matches[0].Groups[5].Value);
            var y0 = Convert.ToInt32(matches[0].Groups[7].Value);
            var y1 = Convert.ToInt32(matches[0].Groups[9].Value);
            var z0 = Convert.ToInt32(matches[0].Groups[11].Value);
            var z1 = Convert.ToInt32(matches[0].Groups[13].Value);

            return (sign, x0, x1, y0, y1, z0, z1);
       }

        // restrict each axis to -50, 50 for Part 1
        private static (int, int, int, int, int, int) bounds(int x0, int x1, int y0, int y1, int z0, int z1, int boundary)
        {
            if (x0 < -boundary) x0 = -boundary;
            if (x1 > boundary) x1 = boundary;
            if (y0 < -boundary) y0 = -boundary;
            if (y1 > boundary) y1 = boundary;
            if (z0 < -boundary) z0 = -boundary;
            if (z1 > boundary) z1 = boundary;

            return (x0, x1, y0, y1, z0, z1);
        }

        // Count the number of lights for Part 1
        private static long sum(Dictionary<(int, int, int), int> grid, int boundary, int dim)
        {
            long sum = 0;

            foreach (var x in Enumerable.Range(-boundary, dim))
                foreach (var y in Enumerable.Range(-boundary, dim))
                    foreach (var z in Enumerable.Range(-boundary, dim))
                        sum += grid[(x, y, z)];

            return sum;
        }
        
        // Determine if two Cuboids intersect for Part 2
        private static bool intersects(Cuboid n, Cuboid e) {
            var ix0 = Math.Max(n.x0, e.x0); var ix1 = Math.Min(n.x1, e.x1);
            var iy0 = Math.Max(n.y0, e.y0); var iy1 = Math.Min(n.y1, e.y1);
            var iz0 = Math.Max(n.z0, e.z0); var iz1 = Math.Min(n.z1, e.z1);

            if (ix0 <= ix1 && iy0 <= iy1 && iz0 <= iz1)
                return true;
            else
                return false;
        }

        // Determine an intersection Cuboid of negative volume for Part 2
        private static Cuboid intersect(Cuboid n, Cuboid e) {
            int isign = -e.sign;
            var ix0 = Math.Max(n.x0, e.x0); var ix1 = Math.Min(n.x1, e.x1);
            var iy0 = Math.Max(n.y0, e.y0); var iy1 = Math.Min(n.y1, e.y1);
            var iz0 = Math.Max(n.z0, e.z0); var iz1 = Math.Min(n.z1, e.z1);

            return new Cuboid(isign, ix0, ix1, iy0, iy1, iz0, iz1);
        }
    }
}