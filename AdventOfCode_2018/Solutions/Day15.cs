using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    /*
     * Read this code on your own risk
     * The solution is not pretty but it does however find the answers
     * 
     * I will try to come back to this and fix it up, it deserves better.
     */
    public class Day15
    {
        public static char[,] Grid;
        public static int h;
        public static int w;

        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            h = rows.Count;
            w = rows[0].Length;
            Grid = new char[w, h];
            var players = new List<Player>();

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Grid[x, y] = rows[y][x];
                    if (Grid[x, y] == 'E') players.Add(new Player(x, y, true, 200, 3));
                    else if (Grid[x, y] == 'G') players.Add(new Player(x, y, false, 200, 3));
                }
            }

            var numGoblins = players.Count(x => !x.IsElf);
            var numElfs = players.Count(x => x.IsElf);

            var count = 0;

            while (numGoblins > 0 && numElfs > 0)
            {
                // Make sure the right order is contained
                players.Sort();

                //For each player make an action
                foreach (var player in players)
                {
                    numGoblins = players.Count(x => !x.IsElf && x.IsAlive);
                    numElfs = players.Count(x => x.IsElf && x.IsAlive);
                    if (numElfs == 0 || numGoblins == 0)
                    {
                        var b = players.Where(p => p.IsAlive).Select(p => p.HitPoints).Sum();
                        PrintGrid();
                        return (b * (count)).ToString();
                    }

                    if (!player.IsAlive) continue;

                    var adjacentEnemies = new List<Player>();
                    // Check if enemy is adjecent
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (Math.Abs(dx) == Math.Abs(dy)) continue;
                            var temp = players.FirstOrDefault(p => p.X == player.X + dx && p.Y == player.Y + dy && p.IsElf != player.IsElf && p.IsAlive);
                            if (temp != null)
                                adjacentEnemies.Add(temp);
                        }
                    }

                    // Attack lowest enemy if enemies is adjecent
                    if (adjacentEnemies.Count > 0)
                    {
                        var target = adjacentEnemies.Where(p => p.HitPoints == adjacentEnemies.Min(t => t.HitPoints)).Min();
                        target.HitPoints -= player.AttackPower;
                        if (!target.IsAlive)
                        {
                            Grid[target.X, target.Y] = '.';

                        }
                        continue;
                    }

                    var points = new HashSet<Tuple<int, int>>();

                    var enemies = players.Where(p => p.IsElf != player.IsElf && p.IsAlive).ToList();

                    foreach (var e in enemies)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (Math.Abs(dx) == Math.Abs(dy)) continue;
                                if (Grid[e.X + dx, e.Y + dy] == '.')
                                    points.Add(new Tuple<int, int>(e.X + dx, e.Y + dy));
                            }
                        }
                    }

                    var nextMove = BestWay(points, player.X, player.Y);
                    if (nextMove == null) continue;
                    Grid[player.X, player.Y] = '.';
                    player.X = nextMove.Item1;
                    player.Y = nextMove.Item2;
                    if (player.IsElf) Grid[player.X, player.Y] = 'E';
                    else Grid[player.X, player.Y] = 'G';

                    adjacentEnemies = new List<Player>();
                    // Check if enemy is adjecent
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (Math.Abs(dx) == Math.Abs(dy)) continue;
                            var temp = players.FirstOrDefault(p => p.X == player.X + dx && p.Y == player.Y + dy && p.IsElf != player.IsElf && p.IsAlive);
                            if (temp != null)
                                adjacentEnemies.Add(temp);
                        }
                    }

                    // Attack lowest enemy if enemies is adjecent
                    if (adjacentEnemies.Count > 0)
                    {
                        var target = adjacentEnemies.Where(p => p.HitPoints == adjacentEnemies.Min(t => t.HitPoints)).Min();
                        target.HitPoints -= player.AttackPower;
                        if (!target.IsAlive)
                        {
                            Grid[target.X, target.Y] = '.';

                        }
                        continue;
                    }
                }

                players.RemoveAll(p => !p.IsAlive);
                numGoblins = players.Count(x => !x.IsElf);
                numElfs = players.Count(x => x.IsElf);
                count++;
            }

            PrintGrid();
            var a = players.Where(p => p.IsAlive).Select(p => p.HitPoints).Sum();
            return (a * (count)).ToString();
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            h = rows.Count;
            w = rows[0].Length;
            var ElfHp = 3;
            var fail = false;
            while (true)
            {
                fail = false;
                ElfHp++;
                Grid = new char[w, h];
                var players = new List<Player>();

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        Grid[x, y] = rows[y][x];
                        if (Grid[x, y] == 'E') players.Add(new Player(x, y, true, 200, ElfHp));
                        else if (Grid[x, y] == 'G') players.Add(new Player(x, y, false, 200, 3));
                    }
                }

                var numGoblins = players.Count(x => !x.IsElf);
                var numElfs = players.Count(x => x.IsElf);

                var count = 0;
                var gHp = 0;
                var eHp = 0;

                while (numGoblins > 0 && numElfs > 0)
                {
                    gHp = players.Where(p => !p.IsElf && p.IsAlive).Select(p => p.HitPoints).Sum();
                    eHp = players.Where(p => p.IsElf && p.IsAlive).Select(p => p.HitPoints).Sum();
                    // Make sure the right order is contained
                    players.Sort();

                    //For each player make an action
                    foreach (var player in players)
                    {
                        if (fail) break;
                        numGoblins = players.Count(x => !x.IsElf && x.IsAlive);
                        numElfs = players.Count(x => x.IsElf && x.IsAlive);
                        if (numElfs == 0 || numGoblins == 0)
                        {
                            var b = players.Where(p => p.IsAlive).Select(p => p.HitPoints).Sum();
                            PrintGrid();
                            return (b * (count)).ToString();
                        }

                        if (!player.IsAlive) continue;

                        var adjecentEnemies = new List<Player>();
                        // Check if enemy is adjecent
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (Math.Abs(dx) == Math.Abs(dy)) continue;
                                var temp = players.FirstOrDefault(p => p.X == player.X + dx && p.Y == player.Y + dy && p.IsElf != player.IsElf && p.IsAlive);
                                if (temp != null)
                                    adjecentEnemies.Add(temp);
                            }
                        }

                        // Attack lowest enemy if enemies is adjecent
                        if (adjecentEnemies.Count > 0)
                        {
                            var target = adjecentEnemies.Where(p => p.HitPoints == adjecentEnemies.Min(t => t.HitPoints)).Min();
                            target.HitPoints -= player.AttackPower;
                            if (!target.IsAlive)
                            {
                                Grid[target.X, target.Y] = '.';
                                if (target.IsElf)
                                {
                                    fail = true;
                                    break;
                                }

                            }
                            continue;
                        }

                        var points = new HashSet<Tuple<int, int>>();

                        var enemies = players.Where(p => p.IsElf != player.IsElf && p.IsAlive).ToList();

                        foreach (var e in enemies)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    if (Math.Abs(dx) == Math.Abs(dy)) continue;
                                    if (Grid[e.X + dx, e.Y + dy] == '.')
                                        points.Add(new Tuple<int, int>(e.X + dx, e.Y + dy));
                                }
                            }
                        }

                        var nextMove = BestWay(points, player.X, player.Y);
                        if (nextMove == null) continue;
                        Grid[player.X, player.Y] = '.';
                        player.X = nextMove.Item1;
                        player.Y = nextMove.Item2;
                        if (player.IsElf) Grid[player.X, player.Y] = 'E';
                        else Grid[player.X, player.Y] = 'G';

                        adjecentEnemies = new List<Player>();
                        // Check if enemy is adjecent
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (Math.Abs(dx) == Math.Abs(dy)) continue;
                                var temp = players.FirstOrDefault(p => p.X == player.X + dx && p.Y == player.Y + dy && p.IsElf != player.IsElf && p.IsAlive);
                                if (temp != null)
                                    adjecentEnemies.Add(temp);
                            }
                        }

                        // Attack lowest enemy if enemies is adjecent
                        if (adjecentEnemies.Count > 0)
                        {
                            var target = adjecentEnemies.Where(p => p.HitPoints == adjecentEnemies.Min(t => t.HitPoints)).Min();
                            target.HitPoints -= player.AttackPower;
                            if (!target.IsAlive)
                            {
                                Grid[target.X, target.Y] = '.';

                            }
                            continue;
                        }
                    }
                    if (fail) break;
                    players.RemoveAll(p => !p.IsAlive);
                    numGoblins = players.Count(x => !x.IsElf);
                    numElfs = players.Count(x => x.IsElf);
                    count++;
                }

                if (fail) continue;
                PrintGrid();
                var a = players.Where(p => p.IsAlive).Select(p => p.HitPoints).Sum();
                return (a * (count)).ToString();
            }
        }

        public static Tuple<int, int> BestWay(HashSet<Tuple<int, int>> poi, int sX, int sY)
        {
            var tempGrid = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (Grid[x, y] == '.') tempGrid[x, y] = 1000;
                    else tempGrid[x, y] = -1;
                }
            }

            tempGrid[sX, sY] = 0;

            var q = new Queue<Tuple<int, int>>();
            q.Enqueue(new Tuple<int, int>(sX, sY));

            Tuple<int, int> endPoint = null;
            var curLen = int.MaxValue;
            var found = false;
            while (q.Count > 0)
            {
                var current = q.Dequeue();
                // Check up first
                var temp = new Tuple<int, int>(current.Item1, current.Item2 - 1);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {

                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (poi.Contains(temp) && !found)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                    found = true;
                }

                // Check Left
                temp = new Tuple<int, int>(current.Item1 - 1, current.Item2);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {
                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (poi.Contains(temp) && !found)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                    found = true;
                }
                // Check right
                temp = new Tuple<int, int>(current.Item1 + 1, current.Item2);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {
                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (poi.Contains(temp) && !found)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                    found = true;
                }
                // Check down
                temp = new Tuple<int, int>(current.Item1, current.Item2 + 1);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {
                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (poi.Contains(temp) && !found)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                    found = true;
                }
            }


            if (endPoint == null)
            {
                return null;
            }
            var curX = endPoint.Item1;
            var curY = endPoint.Item2;
            var f = false;
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var tempPos = new Tuple<int, int>(x, y);
                    if (poi.Contains(tempPos) && tempGrid[x, y] == curLen)
                    {
                        curX = x;
                        curY = y;
                        f = true;
                        break;
                    }
                }
                if (f) break;
            }


            var test = new List<Tuple<int, int>> {
                new Tuple<int, int>(0, -1),
                new Tuple<int, int>(-1, 0),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(0, 1)
            };

            foreach (var t in test)
            {
                var tempX = sX + t.Item1;
                var tempY = sY + t.Item2;
                if (Grid[tempX, tempY] == '.')
                {
                    var x = getRange(curX, curY, sX + t.Item1, sY + t.Item2, curLen);
                    if (x == curLen - 1)
                    {
                        return new Tuple<int, int>(tempX, tempY);

                    }
                }

            }

            //Console.WriteLine($"X:{curX}, Y:{curY}");
            return new Tuple<int, int>(curX, curY);
        }
        private class Player : IComparable<Player>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int HitPoints { get; set; }
            public int AttackPower { get; set; }
            public bool IsElf { get; set; }
            public bool IsAlive { get { return HitPoints > 0; } }
            private static int IdCounter = 0;
            public int Id { get; }

            public Player(int x, int y, bool isElf, int hitPoints, int attackPower)
            {
                X = x;
                Y = y;
                HitPoints = hitPoints;
                AttackPower = attackPower;
                IsElf = isElf;
                Id = IdCounter++;
            }

            public int CompareTo(Player other)
            {
                if (other == null)
                    return 1;
                if (this.Y < other.Y)
                    return -1;
                if (this.Y > other.Y)
                    return 1;
                if (this.X < other.X)
                    return -1;
                if (this.X > other.X)
                    return 1;

                return 0;
            }
        }

        public static int getRange(int eX, int eY, int sX, int sY, int len)
        {
            var tempGrid = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (Grid[x, y] == '.') tempGrid[x, y] = 1000;
                    else tempGrid[x, y] = -1;
                }
            }
            tempGrid[sX, sY] = 0;

            var q = new Queue<Tuple<int, int>>();
            q.Enqueue(new Tuple<int, int>(sX, sY));

            Tuple<int, int> endPoint = null;
            var curLen = int.MaxValue;
            while (q.Count > 0)
            {
                var current = q.Dequeue();
                // Check up first
                var temp = new Tuple<int, int>(current.Item1, current.Item2 - 1);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {

                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (temp.Item1 == eX && temp.Item2 == eY)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                }

                // Check Left
                temp = new Tuple<int, int>(current.Item1 - 1, current.Item2);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {
                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (temp.Item1 == eX && temp.Item2 == eY)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                }
                // Check right
                temp = new Tuple<int, int>(current.Item1 + 1, current.Item2);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {
                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (temp.Item1 == eX && temp.Item2 == eY)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                }
                // Check down
                temp = new Tuple<int, int>(current.Item1, current.Item2 + 1);
                if (tempGrid[temp.Item1, temp.Item2] == 1000)
                {
                    tempGrid[temp.Item1, temp.Item2] = tempGrid[current.Item1, current.Item2] + 1;
                    if (tempGrid[temp.Item1, temp.Item2] < curLen)
                        q.Enqueue(temp);
                }
                if (temp.Item1 == eX && temp.Item2 == eY)
                {
                    curLen = tempGrid[temp.Item1, temp.Item2];
                    endPoint = temp;
                }

                if (tempGrid[temp.Item1, temp.Item2] > len)
                    break;
            }
            return curLen;
        }

        public static void PrintGrid()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Console.Write(Grid[x, y]);
                }
                Console.Write('\n');
            }
        }

        public static void PrintGrid(int[,] arr)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (Grid[x, y] == '.' && arr[x, y] > 0 && arr[x, y] < 1000)
                    {
                        Console.Write(arr[x, y]);
                    }
                    else
                        Console.Write(Grid[x, y]);
                }
                Console.Write('\n');
            }
        }
    }
}
