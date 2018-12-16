using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day16
    {
        private static List<Func<int, int, int, int[], int[]>> Operations = 
            new List<Func<int, int, int, int[], int[]>> {
                addr,
                addi,
                mulr,
                muli,
                banr,
                bani,
                borr,
                bori,
                setr,
                seti,
                gtir,
                gtri,
                gtrr,
                eqir,
                eqri,
                eqrr
            };

        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var counter = 0;


            for(var i = 0; i < rows.Count; i++)
            {
                // Read starting registers
                var temp = rows[i].Split('[');
                if (temp.Length == 1) break;
                var start = new int[4];
                temp = temp[1].Split(',');
                start[0] = int.Parse(temp[0]);
                start[1] = int.Parse(temp[1]);
                start[2] = int.Parse(temp[2]);
                start[3] = int.Parse(temp[3].TrimEnd(']'));

                // Read instruction
                i++;
                temp = rows[i].Split(' ');
                var opCode = int.Parse(temp[0]);
                var a = int.Parse(temp[1]);
                var b = int.Parse(temp[2]);
                var c = int.Parse(temp[3]);

                // Read expected registers
                i++;
                temp = rows[i].Split('[');
                if (temp.Length == 1) break;
                var expected = new int[4];
                temp = temp[1].Split(',');
                expected[0] = int.Parse(temp[0]);
                expected[1] = int.Parse(temp[1]);
                expected[2] = int.Parse(temp[2]);
                expected[3] = int.Parse(temp[3].TrimEnd(']'));
                i++;

                var same = 0;

                foreach(var o in Operations)
                {
                    if (expected.SequenceEqual(o(a,b,c,start))) same++;
                }

                if (same >= 3) counter++;
            }

            return $"{counter}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var validOpcodes = new Dictionary<int, bool[]>();
            var opCodes = new Dictionary<int, int>();
            var remainingOp = new HashSet<int>();

            var registers = new int[4];
            var startOfInstructions = 0;

            for (int i = 0; i < 16; i++)
            {
                validOpcodes[i] = new bool[16];
                opCodes[i] = -1;
                remainingOp.Add(i);
                for (int j = 0; j < 16; j++)
                {
                    validOpcodes[i][j] = true;
                }
            }

            for (var i = 0; i < rows.Count; i++)
            {
                // Read starting registers
                var temp = rows[i].Split('[');
                if (temp.Length == 1)
                {
                    startOfInstructions = i + 2;
                    break;
                }
                var start = new int[4];
                temp = temp[1].Split(',');
                start[0] = int.Parse(temp[0]);
                start[1] = int.Parse(temp[1]);
                start[2] = int.Parse(temp[2]);
                start[3] = int.Parse(temp[3].TrimEnd(']'));

                // Read instruction
                i++;
                temp = rows[i].Split(' ');
                var opCode = int.Parse(temp[0]);
                var a = int.Parse(temp[1]);
                var b = int.Parse(temp[2]);
                var c = int.Parse(temp[3]);

                // Read expected registers
                i++;
                temp = rows[i].Split('[');
                if (temp.Length == 1) break;
                var expected = new int[4];
                temp = temp[1].Split(',');
                expected[0] = int.Parse(temp[0]);
                expected[1] = int.Parse(temp[1]);
                expected[2] = int.Parse(temp[2]);
                expected[3] = int.Parse(temp[3].TrimEnd(']'));
                i++;

                for(int j = 0; j < opCodes.Count; j++)
                {
                    if (!expected.SequenceEqual(Operations[j](a, b, c, start))) validOpcodes[opCode][j] = false;
                }
            }

            // Find the correct instruction for each Opcode
            while (remainingOp.Count > 0)
            {
                var toRemove = new List<int>();
                foreach(var o in remainingOp)
                {
                    foreach(var v in validOpcodes)
                    {
                        if(v.Value[o] && v.Value.Count(x => x) == 1)
                        {
                            opCodes[v.Key] = o;
                            toRemove.Add(o);
                        }
                    }
                }
                foreach(var i in toRemove)
                {
                    foreach (var vos in validOpcodes)
                    {
                        vos.Value[i] = false;
                    }
                    remainingOp.Remove(i);
                }
            }

            // Execute the instructions from intput
            for (int i = startOfInstructions; i < rows.Count; i++)
            {
                var temp = rows[i].Split(' ');
                var opCode = int.Parse(temp[0]);
                var a = int.Parse(temp[1]);
                var b = int.Parse(temp[2]);
                var c = int.Parse(temp[3]);

                registers = Operations[opCodes[opCode]](a, b, c, registers);
            }
            return $"{registers[0]}";
        }

        public static int[] addr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] + reg[b];
            return output;
        }

        public static int[] addi(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] + b;
            return output;
        }

        public static int[] mulr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] * reg[b];
            return output;
        }

        public static int[] muli(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] * b;
            return output;
        }

        public static int[] banr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] & reg[b];
            return output;
        }

        public static int[] bani(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] & b;
            return output;
        }

        public static int[] borr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] | reg[b];
            return output;
        }

        public static int[] bori(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a] | b;
            return output;
        }

        public static int[] setr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = reg[a];
            return output;
        }

        public static int[] seti(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            output[c] = a;
            return output;
        }
        public static int[] gtir(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            if (a > reg[b]) output[c] = 1;
            else output[c] = 0;
            return output;
        }
        public static int[] gtri(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            if (reg[a] > b) output[c] = 1;
            else output[c] = 0;
            return output;
        }

        public static int[] gtrr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            if (reg[a] > reg[b]) output[c] = 1;
            else output[c] = 0;
            return output;
        }

        public static int[] eqir(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            if (a == reg[b]) output[c] = 1;
            else output[c] = 0;
            return output;
        }

        public static int[] eqri(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            if (reg[a] == b) output[c] = 1;
            else output[c] = 0;
            return output;
        }

        public static int[] eqrr(int a, int b, int c, int[] reg)
        {
            var output = reg.ToArray();
            if (reg[a] == reg[b]) output[c] = 1;
            else output[c] = 0;
            return output;
        }
    }
}
