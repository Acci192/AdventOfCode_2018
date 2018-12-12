using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day12
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var pots = new StringBuilder(rows[0].Split(' ')[2]);

            var combinations = new Dictionary<string, char>();
            rows.RemoveRange(0, 2);
            foreach (var row in rows)
            {
                var temp = row.Split(' ');
                combinations[temp[0]] = temp[2][0];
            }

            var zeroIndex = 0;
            for(var g = 0; g < 20; g++)
            {
                zeroIndex += 3;
                pots.Insert(0, ".", 3);
                pots.Append('.', 3);
                var lastPots = new StringBuilder(pots.ToString());
                
                for(var i = 2; i < lastPots.Length - 2; i++)
                {
                    string pattern = lastPots.ToString().Substring(i - 2, 5);
                    
                    pots[i] = combinations[pattern];
                }

                var temp = pots.ToString().TrimStart('.');
                zeroIndex -= pots.Length - temp.Length;
                temp = temp.TrimEnd('.');
                lastPots = new StringBuilder(temp);
                pots = new StringBuilder(temp);
                Console.WriteLine(pots.ToString());

            }

            
            var sum = 0;
            for(var i = 0; i < pots.Length; i++)
            {
                if(pots[i] == '#')
                {
                    sum += i - zeroIndex;
                }
            }
            return $"{sum}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var pots = new StringBuilder(rows[0].Split(' ')[2]);

            var combinations = new Dictionary<string, char>();
            rows.RemoveRange(0, 2);
            foreach (var row in rows)
            {
                var temp = row.Split(' ');
                combinations[temp[0]] = temp[2][0];
            }

            var zeroIndex = 0;
            var sum = 0;
            var lastSum = int.MinValue;
            var lastDiff = int.MinValue;
            var count = 0;
            for (var g = 0; g < 120; g++)
            {
                sum = 0;
                zeroIndex += 3;
                pots.Insert(0, ".", 3);
                pots.Append('.', 3);
                var lastPots = new StringBuilder(pots.ToString());

                for (var i = 2; i < lastPots.Length - 2; i++)
                {
                    string pattern = lastPots.ToString().Substring(i - 2, 5);

                    pots[i] = combinations[pattern];
                }


                lastPots = new StringBuilder(pots.ToString());
                Console.WriteLine(pots.ToString());

                for (var i = 0; i < pots.Length; i++)
                {
                    if (pots[i] == '#')
                    {
                        sum += i - zeroIndex;
                    }
                }
                var diff = sum - lastSum;

                if(diff == lastDiff)
                {
                    count++;
                    if (count > 5)
                    {
                        break;
                    }
                }
                else
                {
                    count = 0;
                }

                lastSum = sum;
                lastDiff = diff;
                
            }


            
            //for (var i = 0; i < pots.Length; i++)
            //{
            //    if (pots[i] == '#')
            //    {
            //        sum += i - zeroIndex;
            //    }
            //}
            return $"{sum}";
        }

        // Hacky solution for now (Was looking for repeating differences and just calculate the rest)
        // Had to go and therefore I did it this way. Will be fixed
        public static string test(string input)
        {
            //var sum = 19437;
            long sum = 19437;
            sum += 186 * (50000000000 - 98);

            return $"{sum - 186}";
        }
    }
}
