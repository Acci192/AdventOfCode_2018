using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode_2018.Solutions
{
    public class Day7
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var graph = new Dictionary<char, List<char>>();
            // Create each node
            for (char c = 'A'; c <= 'Z'; c++)
            {
                graph[c] = new List<char>();
            }

            // Add edges between connected nodes
            foreach (var row in rows)
            {
                var tempRow = row.Split(' ');
                var first = tempRow[1].FirstOrDefault();
                var second = tempRow[7].FirstOrDefault();

                graph[first].Add(second);
            }

            var queue = new SortedSet<char>();
            var visited = new HashSet<char>();

            // Find nodes that doesn't depend on any other node
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if(graph.Where(x => x.Value.Contains(c)).Count() == 0)
                {
                    queue.Add(c);
                }
            }

            var result = new StringBuilder();
            while(queue.Count() != 0)
            {
                var current = queue.Min();
                visited.Add(current);
                result.Append(current);
                queue.Remove(current);

                foreach(var c in graph[current])
                {
                    var notVisitedDependency = graph.Where(x => !visited.Contains(x.Key)).Select(x => x.Value).ToList();
                    if (notVisitedDependency.Count(x => x.Contains(c)) == 0 && !visited.Contains(c))
                    {
                        queue.Add(c);
                    }
                    
                }
            }
            return $"{result.ToString()}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var graph = new Dictionary<char, List<char>>();
            // Create each node
            for (char c = 'A'; c <= 'Z'; c++)
            {
                graph[c] = new List<char>();
            }

            // Add edges between connected nodes
            foreach (var row in rows)
            {
                var tempRow = row.Split(' ');
                var first = tempRow[1].FirstOrDefault();
                var second = tempRow[7].FirstOrDefault();

                graph[first].Add(second);
            }

            var queue = new SortedSet<char>();
            var visited = new HashSet<char>();

            // Find nodes that doesn't depend on any other node
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (graph.Where(x => x.Value.Contains(c)).Count() == 0)
                {
                    queue.Add(c);
                }
            }

            var completedNodes = new bool[26];
            var numWorkers = 5;
            var workers = new List<int>();
            var workerChar = new List<char>();
            for(var i = 0; i < numWorkers; i++)
            {
                workers.Add(0);
                workerChar.Add('_');
            }
            var result = 0;
            while(completedNodes.Count(x => x) != 26)
            {
                if(workers.Count(x => x > 0) == numWorkers || (queue.Count == 0 && workers.Count(x => x > 0) > 0))
                {
                    result++;
                    for(var i = 0; i < numWorkers; i++)
                    {
                        if(workers[i] > 0)
                        {
                            workers[i]--;
                            if(workers[i] == 0)
                            {
                                var doneNode = workerChar[i];
                                completedNodes[doneNode - 65] = true;
                                visited.Add(doneNode);

                                foreach(var c in graph[doneNode])
                                {
                                    var notVisitedDependency = graph.Where(x => !visited.Contains(x.Key)).Select(x => x.Value).ToList();
                                    if (notVisitedDependency.Count(x => x.Contains(c)) == 0 && !visited.Contains(c))
                                    {
                                        queue.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var current = queue.Min();

                    queue.Remove(current);
                    var freeWorker = workers.FindIndex(x => x == 0);
                    workers[freeWorker] = 60 + (current - 64);
                    workerChar[freeWorker] = current;
                }
            }

            return $"{result}";
        }
    }
}
