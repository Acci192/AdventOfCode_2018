using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day11
    {
        public static string A(string input)
        {
            var serialNumber = int.Parse(input);
            

            var size = 300;
            var grid = new int[size, size];

            for(var y = 0; y < size; y++)
            {
                for(var x = 0; x < size; x++)
                {
                    var rackId = x + 10;
                    var powerLevel = rackId * y;
                    powerLevel += serialNumber;
                    powerLevel *= rackId;

                    powerLevel = Math.Abs(powerLevel / 100 % 10);
                    powerLevel -= 5;
                    grid[x, y] = powerLevel;
                }
            }

            var max = int.MinValue;
            var curX = int.MinValue;
            var curY = int.MinValue;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var temp = getAreaValue(grid, x, y);
                    if(temp > max)
                    {
                        max = temp;
                        curX = x;
                        curY = y;
                    }
                }
            }

            return $"{curX},{curY}";
        }

        private static int getAreaValue(int[,] grid, int x, int y)
        {
            if (x < 0 || y < 0 || x > 297 || y > 297)
                return int.MinValue;
            var sum = 0;
            for (int dy = 0; dy < 3; dy++)
            {
                for (int dx = 0; dx < 3; dx++)
                {
                    sum += grid[x + dx, y + dy];
                }
            }
            return sum;
        }

        // This is an optimized solution of the problem based the solution from reddit user: tribulu 
        public static string B_Optimized(string input)
        {
            var serialNumber = int.Parse(input);
            var size = 300;

            var grid = new int[size + 1, size + 1];
            for(var y = 1; y <= size; y++)
            {
                for(var x = 1; x <= size; x++)
                {
                    var rackId = x + 10;
                    var powerLevel = rackId * y;
                    powerLevel += serialNumber;
                    powerLevel *= rackId;

                    powerLevel = Math.Abs(powerLevel / 100 % 10);
                    powerLevel -= 5;

                    grid[x, y] = powerLevel + grid[x - 1, y] + grid[x, y - 1] - grid[x - 1, y - 1];
                } 
            }

            var max = int.MinValue;
            var curX = int.MinValue;
            var curY = int.MinValue;
            var curS = int.MinValue;
            for (var s = 1; s <= size; s++)
            {
                for (var y = s; y <= size; y++)
                {
                    for (var x = s; x <= size; x++)
                    {
                        var sum = grid[x, y] - grid[x - s, y] - grid[x, y - s] + grid[x - s, y - s];
                        if(sum > max)
                        {
                            max = sum;
                            curX = x;
                            curY = y;
                            curS = s;
                        }
                    }
                }
            }

            return $"{curX - curS + 1},{curY - curS + 1},{curS}";
        }

        // This is my original solution for the problem which is highly ineffective both in time and space. (I don't recommend running it)
        // It does however solve the problem
        public static string B(string input)
        {
            var serialNumber = int.Parse(input);


            var size = 300;
            var grid = new int[size, size];

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var rackId = x + 10;
                    var powerLevel = rackId * y;
                    powerLevel += serialNumber;
                    powerLevel *= rackId;

                    powerLevel = Math.Abs(powerLevel / 100 % 10);
                    powerLevel -= 5;
                    grid[x, y] = powerLevel;
                }
            }

            for (var y = size - 1; y >= 0; y--)
            {
                for (var x = size - 1; x >= 0; x--)
                {
                    for(var s = size - Math.Max(y, x); s > 0; s--)
                    {
                        if (getAreaValue(grid, x, y, s) == int.MinValue) break;
                    }
                    
                }
            }
            
            var max = seenValues.Max(x => x.Value);
            var res = seenValues.FirstOrDefault(x => x.Value == max);

            return $"{res.Key.Item1},{res.Key.Item2},{res.Key.Item3}";
        }

        private static Dictionary<Tuple<int, int, int>, int> seenValues = new Dictionary<Tuple<int, int, int>, int>();

        private static int getAreaValue(int[,] grid, int x, int y, int size)
        {
            var tuple = new Tuple<int, int, int>(x, y, size);
            if(size == 1)
            {
                seenValues[tuple] = grid[x, y];
                return grid[x, y];
            }
            if (seenValues.ContainsKey(tuple))
            {
                return seenValues[tuple];
            }

            var sum = grid[x, y];
            for (int d = 1; d < size; d++)
            {
                sum += grid[x + d, y];
                sum += grid[x, y + d];
            }
            var nextArea = getAreaValue(grid, x + 1, y + 1, size - 1);

            sum += nextArea;
            seenValues[tuple] = sum;
            return sum;
        }
    }
}
