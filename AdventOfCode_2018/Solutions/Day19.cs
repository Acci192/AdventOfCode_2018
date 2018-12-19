using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day19
    {
        private static Dictionary<string, Func<int, int, int, int[], int[]>> Operations =
            new Dictionary<string, Func<int, int, int, int[], int[]>> {
                {"addr", Day16.addr },
                {"addi", Day16.addi },
                {"mulr", Day16.mulr },
                {"muli", Day16.muli },
                {"banr", Day16.banr },
                {"bani", Day16.bani },
                {"borr", Day16.borr },
                {"bori", Day16.bori },
                {"setr", Day16.setr },
                {"seti", Day16.seti },
                {"gtir", Day16.gtir },
                {"gtri", Day16.gtri },
                {"gtrr", Day16.gtrr },
                {"eqir", Day16.eqir },
                {"eqri", Day16.eqri },
                {"eqrr", Day16.eqrr }
            };
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var ins = new List<Tuple<string, int[]>>();
            var pc = -1;
            foreach (var row in rows)
            {
                var temp = row.Split(' ');
                if(temp.Length == 2)
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
            while (registers[pc] >= 0 && registers[pc] < ins.Count)
            {
                var i = ins[registers[pc]];
                registers = Operations[i.Item1](i.Item2[0], i.Item2[1], i.Item2[2], registers);
                
                registers[pc]++;
            }

            return $"{registers[0]}";
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
            registers[0] = 1;
            var insCount = new Dictionary<int, int>();
            while (registers[pc] >= 0 && registers[pc] < ins.Count)
            {
                var i = ins[registers[pc]];

                if (!insCount.ContainsKey(registers[pc])) insCount[registers[pc]] = 1;
                else insCount[registers[pc]]++;

                if (insCount[registers[pc]] > 10) break;
                registers = Operations[i.Item1](i.Item2[0], i.Item2[1], i.Item2[2], registers);

                registers[pc]++;
            }

            var large = registers.Max();
            var res = 0;
            for(var i = 1; i <= large; i++)
            {
                if (large % i == 0) res += i;
            }

            return $"{res}";
        }
    }
}
