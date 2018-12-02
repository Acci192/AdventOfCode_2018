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
                var unique = row.Distinct();
                if (unique.Where(x => row.Count(r => r == x) == 2).Count() > 0)
                {
                    two++;
                }

                if (unique.Where(x => row.Count(r => r == x) == 3).Count() > 0)
                {
                    three++;
                }
            }

            return $"{two * three}";
        }

        public static string B(string input)
        {
            
            var rows = input.Split('\n').Select(x => x.Replace("\r", "")).ToList();
            var uniqueRows = new HashSet<string>(rows);

            string first = "";
            var index = -1;
            var found = false;
            foreach (var row in rows)
            {
                var arr = row.ToCharArray();
                for (var c = 0; c < arr.Length; c++)
                {
                    for (var i = 97; i < 123; i++)
                    {
                        var originalChar = arr[c];
                        arr[c] = (char)i;
                        var temp = new string(arr);
                        if (temp != row)
                        {
                            if (uniqueRows.Contains(temp))
                            {
                                first = row;
                                index = c;
                                found = true;
                                break;
                            }
                        }

                        arr[c] = originalChar;
                    }

                    if (found) break;
                }

                if (found) break;
            }

            var result = new StringBuilder(first);
            result.Remove(index, 1);
            return result.ToString();
        }
    }
}
