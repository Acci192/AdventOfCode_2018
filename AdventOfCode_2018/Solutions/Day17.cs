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
        private static int maxX = 653;
        private static int maxY = 1895;

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

            maxY = clay.Max(x => x.Item2);
            maxX = clay.Max(x => x.Item1);
            var q = new Queue<Tuple<int, int>>();
            var curX = 500;
            var curY = 0;
            q.Enqueue(new Tuple<int, int>(curX, curY));

            var visited = new HashSet<Tuple<int, int>>();
            while(q.Count > 0)
            {
                var t = q.Dequeue();
                visited.Add(t);
                var x = t.Item1;
                var y = t.Item2;
                y = FindBottom(x, y, clay, water);
                if (y == maxY) continue;
                var left = FindLeft(x, y, clay, water);
                var right = FindRight(x, y, clay, water);

                while (left.Item1 && right.Item1)
                {
                    y--;
                    left = FindLeft(x, y, clay, water);
                    right = FindRight(x, y, clay, water);
                }
                t = new Tuple<int, int>(left.Item2, y+1);
                
                if (!left.Item1 && !visited.Contains(t))
                {
                    visited.Add(t);
                    q.Enqueue(t);
                }
                    
                t = new Tuple<int, int>(right.Item2, y+1);
                
                if (!right.Item1 && !visited.Contains(t))
                {
                    visited.Add(t);
                    q.Enqueue(new Tuple<int, int>(right.Item2, y));
                }

                for (int ty = clay.Min(a => a.Item2); ty <= clay.Max(a => a.Item2); ty++)
                {
                    for (int tx = clay.Min(a => a.Item1); tx <= clay.Max(a => a.Item1); tx++)
                    {
                        if (clay.Contains(new Tuple<int, int>(tx, ty))) Console.Write('#');
                        else if (water.Contains(new Tuple<int, int>(tx, ty))) Console.Write('|');
                        else Console.Write(' ');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            return $"Result";
        }


        private static int FindBottom(int x, int y, HashSet<Tuple<int, int>> clay, HashSet<Tuple<int, int>> water)
        {
            while (y < maxY && !clay.Contains(new Tuple<int, int>(x, y + 1)))
            {
                water.Add(new Tuple<int, int>(x, y));
                y++;
            }
            return y;
        }

        private static Tuple<bool, int> FindLeft(int x, int y, HashSet<Tuple<int, int>> clay, HashSet<Tuple<int, int>> water)
        {
            while (x > 0)
            {
                water.Add(new Tuple<int, int>(x, y));
                var t = new Tuple<int, int>(x, y + 1);
                if (!clay.Contains(t) && !water.Contains(t)) 
                    return new Tuple<bool, int>(false, x);
                else if (clay.Contains(new Tuple<int, int>(x-1, y)))
                    return new Tuple<bool, int>(true, x);
                x--;
            }
            return new Tuple<bool, int>(false, x);
        }

        private static Tuple<bool, int> FindRight(int x, int y, HashSet<Tuple<int, int>> clay, HashSet<Tuple<int, int>> water)
        {
            while (x < maxX)
            {
                water.Add(new Tuple<int, int>(x, y));
                var t = new Tuple<int, int>(x, y + 1);
                if (!clay.Contains(t) && !water.Contains(t))
                    return new Tuple<bool, int>(false, x);
                else if (clay.Contains(new Tuple<int, int>(x + 1, y)))
                    return new Tuple<bool, int>(true, x);
                x++;
            }
            return new Tuple<bool, int>(false, x);
        }
    }
}
//if(temp[0][0] == 'y')
//{
//    var y =int. Parse(temp[0].Split('=')[1]);
//    if (y > maxY) maxY = y;

//    var x = int.Parse(temp[1].Split('=')[1].Split('.')[2]);
//    if (x > maxX) maxX = x;
//}
//else if (temp[0][0] == 'x')
//{
//    var x = int.Parse(temp[0].Split('=')[1]);
//    if (x > maxX) maxX = x;

//    var y = int.Parse(temp[1].Split('=')[1].Split('.')[2]);
//    if (y > maxY) maxY = y;
//}