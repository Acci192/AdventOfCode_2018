using System;
using AdventOfCode_2018.Solutions;

namespace AdventOfCode_2018
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("../../Inputs/InputDay2.txt");
            Func<string, string> methodToRun = Day2.B;

            var result = methodToRun(input);

            Console.WriteLine(result);

            Console.Write("\nIf you want to test the execution time of the current method, enter a number of iterations: ");
            var readLine = Console.ReadLine();

            if (!int.TryParse(readLine, out var numIterations)) return;

            Benchmark.DisplayTimerProperties();
            Console.WriteLine();
            Benchmark.TimeOperations(methodToRun, input, numIterations);
            Console.ReadKey();
        }
    }
}
