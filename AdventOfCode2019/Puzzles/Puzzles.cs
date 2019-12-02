using System;
using System.IO;

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
        public void Puzzle1()
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
        }
    }
}
