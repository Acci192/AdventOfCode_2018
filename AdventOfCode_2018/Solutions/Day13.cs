using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day13
    {
        public static char[,] grid = new char[150, 150];
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var width = rows[0].Length;
            var height = rows.Count();
            var cars = new List<Car>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    switch (rows[y][x])
                    {
                        case '<':
                            cars.Add(new Car(x, y, 3));
                            grid[x, y] = '-';
                            break;
                        case '>':
                            cars.Add(new Car(x, y, 1));
                            grid[x, y] = '-';
                            break;
                        case '^':
                            cars.Add(new Car(x, y, 0));
                            grid[x, y] = '|';
                            break;
                        case 'v':
                            cars.Add(new Car(x, y, 2));
                            grid[x, y] = '|';
                            break;
                        default:
                            grid[x, y] = rows[y][x];
                            break;
                    }
                }
            }

            while (cars.Count() > 1)
            {
                cars.Sort();

                foreach (var car in cars)
                {
                    car.Move();
                    if(cars.Count(c => c.X == car.X && c.Y == car.Y && c.Id != car.Id) > 0)
                    {
                        car.Alive = false;
                        return $"{car.X},{car.Y}";
                    }
                }
            }
            var collisionCar = cars.FirstOrDefault(c => !c.Alive);
            return $"{collisionCar.X},{collisionCar.Y}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var width = rows[0].Length;
            var height = rows.Count();
            var cars = new List<Car>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    switch (rows[y][x])
                    {
                        case '<':
                            cars.Add(new Car(x, y, 3));
                            grid[x, y] = '-';
                            break;
                        case '>':
                            cars.Add(new Car(x, y, 1));
                            grid[x, y] = '-';
                            break;
                        case '^':
                            cars.Add(new Car(x, y, 0));
                            grid[x, y] = '|';
                            break;
                        case 'v':
                            cars.Add(new Car(x, y, 2));
                            grid[x, y] = '|';
                            break;
                        default:
                            grid[x, y] = rows[y][x];
                            break;
                    }
                }
            }

            while (cars.Count() > 1)
            {
                cars.Sort();

                foreach(var car in cars)
                {
                    car.Move();
                    var collidingCars = cars.Where(c => c.X == car.X && c.Y == car.Y && c.Id != car.Id).ToList();
                    if(collidingCars.Count > 0)
                    {
                        foreach(var c in collidingCars)
                        {
                            c.Alive = false;
                        }
                        car.Alive = false;
                    }
                }
                cars.RemoveAll(c => !c.Alive);
            }

            var lastCar = cars.FirstOrDefault();
            return $"{lastCar.X},{lastCar.Y}";
        }

        private static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        private class Car : IComparable<Car>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Direction { get; set; }
            public int NextTurn { get; set; }
            public bool Alive { get; set; }
            private static int idCounter = 0;
            public int Id { get; set; }

            public Car(int x, int y, int direction)
            {
                X = x;
                Y = y;
                Direction = direction;
                NextTurn = -1;
                Alive = true;
                Id = idCounter++;
            }

            public void Move()
            {
                if (grid[X, Y] == '+')
                {
                    Direction = mod((Direction + NextTurn), 4);
                    NextTurn = mod(NextTurn + 2, 3) - 1;
                }
                else if (grid[X, Y] == '/')
                {
                    if (Direction == 0)
                    {
                        Direction = 1;
                    }
                    else if (Direction == 1)
                    {
                        Direction = 0;
                    }
                    else if (Direction == 2)
                    {
                        Direction = 3;
                    }
                    else if (Direction == 3)
                    {
                        Direction = 2;
                    }
                }
                else if (grid[X, Y] == '\\')
                {
                    if (Direction == 0)
                    {
                        Direction = 3;
                    }
                    else if (Direction == 1)
                    {
                        Direction = 2;
                    }
                    else if (Direction == 2)
                    {
                        Direction = 1;
                    }
                    else if (Direction == 3)
                    {
                        Direction = 0;
                    }
                }
                switch (Direction)
                {
                    // Up
                    case 0:
                        Y--;
                        break;
                    // Right
                    case 1:
                        X++;
                        break;
                    // Down
                    case 2:
                        Y++;
                        break;
                    // Left
                    case 3:
                        X--;
                        break;
                    default:
                        break;
                }
            }

            public int CompareTo(Car other)
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
    }
}
