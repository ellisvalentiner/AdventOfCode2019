using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class TimingRecord
    {

        public TimingRecord(string puzzle, List<TimeSpan> elapsed)
        {
            Puzzle = puzzle;
            Minimum = elapsed.Min();
            Mean = new TimeSpan(Convert.ToInt64(elapsed.Average((TimeSpan arg) => arg.Ticks)));
            Maximum = elapsed.Max();
        }

        public string Puzzle { get; set; }
        public TimeSpan Minimum { get; set; }
        public TimeSpan Mean { get; set; }
        public TimeSpan Maximum { get; set; }
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

        public int[] Puzzle4()
        {
            string input = "367479-893698";
            int[] bounds = input
                .Split("-")
                .ToList()
                .Select(o => int.Parse(o))
                .ToArray();
            int count_valid_passwords = 0;
            int count_valid_part_two_passwords = 0;

            foreach (int candidate_password in Enumerable.Range(bounds[0], bounds[1] - bounds[0]))
            {
                bool[] valid = IsValidPassword(candidate_password, bounds[0], bounds[1]);
                if (valid[0])
                {
                    count_valid_passwords++;
                }
                if (valid[1])
                {
                    count_valid_part_two_passwords++;
                }
            }
            return new int[] { count_valid_passwords, count_valid_part_two_passwords };
        }

        private bool[] IsValidPassword(int password, int start, int end)
        {
            /* Password rules:
             * - six-digit number
             * - within the given range
             * - two adjacent digits are the same
             * - from left to right digits never decrease
             */
            int[] digits = password
                .ToString()
                .Select(o => (int)char.GetNumericValue(o))
                .ToArray();

            // Six-digit number
            if (digits.Count() != 6)
            {
                return new bool[] { false, false };
            }

            // Within the given range
            if (password < start || end < password)
            {
                return new bool[] { false, false };
            }

            // From left to right digits never decrease
            for (int i = 0; i < 5; i++)
            {
                if (digits[i + 1] < digits[i])
                {
                    return new bool[] { false, false };
                }
            }

            // Two adjacent digits are the same
            if (digits.Distinct().Count() == 6)
            {
                return new bool[] { false, false };
            }
            if (digits.GroupBy((o) => o).Select(o => o.Count()).Any(o => o == 2))
            {
                return new bool[] { true, true };
            }

            return new bool[] { true, false };
        }
    }
}
