using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2018.Solutions
{
    public class Day6
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var cordinates = new List<Tuple<int, int>>();
            var distances = new Dictionary<int, int>();
            var infinite = new HashSet<int>();
            var curCoordinate = 1;

            foreach (var row in rows)
            {
                var splitRow = row.Split(',');
                cordinates.Add(new Tuple<int, int>(int.Parse(splitRow[0]), int.Parse(splitRow[1])));
                distances[curCoordinate++] = 0;
            }

            var max = Math.Max(cordinates.Max(x => x.Item1), cordinates.Max(x => x.Item2));
            
            for(int y = 0; y < max; y++)
            {
                for(int x = 0; x < max; x++)
                {
                    var shortest = int.MaxValue;
                    var closestChar = -1;
                    curCoordinate = 1;
                    var equal = false;
                    foreach(var cor in cordinates)
                    {
                        var dis = Math.Abs(cor.Item1 - x) + Math.Abs(cor.Item2 - y);
                        if(dis < shortest)
                        {
                            shortest = dis;
                            closestChar = curCoordinate;
                            equal = false;
                        }
                        else if(dis == shortest)
                        {
                            equal = true;
                        }
                        curCoordinate++;
                    }
                    if (equal) continue;

                    if (x > 0 && y > 0 && x < max - 1 && y < max - 1)
                    {
                        distances[closestChar]++;
                    }
                    else
                    {
                        infinite.Add(closestChar);
                    }
                }
            }

            return $"{distances.Where(x => !infinite.Contains(x.Key)).Max(x => x.Value)}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var cordinates = new List<Tuple<int, int>>();

            foreach (var row in rows)
            {
                var splitRow = row.Split(',');
                cordinates.Add(new Tuple<int, int>(int.Parse(splitRow[0]), int.Parse(splitRow[1])));
            }

            var max = Math.Max(cordinates.Max(x => x.Item1), cordinates.Max(x => x.Item2));

            var result = 0;
            for (int y = 0; y < max; y++)
            {
                for (int x = 0; x < max; x++)
                {
                    var sumDis = 0;
                    foreach (var cor in cordinates)
                    {
                        var dis = Math.Abs(cor.Item1 - x) + Math.Abs(cor.Item2 - y);
                        sumDis += dis;
                    }
                    if(sumDis < 10000)
                    {
                        result++;
                    }
                }
            }

            return $"{result}";
        }
    }
}
