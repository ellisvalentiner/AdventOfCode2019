using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using MarkdownLog;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Puzzles puzzles = new Puzzles();
            var methods = puzzles.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);

            var data = new List<TimingRecord>();
            foreach (var method in methods)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                method.Invoke(puzzles, null);
                stopWatch.Stop();
                data.Add(new TimingRecord(method.Name, stopWatch.Elapsed.ToString("c")));
            }
            Console.WriteLine("# Advent of Code 2019");
            Console.WriteLine(data.ToMarkdownTable());
        }
    }
}
