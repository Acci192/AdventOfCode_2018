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
            for(var i = 0; i < height; i++)
            {
                for(var j = 0; j < width; j++)
                {
                    grid[j, i] = rows[i][j];
                }
            }
            var cars = new List<Car>();
            // Find cars
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (grid[x, y])
                    {
                        case '<':
                            cars.Add(new Car(x, y, 3));
                            ReplaceCar(x, y);
                            break;
                        case '>':
                            cars.Add(new Car(x, y, 1));
                            ReplaceCar(x, y);
                            break;
                        case '^':
                            cars.Add(new Car(x, y, 0));
                            ReplaceCar(x, y);
                            break;
                        case 'v':
                            cars.Add(new Car(x, y, 2));
                            ReplaceCar(x, y);
                            break;
                        default:
                            break;
                    }
                }
            }
            
            for (var i = 0; i < 1000; i++)
            {
                var unique = new HashSet<int>();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var curCar = cars.FirstOrDefault(carz => carz.X == x && carz.Y == y);
                        if(curCar != null)
                        {
                            if (unique.Add(curCar.Id))
                            {
                                curCar.Move();
                                if (cars.Count(c => c.X == curCar.X && c.Y == curCar.Y) > 1)
                                {
                                    var temp = cars.Where(carz => carz.X == curCar.X && carz.Y == curCar.Y);
                                    foreach (var tempCar in temp)
                                    {
                                        Console.WriteLine($"{tempCar.Id} : {tempCar.X},{tempCar.Y}");
                                    }
                                }
                            }
                            
                        }
                    }
                }
                //foreach (var car in cars)
                //{
                //    car.Move();
                //    //if((cars.Count(carz => carz.X == car.X && carz.Y == car.Y) > 1)){
                //    //    Console.WriteLine($"{car.X},{car.Y}");
                //    //    var temp = cars.Where(carz => carz.X == car.X && carz.Y == car.Y);
                //    //    foreach(var tempCar in temp)
                //    //    {
                //    //        Console.WriteLine($"{tempCar.Id} : {tempCar.X},{tempCar.Y}");
                //    //    }

                //    //}
                //    if (grid[car.X, car.Y] == ' ')
                //    {
                //        Console.WriteLine("ERROR");
                //    }
                //}
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (cars.Count(carz => carz.X == x && carz.Y == y) > 1)
                        {
                            var temp = cars.Where(carz => carz.X == x && carz.Y == y);
                            foreach (var tempCar in temp)
                            {
                                Console.WriteLine($"{tempCar.Id} : {tempCar.X},{tempCar.Y}");
                            }
                            //return "";
                        }
                    }
                }
            }

            //PrintMap(grid, width);
            return $"Result";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var width = rows[0].Length;
            var height = rows.Count();
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    grid[j, i] = rows[i][j];
                }
            }
            var cars = new List<Car>();
            // Find cars
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (grid[x, y])
                    {
                        case '<':
                            cars.Add(new Car(x, y, 3));
                            ReplaceCar(x, y);
                            break;
                        case '>':
                            cars.Add(new Car(x, y, 1));
                            ReplaceCar(x, y);
                            break;
                        case '^':
                            cars.Add(new Car(x, y, 0));
                            ReplaceCar(x, y);
                            break;
                        case 'v':
                            cars.Add(new Car(x, y, 2));
                            ReplaceCar(x, y);
                            break;
                        default:
                            break;
                    }
                }
            }

            for (var i = 0; i < 100000; i++)
            {
                var unique = new HashSet<int>();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var curCar = cars.FirstOrDefault(carz => carz.X == x && carz.Y == y);
                        if (curCar != null)
                        {
                            if (unique.Add(curCar.Id))
                            {
                                curCar.Move();
                                if (cars.Count(c => c.X == curCar.X && c.Y == curCar.Y) > 1)
                                {
                                    var temp = cars.Where(carz => carz.X == curCar.X && carz.Y == curCar.Y);
                                    cars = cars.Where(c => temp.Count(t => t.Id == c.Id) == 0).ToList();
                                    
                                }
                            }
                        }
                    }
                }
                if (cars.Count() == 1)
                {
                    break;
                }
                //foreach (var car in cars)
                //{
                //    car.Move();
                //    //if((cars.Count(carz => carz.X == car.X && carz.Y == car.Y) > 1)){
                //    //    Console.WriteLine($"{car.X},{car.Y}");
                //    //    var temp = cars.Where(carz => carz.X == car.X && carz.Y == car.Y);
                //    //    foreach(var tempCar in temp)
                //    //    {
                //    //        Console.WriteLine($"{tempCar.Id} : {tempCar.X},{tempCar.Y}");
                //    //    }

                //    //}
                //    if (grid[car.X, car.Y] == ' ')
                //    {
                //        Console.WriteLine("ERROR");
                //    }
                //}


                //for (int y = 0; y < height; y++)
                //{
                //    for (int x = 0; x < width; x++)
                //    {
                //        if (cars.Count(carz => carz.X == x && carz.Y == y) > 1)
                //        {
                //            var temp = cars.Where(carz => carz.X == x && carz.Y == y);
                //            foreach (var tempCar in temp)
                //            {
                //                Console.WriteLine($"{tempCar.Id} : {tempCar.X},{tempCar.Y}");
                //            }
                //            //return "";
                //        }
                //    }
                //}
            }

            //PrintMap(grid, width);
            return $"Result";
        }

        private static void ReplaceCar(int x, int y)
        {
            if((grid[x - 1, y] == '-' && (grid[x-1,y] == grid[x+1, y] || grid[x + 1, y] == '+'))
                || (grid[x + 1, y] == '-' && (grid[x - 1, y] == grid[x + 1, y] || grid[x - 1, y] == '+')))
            {
                grid[x, y] = '-';
            }
            else
            {
                grid[x, y] = '|';
            }
        }

        private static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        private class Car
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Direction { get; set; }
            public int NextTurn { get; set; }
            private static int idCounter = 0;
            public int Id { get; set; }

            public Car(int x, int y, int direction)
            {
                X = x;
                Y = y;
                Direction = direction;
                NextTurn = -1;
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
        }

        private static void PrintMap(char[,] map, int size)
        {
            for(int y = 0; y < size; y++)
            {
                for(int x = 0; x < size; x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.Write('\n');
            }
        }
    }
}
