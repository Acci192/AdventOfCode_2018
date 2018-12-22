using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day22
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var depth = int.Parse(rows[0].Split(' ')[1]);
            var tarX = int.Parse(rows[1].Split(' ')[1].Split(',')[0]);
            var tarY = int.Parse(rows[1].Split(' ')[1].Split(',')[1]);

            var grid = new int[tarX + 1, tarY + 1];
            var res = 0;
            
            for (var i = 1; i <= Math.Max(tarX, tarY); i++)
            {
                if(i <= tarX)
                {
                    grid[i, 0] = (i * 16807 + depth) % 20183;
                    res += grid[i, 0] % 3;
                }
                if(i <= tarY)
                {
                    grid[0, i] = (i * 48271 + depth) % 20183;
                    res += grid[0, i] % 3;
                }
            }

            for(var y = 1; y <= tarY; y++)
            {
                for(var x = 1; x <= tarX; x++)
                {
                    var gIndex = grid[x - 1, y] * grid[x, y - 1];
                    var erosion = (gIndex + depth) % 20183;
                    grid[x, y] = erosion;
                    res += grid[x, y] % 3;
                }
            }


            //for (var y = 0; y < tarY + 1; y++)
            //{
            //    for (var x = 0; x < tarX + 1; x++)
            //    {
            //        if (x == tarX && y == tarY)
            //            return $"{res}";
            //        res += grid[x, y] % 3;
            //    }
            //}
            return $"{res}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var unvisited = new SimplePriorityQueue<Option>();
            var depth = int.Parse(rows[0].Split(' ')[1]);
            var tarX = int.Parse(rows[1].Split(' ')[1].Split(',')[0]);
            var tarY = int.Parse(rows[1].Split(' ')[1].Split(',')[1]);

            var maxX = tarX + 50;
            var maxY = tarY + 150;

            var grid = new Node[maxX, maxY];
            grid[0, 0] = new Node(0, depth, 0, 0, unvisited);

            for (var i = 1; i < maxX; i++)
            {
                var tNode = new Node(i * 16807, depth, i, 0, unvisited);
                var n = grid[i - 1, 0];
                AddOptions(n, tNode);

                grid[i, 0] = tNode;
            }
            for (var i = 1; i < maxY; i++)
            {
                var tNode = new Node(i * 48271, depth, 0, i, unvisited);
                var n = grid[0, i - 1];
                AddOptions(n, tNode);
                grid[0, i] = tNode;
            }

            for (var y = 1; y < maxY; y++)
            {
                for (var x = 1; x < maxX; x++)
                {
                    Node tNode;
                    if(x == tarX && y == tarY)
                        tNode = new Node(0, depth, x, y, unvisited);
                    else 
                        tNode = new Node(grid[x - 1, y].Erosion * grid[x, y - 1].Erosion, depth, x, y, unvisited);

                    var n = grid[x - 1, y];
                    AddOptions(n, tNode);

                    n = grid[x, y - 1];
                    AddOptions(n, tNode);
                    
                    grid[x, y] = tNode;
                }
            }
            var visited = new HashSet<Option>();
            Option target = null;
            while(unvisited.Count > 0)
            {
                var cur = unvisited.Dequeue();

                foreach(var n in cur.Neighbours)
                {
                    if (visited.Contains(n.Item1)) continue;
                    var tDis = cur.Distance + n.Item2;
                    if (tDis < n.Item1.Distance)
                    {
                        n.Item1.Distance = tDis;
                        n.Item1.Parent = cur;
                        unvisited.UpdatePriority(n.Item1, tDis);
                    }
                        
                }

                visited.Add(cur);
                if (cur.X == tarX && cur.Y == tarY && cur.Tool == Tool.Torch)
                {
                    target = cur;
                }
            }

            return $"{target.Distance}";
        }

        private static void AddOptions(Node n, Node tNode)
        {
            // There is probably a more compact way to to this stuff
            // Many Else if to make sure there is no illegal neighbour
            foreach (var tool in tNode.AllowedTools)
            {
                foreach (var nTool in n.AllowedTools)
                {
                    if (tool.Tool == nTool.Tool)
                    {
                        tool.Neighbours.Add(new Tuple<Option, int>(nTool, 1));
                        nTool.Neighbours.Add(new Tuple<Option, int>(tool, 1));
                    }
                    else if(tool.Type == 0 && nTool.Type == 1)
                    {
                        if((tool.Tool == Tool.Torch && nTool.Tool == Tool.Climbing)
                            || (tool.Tool == Tool.Climbing && nTool.Tool == Tool.Nothing))
                        {
                            tool.Neighbours.Add(new Tuple<Option, int>(nTool, 8));
                            nTool.Neighbours.Add(new Tuple<Option, int>(tool, 8));
                        }
                    }
                    else if (tool.Type == 1 && nTool.Type == 0)
                    {
                        if ((tool.Tool == Tool.Climbing && nTool.Tool == Tool.Torch)
                            ||(tool.Tool == Tool.Nothing && nTool.Tool == Tool.Climbing))
                        {
                            tool.Neighbours.Add(new Tuple<Option, int>(nTool, 8));
                            nTool.Neighbours.Add(new Tuple<Option, int>(tool, 8));
                        }
                    }
                    else if(tool.Type == 0 && nTool.Type == 2)
                    {
                        if ((tool.Tool == Tool.Climbing && nTool.Tool == Tool.Torch)
                            || (tool.Tool == Tool.Torch && nTool.Tool == Tool.Nothing))
                        {
                            tool.Neighbours.Add(new Tuple<Option, int>(nTool, 8));
                            nTool.Neighbours.Add(new Tuple<Option, int>(tool, 8));
                        }
                    }
                    else if (tool.Type == 2 && nTool.Type == 0)
                    {
                        if ((tool.Tool == Tool.Torch && nTool.Tool == Tool.Climbing)
                            || (tool.Tool == Tool.Nothing && nTool.Tool == Tool.Torch))
                        {
                            tool.Neighbours.Add(new Tuple<Option, int>(nTool, 8));
                            nTool.Neighbours.Add(new Tuple<Option, int>(tool, 8));
                        }
                    }
                    else if (tool.Type == 1 && nTool.Type == 2)
                    {
                        if ((tool.Tool == Tool.Climbing && nTool.Tool == Tool.Nothing)
                            || (tool.Tool == Tool.Nothing && nTool.Tool == Tool.Torch))
                        {
                            tool.Neighbours.Add(new Tuple<Option, int>(nTool, 8));
                            nTool.Neighbours.Add(new Tuple<Option, int>(tool, 8));
                        }
                    }
                    else if (tool.Type == 2 && nTool.Type == 1)
                    {
                        if ((tool.Tool == Tool.Nothing && nTool.Tool == Tool.Climbing)
                            || (tool.Tool == Tool.Torch && nTool.Tool == Tool.Nothing))
                        {
                            tool.Neighbours.Add(new Tuple<Option, int>(nTool, 8));
                            nTool.Neighbours.Add(new Tuple<Option, int>(tool, 8));
                        }
                    }
                }
            }
        }

        private class Node
        {
            public int GIndex { get; set; }
            public int Erosion { get; set; }
            public int Type { get; set; }
            public HashSet<Option> AllowedTools { get; set; }

            public Node(int gIndex, int depth, int x, int y, SimplePriorityQueue<Option> unvisited)
            {
                GIndex = gIndex;
                Erosion = (gIndex + depth) % 20183;
                Type = Erosion % 3;
                AllowedTools = new HashSet<Option>();
                if(x == 0 && y == 0)
                {
                    AllowedTools.Add(new Option(x, y, Tool.Torch, Type));
                }
                else if(Type == 0)
                {
                    AllowedTools.Add(new Option(x, y, Tool.Climbing, Type));
                    AllowedTools.Add(new Option(x, y, Tool.Torch, Type));
                }
                else if(Type == 1)
                {
                    AllowedTools.Add(new Option(x, y, Tool.Climbing, Type));
                    AllowedTools.Add(new Option(x, y, Tool.Nothing, Type));
                }
                else
                {
                    AllowedTools.Add(new Option(x, y, Tool.Torch, Type));
                    AllowedTools.Add(new Option(x, y, Tool.Nothing, Type));
                }

                foreach(var tool in AllowedTools)
                {
                    unvisited.Enqueue(tool, tool.Distance);
                }
            }
        }

        private enum Tool
        {
            Torch,
            Climbing,
            Nothing
        }
        private class Option : IComparable<Option>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Tool Tool { get; set; }
            public List<Tuple<Option, int>> Neighbours {get; set;}
            public int Distance { get; set; }
            public Option Parent { get; set; }
            public int Type { get; set; }

            public Option(int x, int y, Tool tool, int type)
            {
                X = x;
                Y = y;
                Tool = tool;
                Neighbours = new List<Tuple<Option, int>>();
                Type = type;
                if (x == 0 && y == 0)
                    Distance = 0;
                else
                    Distance = int.MaxValue;
            }

            public override bool Equals(object obj)
            {
                var option = obj as Option;
                return option != null &&
                       X == option.X &&
                       Y == option.Y &&
                       Tool == option.Tool;
            }

            public override int GetHashCode()
            {
                var hashCode = -532945230;
                hashCode = hashCode * -1521134295 + X.GetHashCode();
                hashCode = hashCode * -1521134295 + Y.GetHashCode();
                hashCode = hashCode * -1521134295 + Tool.GetHashCode();
                return hashCode;
            }

            public int CompareTo(Option other)
            {
                if (other == null)
                    return 1;
                if (Distance < other.Distance)
                    return -1;
                if (Distance > other.Distance)
                    return 1;
                return 0;
            }
        }
    }
}
