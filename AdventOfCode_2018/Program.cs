using System;
using System.Diagnostics;
using AdventOfCode_2018.Solutions;

namespace AdventOfCode_2018
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("../../Inputs/InputDay1.txt");
            Func<string, string> methodToRun = Day1.A;

            var result = methodToRun(input);

            System.Console.WriteLine(result);

            Console.Write("\nIf you want to test the execution time of the current method, enter a number of iterations: ");
            var readLine = Console.ReadLine();
            if (int.TryParse(readLine, out var numIterations))
            {
                Benchmark.DisplayTimerProperties();
                Console.WriteLine();
                Benchmark.TimeOperations(methodToRun, input, numIterations);
                Console.ReadKey();
            }            
        }
    }
}
