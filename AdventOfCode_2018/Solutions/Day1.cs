using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2018.Solutions
{
    public class Day1
    {
        public static string A (string input)
        {
            var result = input.Split('\n').Sum(int.Parse);

            return result.ToString();
        }

        public static string B (string input)
        {
            var result = 0;
            var results = new HashSet<int>();
            while (true)
            {
                foreach(var i in input.Split('\n'))
                {
                    result += int.Parse(i);
                    if (!results.Add(result))
                        return result.ToString();
                }
            }
        }
    }
}
