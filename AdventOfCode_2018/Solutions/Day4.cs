using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day4
    {
        public static string A(string input)
        {
            var currentId = -1;
            var beginMin = 0;

            var instructions = new SortedDictionary<DateTime, string>();
            var workers = new Dictionary<int, int>();
            var rows = input.Replace("\r", "").Split('\n');
            foreach (var row in rows)
            {
                var split = row.Split(']');
                var time = DateTime.Parse(split[0].Substring(1));
                int id;
                var info = "_";
                var idString = Regex.Match(split[1], @"\d{1,4}").Value;
                if (int.TryParse(idString, out id))
                {
                    info = idString;
                }
                else if (split[1].Contains("asleep"))
                {
                    info = "a";
                }
                else
                {
                    info = "w";
                }
                instructions.Add(time, info);
            }

            foreach (var instruction in instructions)
            {
                
                if (int.TryParse(instruction.Value, out var temp))
                {
                    currentId = temp;
                    beginMin = 0;
                    if (instruction.Key.Hour == 12)
                    {
                        beginMin = instruction.Key.Minute;
                    }
                    
                } 
                else if (instruction.Value.Equals("a"))
                {
                    beginMin = instruction.Key.Minute;
                }
                else
                {
                    if (!workers.ContainsKey(currentId))
                    {
                        workers[currentId] = instruction.Key.Minute - beginMin;
                    }
                    else
                    {
                        workers[currentId] += instruction.Key.Minute - beginMin;
                    }
                }
            }

            var lazy = workers.FirstOrDefault(x => x.Value == workers.Max(y => y.Value));

            var LazyInstructions = new SortedDictionary<DateTime, string>();
            var minArray = new int[60];
            foreach (var instruction in instructions)
            {
                if (int.TryParse(instruction.Value, out var temp))
                {
                    currentId = temp;
                    beginMin = 0;
                    if (temp == lazy.Key)
                    {
                        
                        LazyInstructions.Add(instruction.Key, instruction.Value);
                    }
                    
                }
                else if (instruction.Value.Equals("a"))
                {
                    if (currentId == lazy.Key)
                    {
                        beginMin = instruction.Key.Minute;
                        LazyInstructions.Add(instruction.Key, instruction.Value);
                    }
                }
                else
                {
                    if (currentId == lazy.Key)
                    {
                        var tempz = instruction.Key.Minute - beginMin;
                        for (int i = beginMin; i < instruction.Key.Minute; i++)
                        {
                            minArray[i]++;
                        }
                        LazyInstructions.Add(instruction.Key, instruction.Value);
                    }
                        
                }
                
            }

            var max = int.MinValue;
            var mixIndex = -1;
            for (int i = 0; i < minArray.Length; i++)
            {
                if (minArray[i] > max)
                {
                    max = minArray[i];
                    mixIndex = i;
                }
            }
            return $"{mixIndex * lazy.Key}";
        }

        public static string B(string input)
        {
            var currentId = -1;
            var beginMin = 0;

            var instructions = new SortedDictionary<DateTime, string>();
            var workers = new Dictionary<int, int[]>();
            var rows = input.Replace("\r", "").Split('\n');
            foreach (var row in rows)
            {
                var split = row.Split(']');
                var time = DateTime.Parse(split[0].Substring(1));
                int id;
                var info = "_";
                var idString = Regex.Match(split[1], @"\d{1,4}").Value;
                if (int.TryParse(idString, out id))
                {
                    info = idString;
                }
                else if (split[1].Contains("asleep"))
                {
                    info = "a";
                }
                else
                {
                    info = "w";
                }
                instructions.Add(time, info);
            }

            foreach (var instruction in instructions)
            {

                if (int.TryParse(instruction.Value, out var temp))
                {
                    currentId = temp;
                    beginMin = 0;
                    if (instruction.Key.Hour == 12)
                    {
                        beginMin = instruction.Key.Minute;
                    }

                }
                else if (instruction.Value.Equals("a"))
                {
                    beginMin = instruction.Key.Minute;
                }
                else
                {
                    if (!workers.ContainsKey(currentId))
                    {
                        workers[currentId] = new int[60];
                    }

                    for (int i = beginMin; i < instruction.Key.Minute; i++)
                    {
                        workers[currentId][i]++;
                    }
                }
            }

            var guard = -1;
            var minute = -1;
            var index = -1;
            foreach (var worker in workers)
            {
                for (int i = 0; i < worker.Value.Length; i++)
                {
                    if (worker.Value[i] > minute)
                    {
                        guard = worker.Key;
                        minute = worker.Value[i];
                        index = i;
                    }
                }
            }
            
            return $"{guard * index}";
        }
    }
}
