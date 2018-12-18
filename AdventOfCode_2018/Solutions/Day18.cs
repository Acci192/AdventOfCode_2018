using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day18
    {
        private static int Max = int.MaxValue;
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var grid = new char[rows[0].Length, rows.Count];
            Max = rows.Count;
            for(var y = 0; y < rows.Count; y++)
            {
                for(var x = 0; x < rows[0].Length; x++)
                {
                    grid[x, y] = rows[y][x];
                }
            }

            var numLum = 0;
            var numTree = 0;
            for (int i = 0; i < 10; i++)
            {
                numLum = 0;
                numTree = 0;
                var temp = grid.Clone() as char[,];

                for (var y = 0; y < rows.Count; y++)
                {
                    for (var x = 0; x < rows[0].Length; x++)
                    {
                        Update(x, y, grid, temp);
                        if (temp[x, y] == '|') numTree++;
                        if (temp[x, y] == '#') numLum++;
                    }
                }

                grid = temp.Clone() as char[,];
            }

            return $"{numTree*numLum}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var grid = new char[rows[0].Length, rows.Count];
            Max = rows.Count;
            for (var y = 0; y < rows.Count; y++)
            {
                for (var x = 0; x < rows[0].Length; x++)
                {
                    grid[x, y] = rows[y][x];
                }
            }

            var curRes = 0;
            var numLum = 0;
            var numTree = 0;

            var test = new Dictionary<int, int>();
            var val = new List<int>();
            for (int i = 1; i < 10000; i++)
            {

                numLum = 0;
                numTree = 0;
                var temp = grid.Clone() as char[,];

                for (var y = 0; y < rows.Count; y++)
                {
                    for (var x = 0; x < rows[0].Length; x++)
                    {
                        Update(x, y, grid, temp);
                        if (temp[x, y] == '|') numTree++;
                        if (temp[x, y] == '#') numLum++;
                    }
                }
                curRes = numTree * numLum;

                if (test.ContainsKey(curRes)) test[curRes]++;
                else test[curRes] = 1;

                if (val.Contains(curRes)) return $"{val[(1000000000 - i) % val.Count]}";
                if (test[curRes] > 10) val.Add(curRes);

                grid = temp.Clone() as char[,];
            }

            return $"Not found";
        }

        private static void Update(int x, int y, char[,] g, char[,] c)
        {
            var type = g[x, y];
            var numTree = 0;
            var numOpen = 0;
            var numLumber = 0;
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var tx = x + i;
                    var ty = y + j;
                    if (tx < 0 || tx >= Max || ty < 0 || ty >= Max) continue;
                    if (g[tx, ty] == '.') numOpen++;
                    else if (g[tx, ty] == '|') numTree++;
                    else numLumber++;
                }
            }

            if (type == '.' && numTree >= 3) c[x, y] = '|';
            else if (type == '|' && numLumber >= 3) c[x, y] = '#';
            else if (type == '#')
            {
                if (numLumber >= 1 && numTree >= 1) c[x, y] = '#';
                else c[x, y] = '.';
            }
        }
    }
}
