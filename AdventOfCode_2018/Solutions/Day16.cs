using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day16
    {
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

                var output = addr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = addi(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = mulr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = muli(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = banr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = bani(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = borr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = bori(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = setr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = seti(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = gtir(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = gtri(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = gtrr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = eqir(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = eqri(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;

                output = eqrr(a, b, c, start);
                if (expected.SequenceEqual(output)) same++;


                if (same >= 3)
                {
                    counter++;
                }

            }


            return $"{counter}";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var counter = 0;
            var opCodes = new Dictionary<int, bool[]>();

            for(int i = 0; i < 16; i++)
            {
                opCodes[i] = new bool[16];
                for (int j = 0; j < 16; j++)
                {
                    opCodes[i][j] = true;
                }
            }

            for (var i = 0; i < rows.Count; i++)
            {
                // Read starting registers
                var temp = rows[i].Split('[');
                if (temp.Length == 1)
                {
                    var result = readInstructions(i + 2, rows);
                    return $"{result}";
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

                var same = 0;

                var output = addr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][0] = false;

                output = addi(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][1] = false;

                output = mulr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][2] = false;

                output = muli(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][3] = false;

                output = banr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][4] = false;

                output = bani(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][5] = false;

                output = borr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][6] = false;

                output = bori(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][7] = false;

                output = setr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][8] = false;

                output = seti(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][9] = false;

                output = gtir(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][10] = false;

                output = gtri(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][11] = false;

                output = gtrr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][12] = false;

                output = eqir(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][13] = false;

                output = eqri(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][14] = false;

                output = eqrr(a, b, c, start);
                if (!expected.SequenceEqual(output)) opCodes[opCode][15] = false;


                if (same >= 3)
                {
                    counter++;
                }

            }


            return $"{counter}";
        }


        public static int readInstructions(int start, List<string> rows)
        {
            var registers = new int[4];
            for(int i = start; i < rows.Count; i++)
            {
                var temp = rows[i].Split(' ');
                var opCode = int.Parse(temp[0]);
                var a = int.Parse(temp[1]);
                var b = int.Parse(temp[2]);
                var c = int.Parse(temp[3]);

                switch (opCode)
                {
                    case 0:
                        registers = mulr(a, b, c, registers);
                        break;
                    case 1:
                        registers = eqri(a, b, c, registers);
                        break;
                    case 2:
                        registers = setr(a, b, c, registers);
                        break;
                    case 3:
                        registers = eqrr(a, b, c, registers);
                        break;
                    case 4:
                        registers = gtrr(a, b, c, registers);
                        break;
                    case 5:
                        registers = muli(a, b, c, registers);
                        break;
                    case 6:
                        registers = borr(a, b, c, registers);
                        break;
                    case 7:
                        registers = bani(a, b, c, registers);
                        break;
                    case 8:
                        registers = addr(a, b, c, registers);
                        break;
                    case 9:
                        registers = banr(a, b, c, registers);
                        break;
                    case 10:
                        registers = eqir(a, b, c, registers);
                        break;
                    case 11:
                        registers = gtir(a, b, c, registers);
                        break;
                    case 12:
                        registers = addi(a, b, c, registers);
                        break;
                    case 13:
                        registers = gtri(a, b, c, registers);
                        break;
                    case 14:
                        registers = seti(a, b, c, registers);
                        break;
                    case 15:
                        registers = bori(a, b, c, registers);
                        break;
                }
            }
            return registers[0];
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
