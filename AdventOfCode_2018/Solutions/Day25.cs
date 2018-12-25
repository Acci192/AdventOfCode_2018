using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day25
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var constelations = new Dictionary<int, List<Position>>();
            var counter = 1;
            foreach (var row in rows)
            {
                var t = row.Split(',');
                var x = int.Parse(t[0]);
                var y = int.Parse(t[1]);
                var z = int.Parse(t[2]);
                var q = int.Parse(t[3]);
                var newPos = new Position(x, y, z, q);
                if (constelations.Count == 0)
                {
                    constelations[counter++] = new List<Position> { newPos };
                    continue;
                }

                var found = false;
                foreach(var key in constelations.Keys)
                {
                    foreach(var pos in constelations[key])
                    {
                        if(pos.GetDistance(newPos) <= 3)
                        {
                            constelations[key].Add(newPos);
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                if (!found)
                {
                    constelations[counter++] = new List<Position> { newPos };
                }
            }
            var changed = true;
            while (changed)
            {
                changed = false;
                var mergeFrom = -1;
                var mergeTo = -1;

                foreach(var key in constelations.Keys)
                {
                    foreach(var key2 in constelations.Keys)
                    {
                        if (key == key2) continue;

                        foreach(var pos1 in constelations[key])
                        {
                            foreach(var pos2 in constelations[key2])
                            {
                                if(pos1.GetDistance(pos2) <= 3)
                                {
                                    mergeFrom = key2;
                                    mergeTo = key;
                                    changed = true;
                                    break;
                                }
                            }
                            if (changed) break;
                        }
                        if (changed) break;
                    }
                    if (changed) break;
                }
                if (changed)
                {
                    foreach (var pos in constelations[mergeFrom])
                    {
                        constelations[mergeTo].Add(pos);
                    }
                    constelations.Remove(mergeFrom);
                }
            }

            return $"{constelations.Count}";
        }

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int Q { get; set; }

            public Position(int x, int y, int z, int q)
            {
                X = x;
                Y = y;
                Z = z;
                Q = q;
            }

            public int GetDistance(Position other)
            {
                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z) + Math.Abs(Q - other.Q);
            }
        }
    }
}
