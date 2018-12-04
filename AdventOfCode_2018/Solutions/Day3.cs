using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day3
    {
        public static string A(string input)
        {
            var grid = new int[1200, 1200];
            var splitInput = input.Replace("\r", "").Split('\n');
            var sum = 0;

            foreach (var row in splitInput)
            {   
                var matches = Regex.Matches(row, @"\d{1,4}");
                var list = matches.Cast<Match>().Select(match => match.Value).ToList();
                var posX = int.Parse(list[1]);
                var posY = int.Parse(list[2]);
                var width = int.Parse(list[3]);
                var height = int.Parse(list[4]);

                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        if (++grid[posY + j, posX + i] == 2) sum++;
                    }
                }
            }

            return $"{sum}";
        }

        public static string B(string input)
        {
            var grid = new int[1200, 1200];
            var splitInput = input.Replace("\r", "").Split('\n');

            var unique = new HashSet<int>();
            foreach (var row in splitInput)
            {
                var matches = Regex.Matches(row, @"\d{1,4}");
                var list = matches.Cast<Match>().Select(match => match.Value).ToList();
                var id = int.Parse(list[0]);
                var posX = int.Parse(list[1]);
                var posY = int.Parse(list[2]);
                var width = int.Parse(list[3]);
                var height = int.Parse(list[4]);
                unique.Add(id);
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        if (grid[posY + j, posX + i] == 0)
                        {
                            grid[posY + j, posX + i] = id;
                        }
                        else if(grid[posY + j, posX + i] > 0)
                        {
                            unique.Remove(grid[posY + j, posX + i]);
                            grid[posY + j, posX + i] = -1;
                            unique.Remove(id);
                        }
                        else
                        {
                            unique.Remove(id);
                        }
                    }
                }
            }

            return $"{unique.FirstOrDefault()}";
        }
    }
}
