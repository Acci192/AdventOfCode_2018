using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode_2018.Solutions
{
    public class Day9
    {
        private static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static string A(string input)
        {
            var inp = input.Split(' ');
            var numPlayers = int.Parse(inp[0]);
            var lastMarble = int.Parse(inp[6]);

            var players = new int[numPlayers];

            var currentPlayer = 2;
            var marbles = new List<int>();
            var currentIndex = 1;

            marbles.Add(0);
            marbles.Add(1);
            for(int i = 2; i < lastMarble; i++)
            {
                if(i % 23 != 0)
                {
                    var newCurrent = (currentIndex + 2) % marbles.Count();
                    marbles.Insert(newCurrent, i);
                    currentIndex = newCurrent;
                }
                else
                {
                    var newCurrent = mod((currentIndex - 7),  marbles.Count());
                    players[currentPlayer] += i + marbles[newCurrent];
                    marbles.RemoveAt(newCurrent);
                    currentIndex = newCurrent;
                }

                currentPlayer = (currentPlayer + 1) % numPlayers;
            }
            
            return $"{players.Max()}";
        }

        public static string B(string input)
        {
            var inp = input.Split(' ');
            var numPlayers = int.Parse(inp[0]);
            var lastMarble = int.Parse(inp[6]) * 100;

            var players = new long[numPlayers];

            var marbles = new LinkedList<int>();

            marbles.AddFirst(0);
            var currentNode = marbles.First;
            currentNode = marbles.AddAfter(currentNode, 1);

            for (int i = 2; i < lastMarble; i++)
            {
                if (i % 23 != 0)
                {
                    if(currentNode.Next == null)
                    {
                        currentNode = marbles.First;
                    }
                    else
                    {
                        currentNode = currentNode.Next;
                    }
                    currentNode = marbles.AddAfter(currentNode, i);
                }
                else
                {
                    players[i % numPlayers] += i;
                    for(var j = 0; j < 6; j++)
                    {
                        if(currentNode.Previous == null)
                        {
                            currentNode = marbles.Last;
                        }
                        else
                        {
                            currentNode = currentNode.Previous;
                        }
                    }
                    if (currentNode.Previous == null)
                    {
                        players[i % numPlayers] += marbles.Last();
                        marbles.RemoveLast();
                    }
                    else
                    {
                        players[i % numPlayers] += currentNode.Previous.Value;
                        marbles.Remove(currentNode.Previous);
                    }
                }
            }

            return $"{players.Max()}";
        }
    }
}
