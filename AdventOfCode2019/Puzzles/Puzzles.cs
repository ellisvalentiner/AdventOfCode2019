using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class TimingRecord
    {
        public TimingRecord(string puzzle, string elapsed)
        {
            Puzzle = puzzle;
            Elapsed = elapsed;
        }

        public string Puzzle { get; set; }
        public string Elapsed { get; set; }
    }

    public class Puzzles
    {
        public int[] Puzzle1()
        {
            // Initialize total
            int total_fuel = 0;
            int total_fuel_plus_fuel = 0;

            // Read input
            foreach (string line in File.ReadAllLines("Data/day-1.txt"))
            {
                int input_mass = Int32.Parse(line);
                int fuel = (input_mass / 3) - 2;
                total_fuel += fuel;
                while (fuel > 0)
                {
                    total_fuel_plus_fuel += fuel;
                    fuel = (fuel / 3) - 2;
                }
            }
            Console.WriteLine($"Day 1\tPart One: {total_fuel}\tPart Two: {total_fuel_plus_fuel}");
            return new int[] {total_fuel, total_fuel_plus_fuel };
        }

        public int[] Puzzle2()
        {
            // Initialize vars
            List<int> intcode = new List<int>();

            // Prepare
            foreach (string line in File.ReadAllLines("Data/day-2.txt"))
            {
                foreach (string val in line.Split(","))
                {
                    intcode.Add(Int32.Parse(val));
                }
            }

            // Part One
            int part_one = IntcodeEvaluator(new List<int>(intcode), 12, 2);

            // Part Two
            int part_two = 0;
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    if (IntcodeEvaluator(new List<int>(intcode), noun, verb) == 19690720)
                    {
                        part_two = 100 * noun + verb;
                        break;
                    }
                }
            }

            Console.WriteLine($"Day 2\tPart One: {part_one}\tPart Two: {part_two}");
            return new int[] {part_one, part_two };
        }

        private int IntcodeEvaluator(List<int> intcode, int noun = 12, int verb = 2)
        {
            intcode[1] = noun;
            intcode[2] = verb;
            int opcode = 0;
            int pointer = 0;
            while (opcode != 99)
            {
                opcode = intcode[pointer];
                switch (opcode)
                {
                    case 1:
                        intcode[intcode[pointer + 3]] = intcode[intcode[pointer + 1]] + intcode[intcode[pointer + 2]];
                        pointer += 4;
                        continue;
                    case 2:
                        intcode[intcode[pointer + 3]] = intcode[intcode[pointer + 1]] * intcode[intcode[pointer + 2]];
                        pointer += 4;
                        continue;
                    case 99:
                        break;
                    default:
                        throw new InvalidProgramException("Bad opcode!");
                }
            }
            return intcode[0];
        }

        public int[] Puzzle3()
        {
            // Read data
            string[] instructions = File.ReadAllLines("Data/day-3.txt");
            string[] wire_path_1 = instructions[0].Split(",");
            string[] wire_path_2 = instructions[1].Split(",");

            // Part wire path and return coordinates
            List<Coord> path1 = WirePathToCoords(wire_path_1);
            List<Coord> path2 = WirePathToCoords(wire_path_2);

            // Find closest intersection
            Coord origin = new Coord(0, 0);
            int distance_to_nearest_intersection = 999999;
            int fewest_steps_to_intersection = 999999;
            var path_intersections = path1.Intersect(path2);
            foreach (Coord coord in path_intersections)
            {
                int dist = ManhattanDistance(coord, origin);
                distance_to_nearest_intersection = Math.Min(dist, distance_to_nearest_intersection);
                fewest_steps_to_intersection = Math.Min(path1.IndexOf(coord) + path2.IndexOf(coord) + 2, fewest_steps_to_intersection);
            }
            Console.WriteLine($"Day 3\tPart One: {distance_to_nearest_intersection}\tPart Two: {fewest_steps_to_intersection}");
            return new int[] { distance_to_nearest_intersection, fewest_steps_to_intersection};
        }

        struct Coord
        {
            public int x, y;

            public Coord(int p1, int p2)
            {
                x = p1;
                y = p2;
            }
        }

        private List<Coord> WirePathToCoords(string[] wirepath)
        {
            List<Coord> wire_path_coords = new List<Coord> { };
            int x = 0;
            int y = 0;
            foreach (string step in wirepath)
            {
                switch (step[0])
                {
                    case 'R':
                        for (int i = 0; i < int.Parse(step.Substring(1)); i++)
                        {
                            x++;
                            wire_path_coords.Add(new Coord(x, y));
                        }
                        continue;
                    case 'U':
                        for (int i = 0; i < int.Parse(step.Substring(1)); i++)
                        {
                            y++;
                            wire_path_coords.Add(new Coord(x, y));
                        }
                        continue;
                    case 'L':
                        for (int i = 0; i < int.Parse(step.Substring(1)); i++)
                        {
                            x--;
                            wire_path_coords.Add(new Coord(x, y));
                        }
                        continue;
                    case 'D':
                        for (int i = 0; i < int.Parse(step.Substring(1)); i++)
                        {
                            y--;
                            wire_path_coords.Add(new Coord(x, y));
                        }
                        continue;
                    default:
                        throw new InvalidDataException("Unexpected direction");
                }
            }
            return wire_path_coords;
        }

        private int ManhattanDistance(Coord coord1, Coord coord2)
        {
            return Math.Abs(coord1.x - coord2.x) + Math.Abs(coord1.y - coord2.y);
        }
    }
}
