using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using MarkdownLog;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main()
        {
            Puzzles puzzles = new Puzzles();
            var methods = puzzles.GetType().GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly |
                BindingFlags.Static
            );
            List<TimingRecord> timing_data = new List<TimingRecord> { };
            foreach (var method in methods)
            {
                var timings = new List<TimeSpan> { };
                for (int i = 0; i <= 5; i++)
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    method.Invoke(puzzles, null);
                    stopWatch.Stop();
                    timings.Add(stopWatch.Elapsed);
                }
                timing_data.Add(new TimingRecord(method.Name, timings));
            }
            File.WriteAllText("README.md", @$"
# Advent of Code 2019

## Performance

{timing_data.ToMarkdownTable()}
");
        }
    }
}
