using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode_2018.Solutions
{
    public class Day2
    {
        public static string A(string input)
        {
            var rows = input.Split('\n').Select(x => x.Replace("\r", "")).ToList();
            var two = 0;
            var three = 0;
            foreach (var row in rows)
            {
                var unique = row.Distinct().ToList();
                if (unique.Count(x => row.Count(r => r == x) == 2) > 0)
                {
                    two++;
                }

                if (unique.Count(x => row.Count(r => r == x) == 3) > 0)
                {
                    three++;
                }
            }

            return $"{two * three}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var uniqueRows = new HashSet<string>(rows);

            foreach (var row in rows)
            {
                var arr = row.ToCharArray();
                for (var i = 0; i < arr.Length; i++)
                {
                    for (var c = 'a'; c <= 'z'; c++)
                    {
                        var originalChar = arr[i];
                        arr[i] = c;
                        var temp = new string(arr);
                        if (temp != row)
                        {
                            if (uniqueRows.Contains(temp))
                            {
                                var result = new StringBuilder(row);
                                result.Remove(i, 1);
                                return result.ToString();
                            }
                        }

                        arr[i] = originalChar;
                    }
                }
            }

            return "Nothing found";
        }
    }
}
