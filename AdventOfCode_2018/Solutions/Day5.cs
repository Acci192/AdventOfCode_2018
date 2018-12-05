using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day5
    {
        private static int RemoveOppositePolarities(string input)
        {
            var sb = new StringBuilder(input);
            for (var i = 0; i < sb.Length - 1; i++)
            {
                if ((sb[i] == sb[i + 1] - 32) || (sb[i] == sb[i + 1] + 32))
                {
                    sb.Remove(i, 2);
                    i -= 2;
                    if (i < 0) i = -1;
                }
            }

            return sb.Length;
        }
        public static string A(string input)
        {
            var sb = new StringBuilder(input);

            return $"{RemoveOppositePolarities(input)}";
        }

        public static string B(string input)
        {
            var minLength = int.MaxValue;

            for (var c = 'a'; c <= 'z'; c++)
            {
                var sb = new StringBuilder(input).Replace($"{c}", "");
                sb.Replace($"{(char)(c - 32)}", "");
                
                var sbLength = RemoveOppositePolarities(sb.ToString());
                if (sbLength < minLength)
                {
                    minLength = sbLength;
                }
            }

            return $"{minLength}";
        }
    }
}
