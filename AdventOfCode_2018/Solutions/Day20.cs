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
            var size = 204;
            var grid = new char[size, size];

            var curX = size/2;
            var curY = size/2;

            var stack = new Stack<Tuple<int, int>>();

            for(var i = 1; i < input.Length; i++)
            {
                var c = input[i];
                FillGrid(curX, curY, c, grid);
                if (c == 'N' || c == 'E' || c == 'S' || c == 'W')
                {
                    switch (c)
                    {
                        case 'N':
                            curY -= 2;
                            break;
                        case 'E':
                            curX += 2;
                            break;
                        case 'S':
                            curY += 2;
                            break;
                        case 'W':
                            curX -= 2;
                            break;
                    }
                }
                else if(c == '(')
                {
                    stack.Push(new Tuple<int, int>(curX, curY));
                }
                else if(c == '|')
                {
                    curX = stack.Peek().Item1;
                    curY = stack.Peek().Item2;
                }
                else if(c == ')')
                {
                    stack.Pop();
                }
            }
            var q = new Queue<Node>();
            var seen = new HashSet<Node>();
            var startX = size / 2;
            var startY = size / 2;
            q.Enqueue(new Node(startX, startY, 0));
            seen.Add(new Node(startX, startY, 0));

            while (q.Count > 0)
            {
                var curNode = q.Dequeue();
                var cX = curNode.X;
                var cY = curNode.Y;

                var north = new Node(cX, cY - 2, curNode.NumDoors+1);
                var east = new Node(cX + 2, cY, curNode.NumDoors + 1);
                var south = new Node(cX, cY + 2, curNode.NumDoors + 1);
                var west = new Node(cX - 2, cY, curNode.NumDoors + 1);
                if (grid[cX,cY - 1] == '-' && !seen.Contains(north))
                {
                    q.Enqueue(north);
                    seen.Add(north);
                }
                if (grid[cX + 1, cY] == '|' && !seen.Contains(east))
                {
                    q.Enqueue(east);
                    seen.Add(east);
                }
                if (grid[cX, cY + 1] == '-' && !seen.Contains(south))
                {
                    q.Enqueue(south);
                    seen.Add(south);
                }
                if (grid[cX - 1, cY] == '|' && !seen.Contains(west))
                {
                    q.Enqueue(west);
                    seen.Add(west);
                }
            }

            var resA = seen.Max(x => x.NumDoors);
            var resB = seen.Count(x => x.NumDoors >= 1000);
            grid[size / 2, size / 2] = 'X';
            //for (var y = 0; y < size; y++)
            //{
            //    for (var x = 0; x < size; x++)
            //    {
            //        if (grid[x, y] == '\0') grid[x, y] = '#';
            //        Console.Write($"{grid[x, y]}");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine();

            return $"A: {resA}\nB:{resB}";
        }


        public class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int NumDoors { get; set; }

            public Node(int x, int y, int numDoors)
            {
                X = x;
                Y = y;
                NumDoors = numDoors;
            }

            public override bool Equals(object obj)
            {
                var node = obj as Node;
                return node != null &&
                       X == node.X &&
                       Y == node.Y;
            }

            public override int GetHashCode()
            {
                var hashCode = 1861411795;
                hashCode = hashCode * -1521134295 + X.GetHashCode();
                hashCode = hashCode * -1521134295 + Y.GetHashCode();
                return hashCode;
            }
        }

        public static void FillGrid(int x, int y, char c, char[,] g)
        {
            g[x + 1, y + 1] = '#';
            g[x + 1, y - 1] = '#';
            g[x - 1, y + 1] = '#';
            g[x - 1, y - 1] = '#';
            g[x, y] = '.';
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
                    g[x - 1, y] = '|';
                    break;
            }
        }
    }
}
