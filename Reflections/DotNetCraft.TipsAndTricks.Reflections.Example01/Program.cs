using System;
using BenchmarkDotNet.Running;

namespace DotNetCraft.TipsAndTricks.Reflections.Example01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<PropertyGetAccessBenchmarks>();
            //Console.WriteLine(summary);

            var summary = BenchmarkRunner.Run<PropertySetAccessBenchmarks>();
            Console.WriteLine(summary);
        }
    }
}