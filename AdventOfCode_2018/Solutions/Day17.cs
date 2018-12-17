using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day17
    {
        private static int maxX = int.MinValue;
        private static int maxY = int.MinValue;
        private static int minX = int.MaxValue;
        private static int minY = int.MaxValue;

        private static HashSet<Tuple<int, int>> Clay = new HashSet<Tuple<int, int>>();
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var clay = new HashSet<Tuple<int, int>>();
            var water = new HashSet<Tuple<int, int>>();
            foreach (var row in rows)
            {
                var temp = row.Split(',');

                if (temp[0][0] == 'y')
                {
                    var y = int.Parse(temp[0].Split('=')[1]);
                    
                    var firstX = int.Parse(temp[1].Split('=')[1].Split('.')[0]);
                    var lastX = int.Parse(temp[1].Split('=')[1].Split('.')[2]);

                    for(int x = firstX; x <= lastX; x++)
                    {
                        clay.Add(new Tuple<int, int>(x, y));
                    }
                }
                else if (temp[0][0] == 'x')
                {
                    var x = int.Parse(temp[0].Split('=')[1]);
                    if (x > maxX) maxX = x;

                    var firstY = int.Parse(temp[1].Split('=')[1].Split('.')[0]);
                    var lastY = int.Parse(temp[1].Split('=')[1].Split('.')[2]);
                    for (int y = firstY; y <= lastY; y++)
                    {
                        clay.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            maxY = clay.Select(x=> x.Item2).Max();
            maxX = clay.Select(x => x.Item1).Max() +1 ;
            minY = clay.Select(x => x.Item2).Min();
            minX = clay.Select(x => x.Item1).Min();
            var q = new Queue<Tuple<int, int>>();
            var curX = 500;
            var curY = 0;
            q.Enqueue(new Tuple<int, int>(curX, curY));

            var visited = new HashSet<Tuple<int, int>>();
            var stillWater = new HashSet<Tuple<int, int>>();
            while (q.Count > 0)
            {
                var t = q.Dequeue();
                visited.Add(t);
                var x = t.Item1;
                var y = t.Item2;
                y = FindBottom(x, y, clay, water);
                
                var left = FindLeft(x, y, clay, water, stillWater);
                var right = FindRight(x, y, clay, water, stillWater);

                while (left.Item1 && right.Item1)
                {
                    for(int i = left.Item2; i <= right.Item2; i++)
                    {
                        stillWater.Add(new Tuple<int, int>(i, y));
                    }
                    y--;
                    left = FindLeft(x, y, clay, water, stillWater);
                    right = FindRight(x, y, clay, water, stillWater);
                }
                if (y == maxY) continue;
                t = new Tuple<int, int>(left.Item2, y);
                
                if (!left.Item1 && !visited.Contains(t))
                {
                    visited.Add(t);
                    q.Enqueue(t);
                }
                    
                t = new Tuple<int, int>(right.Item2, y);
                
                if (!right.Item1 && !visited.Contains(t))
                {
                    visited.Add(t);
                    q.Enqueue(t);
                }

                
            }
            for (int ty = 0; ty <= maxY; ty++)
            {
                for (int tx = 450; tx <= maxX; tx++)
                {
                    if (clay.Contains(new Tuple<int, int>(tx, ty))) Console.Write('#');
                    else if (stillWater.Contains(new Tuple<int, int>(tx, ty))) Console.Write('~');
                    else if (water.Contains(new Tuple<int, int>(tx, ty))) Console.Write('|');
                    else Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            return $"Total Water: {water.Count}\nStill Water: {stillWater.Count}";
        }


        private static int FindBottom(int x, int y, HashSet<Tuple<int, int>> clay, HashSet<Tuple<int, int>> water)
        {
            while (y < maxY && !clay.Contains(new Tuple<int, int>(x, y + 1)))
            {
                if(y >= minY && y < maxY)
                    water.Add(new Tuple<int, int>(x, y));
                y++;
            }
            return y;
        }

        private static Tuple<bool, int> FindLeft(int x, int y, HashSet<Tuple<int, int>> clay, HashSet<Tuple<int, int>> water, HashSet<Tuple<int, int>> still)
        {
            while (x > 0)
            {
                water.Add(new Tuple<int, int>(x, y));
                var t = new Tuple<int, int>(x, y + 1);
                if (!clay.Contains(t) && !still.Contains(t)) 
                    return new Tuple<bool, int>(false, x);
                else if (clay.Contains(new Tuple<int, int>(x-1, y)))
                    return new Tuple<bool, int>(true, x);
                x--;
            }
            return new Tuple<bool, int>(false, x);
        }

        private static Tuple<bool, int> FindRight(int x, int y, HashSet<Tuple<int, int>> clay, HashSet<Tuple<int, int>> water, HashSet<Tuple<int, int>> still)
        {
            while (x <= maxX)
            {
                water.Add(new Tuple<int, int>(x, y));
                var t = new Tuple<int, int>(x, y + 1);
                if (!clay.Contains(t) && !still.Contains(t))
                    return new Tuple<bool, int>(false, x);
                else if (clay.Contains(new Tuple<int, int>(x + 1, y)))
                    return new Tuple<bool, int>(true, x);
                x++;
            }
            return new Tuple<bool, int>(false, x);
        }
    }
}
