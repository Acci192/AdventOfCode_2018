using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode_2018.Solutions
{
    public class Day4
    {
        public static string A(string input)
        {
            var currentId = -1;
            var beginMin = 0;

            var workers = new Dictionary<int, int[]>();
            var rows = input.Replace("\r", "").Split('\n');

            Array.Sort(rows);

            foreach (var row in rows)
            {
                var regex = Regex.Match(row, @"\[(.*)\](.*#(\d*)|.*(wakes|asleep))");
                var time = DateTime.Parse(regex.Groups[1].Value);
                if (int.TryParse(regex.Groups[3].Value, out var temp))
                {
                    currentId = temp;
                    if (!workers.ContainsKey(currentId))
                    {
                        workers[currentId] = new int[60];
                    }
                }
                else if (regex.Groups[4].Value.Equals("asleep"))
                {
                    beginMin = time.Minute;
                }
                else
                {
                    for (var i = beginMin; i < time.Minute; i++)
                        workers[currentId][i]++;
                }
            }

            var lazyWorker = workers.FirstOrDefault(x => x.Value.Sum() == workers.Max(y => y.Value.Sum()));
            var maxIndex = 0;
            var max = lazyWorker.Value[maxIndex];
            for (var i = 1; i < lazyWorker.Value.Length; i++)
            {
                if (lazyWorker.Value[i] > max)
                {
                    max = lazyWorker.Value[i];
                    maxIndex = i;
                }
            }

            return $"{maxIndex * lazyWorker.Key}";
        }

        public static string B(string input)
        {
            var currentId = -1;
            var beginMin = 0;

            var workers = new Dictionary<int, int[]>();
            var rows = input.Replace("\r", "").Split('\n');

            Array.Sort(rows);

            foreach (var row in rows)
            {
                var regex = Regex.Match(row, @"\[(.*)\](.*#(\d*)|.*(wakes|asleep))");
                var time = DateTime.Parse(regex.Groups[1].Value);
                if (int.TryParse(regex.Groups[3].Value, out var temp))
                {
                    currentId = temp;
                    if (!workers.ContainsKey(currentId))
                    {
                        workers[currentId] = new int[60];
                    }
                }
                else if (regex.Groups[4].Value.Equals("asleep"))
                {
                    beginMin = time.Minute;
                }
                else
                {
                    for (var i = beginMin; i < time.Minute; i++)
                        workers[currentId][i]++;
                }
            }

            var guard = -1;
            var minute = -1;
            var index = -1;
            foreach (var worker in workers)
            {
                for (int i = 0; i < worker.Value.Length; i++)
                {
                    if (worker.Value[i] > minute)
                    {
                        guard = worker.Key;
                        minute = worker.Value[i];
                        index = i;
                    }
                }
            }
            
            return $"{guard * index}";
        }
    }
}
