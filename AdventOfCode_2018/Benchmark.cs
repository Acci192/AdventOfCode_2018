using System;
using System.Diagnostics;

namespace AdventOfCode_2018
{
    public class Benchmark
    {
        /* Benchmark class used to determine the execution time of a method
         * These methods are based on the examples at Microsoft .NET API Documentation Stopwatch Class 
         * https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch?view=netframework-4.7.2
         */

        public static void DisplayTimerProperties()
        {
            Console.WriteLine(Stopwatch.IsHighResolution
                ? "Operations timed using the system's high-resolution performance counter."
                : "Operations timed using the DateTime class.");

            var frequency = Stopwatch.Frequency;
            Console.WriteLine($"    Timer frequency in ticks per second = {frequency}");
            var nanoSecPerTick = (1000L * 1000L * 1000L) / frequency;
            Console.WriteLine($"    Timer is accurate within {nanoSecPerTick} nanoseconds");
        }

        public static void TimeOperations(Func<string,string> testMethod, string input, int iterations)
        {
            var nanoSecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numIterations = iterations;

            var methodName = $"{testMethod.Method.DeclaringType?.Name}.{testMethod.Method.Name}";

            // Define variables for operation statistics.
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;
            int indexFastest = -1;
            int indexSlowest = -1;
            long milliSec = 0;

            Stopwatch totalTime = Stopwatch.StartNew();

            // Run the current operation numIterations+1 times.
            // The first execution time will be tossed
            // out, since it can skew the average time.
            for (var i = 0; i <= numIterations; i++)
            {
                long ticksThisTime = 0;
                Stopwatch timePerParse;


                // Start a new stopwatch timer.
                timePerParse = Stopwatch.StartNew();

                // Execute the Method to be tested
                testMethod(input);

                // Stop the timer, and save the
                // elapsed ticks for the operation.
                timePerParse.Stop();
                ticksThisTime = timePerParse.ElapsedTicks;


                // Skip over the time for the first operation,
                // just in case it caused a one-time
                // performance hit.
                if (i == 0)
                {
                    totalTime.Reset();
                    totalTime.Start();
                }
                else
                {
                    // Update operation statistics
                    // for iterations 1-numIterations+1.
                    if (maxTicks < ticksThisTime)
                    {
                        indexSlowest = i;
                        maxTicks = ticksThisTime;
                    }
                    if (minTicks > ticksThisTime)
                    {
                        indexFastest = i;
                        minTicks = ticksThisTime;
                    }

                    numTicks += ticksThisTime;
                }
            }

            // Display the statistics for numIterations iterations.
            totalTime.Stop();
            milliSec = totalTime.ElapsedMilliseconds;
            double averageInMillisec = (numTicks * nanoSecPerTick / numIterations) / 1000000;
            Console.WriteLine();
            Console.WriteLine($"{methodName} Summary:");
            Console.WriteLine($"  Slowest time:  #{indexSlowest}/{numIterations} = {maxTicks} ticks");
            Console.WriteLine($"  Fastest time:  #{indexFastest}/{numIterations} = {minTicks} ticks");
            Console.WriteLine($"  Average time:  {numTicks / numIterations} ticks = {(numTicks * nanoSecPerTick) / numIterations} nanoseconds (Approx. {averageInMillisec} milliseconds)");
            Console.WriteLine($"  Total time looping through {numIterations} operations: {milliSec} milliseconds");     
        }
    }
}
