using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day10
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var points = new List<AoCPoint>();
            foreach (var row in rows)
            {
                var posString = row.Split('<')[1].Split('>')[0].Split(',');
                var velString = row.Split('<')[2].Split('>')[0].Split(',');
                points.Add(new AoCPoint(int.Parse(posString[0]), int.Parse(posString[1]), int.Parse(velString[0]), int.Parse(velString[1])));
            }

            var lastArea = int.MaxValue;
            var curArea = int.MinValue;
            while (true)
            {
                points.ForEach(x => x.Update());
                
                var width = points.Max(point => point.x) - points.Min(point => point.x);
                var height = points.Max(point => point.y) - points.Min(point => point.y);

                curArea = width * height;

                if (lastArea < curArea && points.Count(point => point.isClose) == points.Count())
                {
                    points.ForEach(x => x.Rewind());

                    var startX = points.Min(point => point.x);
                    var startY = points.Min(point => point.y);
                    var endX = points.Max(point => point.x);
                    var endY = points.Max(point => point.y);

                    var result = new StringBuilder();
                    for(int y = startY; y <= endY; y++)
                    {
                        for (int x = startX; x <= endX; x++)
                        {
                            if(points.Count(node => node.x == x && node.y == y) > 0)
                            {
                                result.Append('#');
                            }
                            else
                            {
                                result.Append(' ');
                            }
                        }
                        result.Append('\n');
                    }
                    return result.ToString();
                }
                lastArea = curArea;
            }
        }

        


        public static string B  (string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var points = new List<AoCPoint>();
            foreach (var row in rows)
            {
                var posString = row.Split('<')[1].Split('>')[0].Split(',');
                var velString = row.Split('<')[2].Split('>')[0].Split(',');
                points.Add(new AoCPoint(int.Parse(posString[0]), int.Parse(posString[1]), int.Parse(velString[0]), int.Parse(velString[1])));
            }
            var counter = 0;
            var lastArea = int.MaxValue;
            var curArea = int.MinValue;
            while (true)
            {
                for (int i = 0; i < rows.Count(); i++)
                {
                    points[i].Update();
                }

                var width = points.Max(point => point.x) - points.Min(point => point.x);
                var height = points.Max(point => point.y) - points.Min(point => point.y);

                curArea = width * height;

                if (lastArea < curArea && points.Count(point => point.isClose) == 300)
                {
                    return counter.ToString();
                }
                lastArea = curArea;
                counter++;
            }
        }

        private class AoCPoint
        {
            public static int close = 300;
            public int x { get; set; }
            public int y { get; set; }
            public int velX { get; set; }
            public int velY { get; set; }
            public bool isClose { get; set; }

            public AoCPoint(int x, int y, int velX, int velY)
            {
                this.x = x;
                this.y = y;
                this.velX = velX;
                this.velY = velY;
                isClose = x < close && y < close && y > -close && x > -close;
            }

            public void Update()
            {
                x += velX;
                y += velY;
                isClose = x < close && y < close && y > -close && x > -close;
            }

            public void Rewind()
            {
                x -= velX;
                y -= velY;
            }
        }
    }
}
