using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day23
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var bots = new Dictionary<Tuple<int, int, int>, int>();
            foreach (var row in rows)
            {
                var t = row.Split('<')[1].Split(',');
                var x = int.Parse(t[0]);
                var y = int.Parse(t[1]);
                var z = int.Parse(t[2].TrimEnd('>'));
                var r = int.Parse(t[3].Substring(3));
                bots.Add(new Tuple<int, int, int>(x, y, z), r);
            }

            var best = bots.Where(b => b.Value == bots.Max(t => t.Value)).FirstOrDefault();

            var res = 0;
            foreach(var b in bots)
            {
                var dist = Math.Abs(best.Key.Item1 - b.Key.Item1) + Math.Abs(best.Key.Item2 - b.Key.Item2) + Math.Abs(best.Key.Item3 - b.Key.Item3);
                if (dist <= best.Value) res++;
            }
            return $"{res}";
        }

        // Really bad solution. I made an assumption that the best point will be around any of the edges. 
        // From there I used the two best positions and randomized offssets until I couldn't find a better position.
        // I have to come back to this and solve it in a better way.
        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var bots = new Dictionary<Tuple<int, int, int>, int>();
            foreach (var row in rows)
            {
                var t = row.Split('<')[1].Split(',');
                var x = int.Parse(t[0]);
                var y = int.Parse(t[1]);
                var z = int.Parse(t[2].TrimEnd('>'));
                var r = int.Parse(t[3].Substring(3));
                bots.Add(new Tuple<int, int, int>(x, y, z), r);
            }
            var poi = new List<Pos>();
            var points = new List<Pos>();
            var set = new HashSet<Tuple<int, int, int>>();
            foreach(var b in bots)
            { 
                poi.Add(new Pos(b.Key.Item1 + b.Value, b.Key.Item2, b.Key.Item3, bots));
                poi.Add(new Pos(b.Key.Item1 - b.Value, b.Key.Item2, b.Key.Item3, bots));
                poi.Add(new Pos(b.Key.Item1, b.Key.Item2 + b.Value, b.Key.Item3, bots));
                poi.Add(new Pos(b.Key.Item1, b.Key.Item2 - b.Value, b.Key.Item3, bots));
                poi.Add(new Pos(b.Key.Item1, b.Key.Item2, b.Key.Item3 + b.Value, bots));
                poi.Add(new Pos(b.Key.Item1, b.Key.Item2, b.Key.Item3 - b.Value, bots));

                points.Add(new Pos(b.Key.Item1, b.Key.Item2, b.Key.Item3, bots));

            }
            poi = poi.OrderBy(x => x.NumCloseBots).ToList();
            var bestOption = poi.Last();
            var nextBest = poi[poi.Count-2];
            var test = new List<Pos>();
            var max = bestOption.NumCloseBots;

            Pos res = bestOption;
            long bestDist = Math.Abs(bestOption.X) + Math.Abs(bestOption.Y) + Math.Abs(bestOption.Z);
            for (var i = 0; i < 200000; i++)
            {
                var random = new Random();
                var x = random.Next(10000) - 5000;
                var y = random.Next(10000) - 5000;
                var z = random.Next(10000) - 5000;

                var temp = new Pos(bestOption.X + x, bestOption.Y + y, bestOption.Z + z, bots);
                if (temp.NumCloseBots > max || (temp.NumCloseBots == max && (Math.Abs(temp.X) + Math.Abs(temp.Y) + Math.Abs(temp.Z) < bestDist)))
                {
                    nextBest = bestOption;
                    max = temp.NumCloseBots;
                    bestOption = temp;
                    bestDist = (Math.Abs(temp.X) + Math.Abs(temp.Y) + Math.Abs(temp.Z));
                    i = 0;
                }

                temp = new Pos(nextBest.X + x, nextBest.Y + y, nextBest.Z + z, bots);
                if (temp.NumCloseBots > max || (temp.NumCloseBots == max && (Math.Abs(temp.X) + Math.Abs(temp.Y) + Math.Abs(temp.Z) < bestDist)))
                {
                    nextBest = bestOption;
                    max = temp.NumCloseBots;
                    bestOption = temp;
                    bestDist = (Math.Abs(temp.X) + Math.Abs(temp.Y) + Math.Abs(temp.Z));
                    i = 0;
                }
            }

            return $"{bestDist}";
        }

        private static bool InRange(int x1, int y1, int z1, int x2, int y2, int z2, int r)
        {
            var dist = Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2);
            return dist <= r;
        }

        private class Pos
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int NumCloseBots { get; set; }

            public Pos(int x, int y, int z, Dictionary<Tuple<int,int,int>, int> bots)
            {
                X = x;
                Y = y;
                Z = z;
                NumCloseBots = 0;
                foreach(var b in bots)
                {
                    var dist = Math.Abs(X - b.Key.Item1) + Math.Abs(Y - b.Key.Item2) + Math.Abs(Z - b.Key.Item3);
                    if (dist <= b.Value) NumCloseBots++;
                }
            }
        }
    }
}
