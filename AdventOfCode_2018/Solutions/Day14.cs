using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day14
    {
        public static string A(string input)
        {
            var numTimes = int.Parse(input);

            var recipes = new List<int> { 3, 7 };

            var elfA = 0;
            var elfB = 1;

            for(var i = 0; i < numTimes*10; i++)
            {
                var first = recipes[elfA];
                var sec = recipes[elfB];

                var sum = first + sec;

                if (sum < 10)
                    recipes.Add(sum);
                else
                {
                    var tempA = (int)sum / 10;
                    var tempB = sum % 10;
                    recipes.Add(tempA);
                    recipes.Add(tempB);
                }

                elfA = (elfA + first + 1) % recipes.Count();
                elfB = (elfB + sec + 1) % recipes.Count();
            }

            var result = recipes.GetRange(numTimes, 10);
            var t = string.Join("", result);

            return $"{t}";
        }

        private static List<int> staticRecipes = new List<int>();
        public static string B(string input)
        {
            staticRecipes = new List<int>();
            var numTimes = int.Parse(input);

            staticRecipes.Add(3);
            staticRecipes.Add(7);

            var elfA = 0;
            var elfB = 1;

            while(true)
            {
                var first = staticRecipes[elfA];
                var sec = staticRecipes[elfB];

                var sum = first + sec;

                if (sum < 10)
                {
                    staticRecipes.Add(sum);
                    if(matchInput(input))
                        return $"{ staticRecipes.Count - 6}";
                }
                else
                {
                    var tempA = sum / 10;
                    var tempB = sum % 10;
                    staticRecipes.Add(tempA);

                    if (matchInput(input))
                        return $"{ staticRecipes.Count - 6}";

                    staticRecipes.Add(tempB);

                    if (matchInput(input))
                        return $"{ staticRecipes.Count - 6}";
                }
                            
                elfA = (elfA + first + 1) % staticRecipes.Count();
                elfB = (elfB + sec + 1) % staticRecipes.Count();
            }
        }

        private static bool matchInput(string input)
        {
            if (staticRecipes.Count < 10) return false;
            var temp = string.Join("", staticRecipes.GetRange(staticRecipes.Count - 6, 6));
            return temp.Equals(input);
        }

    }
}
