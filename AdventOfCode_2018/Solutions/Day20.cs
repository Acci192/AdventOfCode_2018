using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day20
    {
        public static string A(string input)
        {
            var grid = new char[100, 100];

            var curX = 50;
            var curY = 50;

            var stack = new Stack<Tuple<int, int>>();

            for(var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == 'N' || c == 'E' || c == 'S' || c == 'W')
                {
                    FillGrid(curX, curY, c, grid);
                    switch (c)
                    {
                        case 'N':
                            curY-=2;
                            break;
                        case 'E':
                            curX+=2;
                            break;
                        case 'S':
                            curY+=2;
                            break;
                        case 'W':
                            curX-=2;
                            break;
                    }
                }
                else if(c == '(')
                {
                    var test = FindSubPath(input, i);
                }
                    

            }
            return $"";
        }

        public static string FindSubPath(string path, int start)
        {
            var count = 0;
            for(var i = start; i < path.Length; i++)
            {
                if (path[i] == '(') count++;
                else if (path[i] == ')') count--;

                if (count == 0) return path.Substring(start, i + 1 - start);
            }
            return "";
        }
        public static void FillGrid(int x, int y, char c, char[,] g)
        {
            g[x - 1, y - 1] = '#';
            g[x + 1, y - 1] = '#';
            g[x - 1, y + 1] = '#';
            g[x - 1, y - 1] = '#';

            switch (c)
            {
                case 'N':
                    g[x, y - 1] = '-';
                    break;
                case 'E':
                    g[x + 1, y] = '|';
                    break;
                case 'S':
                    g[x, y + 1] = '-';
                    break;
                case 'W':
                    g[x + 1, y] = '|';
                    break;
            }
        }
    }
}
