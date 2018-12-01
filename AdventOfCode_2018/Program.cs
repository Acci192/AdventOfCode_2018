using System;

namespace AdventOfCode_2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("../../Inputs/InputDay1.txt");
            var result = Day1.B(input);

            System.Console.WriteLine(result);

            System.Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
