using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day21
    {
        private static Dictionary<string, Action<int, int, int, int[]>> Operations =
            new Dictionary<string, Action<int, int, int, int[]>> {
                {"addr", addr },
                {"addi", addi },
                {"mulr", mulr },
                {"muli", muli },
                {"banr", banr },
                {"bani", bani },
                {"borr", borr },
                {"bori", bori },
                {"setr", setr },
                {"seti", seti },
                {"gtir", gtir },
                {"gtri", gtri },
                {"gtrr", gtrr },
                {"eqir", eqir },
                {"eqri", eqri },
                {"eqrr", eqrr }
            };
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var ins = new List<Tuple<string, int[]>>();
            var pc = -1;
            foreach (var row in rows)
            {
                var temp = row.Split(' ');
                if (temp.Length == 2)
                {
                    pc = int.Parse(temp[1]);
                }
                else
                {
                    var inp = new int[3];
                    inp[0] = int.Parse(temp[1]);
                    inp[1] = int.Parse(temp[2]);
                    inp[2] = int.Parse(temp[3]);

                    ins.Add(new Tuple<string, int[]>(temp[0], inp));
                }
            }

            var registers = new int[6];
            registers[0] = 0;
            while (registers[pc] >= 0 && registers[pc] < ins.Count)
            {
                var i = ins[registers[pc]];
                Operations[i.Item1](i.Item2[0], i.Item2[1], i.Item2[2], registers);

                registers[pc]++;

                if(registers[pc] == 28)
                {
                    return $"{registers[1]}";
                }
            }

            return $"";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var ins = new List<Tuple<string, int[]>>();
            var pc = -1;
            foreach (var row in rows)
            {
                var temp = row.Split(' ');
                if (temp.Length == 2)
                {
                    pc = int.Parse(temp[1]);
                }
                else
                {
                    var inp = new int[3];
                    inp[0] = int.Parse(temp[1]);
                    inp[1] = int.Parse(temp[2]);
                    inp[2] = int.Parse(temp[3]);

                    ins.Add(new Tuple<string, int[]>(temp[0], inp));
                }
            }

            var registers = new int[6];
            var valid = new HashSet<int>();
            var last = int.MinValue;
            while (registers[pc] >= 0 && registers[pc] < ins.Count)
            {
                var i = ins[registers[pc]];
                Operations[i.Item1](i.Item2[0], i.Item2[1], i.Item2[2], registers);

                registers[pc]++;

                if (registers[pc] == 28)
                {
                    if (valid.Add(registers[1]))
                    {
                        last = registers[1];
                    }
                    else
                    {
                        return $"{last}";
                    }
                }
            }

            return $"";
        }

        public static void addr(int a, int b, int c, int[] reg)
        {
            reg[c] = reg[a] + reg[b];
        }

        public static void addi(int a, int b, int c, int[] reg)
        {
            reg[c] = reg[a] + b;
        }

        public static void mulr(int a, int b, int c, int[] reg)
        {
            reg[c] = reg[a] * reg[b];
        }

        public static void muli(int a, int b, int c, int[] reg)
        {
            reg[c] = reg[a] * b;
        }

        public static void banr(int a, int b, int c, int[] reg)
        {
            
            reg[c] = reg[a] & reg[b];
            
        }

        public static void bani(int a, int b, int c, int[] reg)
        {  
            reg[c] = reg[a] & b;
        }

        public static void borr(int a, int b, int c, int[] reg)
        {
            reg[c] = reg[a] | reg[b];           
        }

        public static void bori(int a, int b, int c, int[] reg)
        {           
            reg[c] = reg[a] | b;            
        }

        public static void setr(int a, int b, int c, int[] reg)
        {           
            reg[c] = reg[a];          
        }

        public static void seti(int a, int b, int c, int[] reg)
        {         
            reg[c] = a;            
        }
        public static void gtir(int a, int b, int c, int[] reg)
        {
            if (a > reg[b]) reg[c] = 1;
            else reg[c] = 0;
        }
        public static void gtri(int a, int b, int c, int[] reg)
        {
            if (reg[a] > b) reg[c] = 1;
            else reg[c] = 0;
        }

        public static void gtrr(int a, int b, int c, int[] reg)
        {
            if (reg[a] > reg[b]) reg[c] = 1;
            else reg[c] = 0;
        }

        public static void eqir(int a, int b, int c, int[] reg)
        {
            if (a == reg[b]) reg[c] = 1;
            else reg[c] = 0;
        }

        public static void eqri(int a, int b, int c, int[] reg)
        {
            if (reg[a] == b) reg[c] = 1;
            else reg[c] = 0;
        }

        public static void eqrr(int a, int b, int c, int[] reg)
        {
            if (reg[a] == reg[b]) reg[c] = 1;
            else reg[c] = 0;
        }
    }
}
