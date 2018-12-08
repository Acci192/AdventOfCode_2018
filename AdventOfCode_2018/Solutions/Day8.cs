using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day8
    {
        

        // This was my first solution for Part A
        public static string A(string input)
        {
            var inp = input.Split(' ').Select(x => int.Parse(x)).ToList();
            var sum = 0;

            for(int i = 0; i < inp.Count(); i++)
            {
                if (inp[i] != 0) continue;

                var numMeta = inp[i + 1];
                for(int j = 0; j < numMeta; j++)
                {
                    sum += inp[i + 2];
                    inp.RemoveAt(i + 2);
                }
                if (i - 2 < 0) continue;

                inp[i - 2] -= 1;
                inp.RemoveAt(i + 1);
                inp.RemoveAt(i);
                i -= 3;
            }
            return $"{sum}";
        }

        // This is my second solution for Part A after solving Part B
        public static string ARecursive(string input)
        {
            var inp = input.Split(' ').Select(x => int.Parse(x)).ToList();
            return $"{GetMetadataA(inp, 0).Item2}";
        }

        public static string B(string input)
        {
            var inp = input.Split(' ').Select(x => int.Parse(x)).ToList();
            return $"{GetMetadataB(inp, 0).Item2}";
        }

        private static Tuple<List<int>, int> GetMetadataA(List<int> tree, int index)
        {
            var sum = 0;
            var isParent = tree[index] != 0;
            while (tree[index] != 0)
            {
                var res = GetMetadataA(tree, index + 2);
                tree = res.Item1;
                sum += res.Item2;
            }

            var num = tree[index + 1];
            for (int j = 0; j < num; j++)
            {
                sum += tree[index + 2];

                tree.RemoveAt(index + 2);
            }
            if (index == 0) return new Tuple<List<int>, int>(tree, sum);
            tree[index - 2] -= 1;
            tree.RemoveAt(index + 1);
            tree.RemoveAt(index);

            return new Tuple<List<int>, int>(tree, sum);
        }

        private static Tuple<List<int>, int> GetMetadataB(List<int> tree, int index)
        {
            var sums = new List<int>();
            var isParent = tree[index] != 0;
            while(tree[index] != 0)
            {
                var res = GetMetadataB(tree, index + 2);
                tree = res.Item1;
                sums.Add(res.Item2);
            }
            if (!isParent)
            {
                var num = tree[index + 1];
                for (int j = 0; j < num; j++)
                {
                    sums.Add(tree[index + 2]);
                    tree.RemoveAt(index + 2);
                }

                tree[index - 2] -= 1;
                tree.RemoveAt(index + 1);
                tree.RemoveAt(index);
                
                return new Tuple<List<int>, int>(tree, sums.Sum());
            }
            else
            {
                var summ = 0;
                var num = tree[index + 1];
                for (int j = 0; j < num; j++)
                {
                    var tempIndex = tree[index + 2] - 1;
                    if(tempIndex < sums.Count())
                    {
                        summ += sums[tempIndex];
                    }
                    
                    tree.RemoveAt(index + 2);
                }

                if(index == 0) return new Tuple<List<int>, int>(tree, summ);
                tree[index - 2] -= 1;
                tree.RemoveAt(index + 1);
                tree.RemoveAt(index);
                return new Tuple<List<int>, int>(tree, summ);
            }
        } 
    }
}
