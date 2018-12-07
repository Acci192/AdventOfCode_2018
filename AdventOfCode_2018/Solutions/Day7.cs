using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day7
    {
        private class Node
        {
            public char Value { get; set; }
            public List<char> Connections { get; set; }

            public Node(char value)
            {
                Value = value;
                Connections = new List<char>();
            }
        }
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var dic = new Dictionary<char, List<char>>();
            List<Node> graph = new List<Node>();
            for(char c = 'A'; c <= 'Z'; c++)
            {
                dic[c] = new List<char>();
            }
            foreach (var row in rows)
            {
                var tempRow = row.Split(' ');
                var first = tempRow[1].FirstOrDefault();
                var second = tempRow[7].FirstOrDefault();

                dic[first].Add(second);
            }

            char startingChar = ',';
            var starting = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if(dic.Where(x => x.Value.Contains(c)).Count() == 0)
                {
                    startingChar = c;
                    starting.Add(c);

                }
            }
            Console.WriteLine($"***** = {string.Join(",", starting)}");

            foreach (var temp in dic)
            {
                Console.WriteLine($"{temp.Key} = {string.Join(",", temp.Value)}");
            }

            var queue = new SortedSet<char>();
            var visited = new HashSet<char>();
            foreach(var c in starting)
            {
                queue.Add(c);
            }
            
            var result = new StringBuilder();
            while(queue.Count() != 0)
            {
                var current = queue.Min();
                visited.Add(current);
                result.Append(current);
                queue.Remove(current);
                foreach(var c in dic[current])
                {
                    var test = dic.Where(x => !visited.Contains(x.Key)).Select(x => x.Value).ToList();
                    if (test.Count(x => x.Contains(c)) == 0)
                    {
                        if (!visited.Contains(c))
                            queue.Add(c);
                    }
                    
                }
            }
            return $"{result.ToString()}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var dic = new Dictionary<char, List<char>>();
            List<Node> graph = new List<Node>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                dic[c] = new List<char>();
            }
            foreach (var row in rows)
            {
                var tempRow = row.Split(' ');
                var first = tempRow[1].FirstOrDefault();
                var second = tempRow[7].FirstOrDefault();

                dic[first].Add(second);
            }

            var queue = new SortedSet<char>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (dic.Where(x => x.Value.Contains(c)).Count() == 0)
                {
                    queue.Add(c);

                }
            }

            
            var workers = new Dictionary<int, int>();
            for(var i = 0; i < 2; i++)
            {
                workers[i] = 0;
            }
            var visited = new HashSet<char>();

            var time = new Dictionary<char, int>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                time[c] = 0;
            }


            var result = new StringBuilder();
            while (queue.Count() != 0)
            {
                var current = queue.Min();
                visited.Add(current);

                //var firstWorker = workers.Min(x => x.Value);
                //var temp = workers.FirstOrDefault(x => x.Value == firstWorker);
                //workers[temp.Key] += (current - 64);

                time[current] += 60 +(current - 64);
                result.Append(current);
                queue.Remove(current);
                foreach (var c in dic[current])
                {
                    var test = dic.Where(x => !visited.Contains(x.Key)).Select(x => x.Value).ToList();
                    //var max = dic.Where(x => !visited.Contains(x.Key)).Max(x => x.Value).ToList();
                    if (test.Count(x => x.Contains(c)) == 0)
                    {
                        if (!visited.Contains(c))
                        {
                            queue.Add(c);
                            time[c] = time[current];
                        }
                            
                    }

                }
            }
            return $"{time.Max(x => x.Value)}";
        }
    }
}
