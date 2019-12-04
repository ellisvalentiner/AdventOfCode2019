using System;
using System.IO;
using System.Collections.Generic;

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
            Console.WriteLine($"Day 1 - Part One: {total_fuel}\t Part Two: {total_fuel_plus_fuel}");
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

            Console.WriteLine($"Day 1 - Part One: {part_one}\t Part Two: {part_two}");
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
    }
}
