using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2018.Solutions
{
    public class Day24
    {


        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();

            var fighters = GetFighters();

            var numImmune = fighters.Count(x => x.Type == 0);
            var numInfection = fighters.Count(x => x.Type == 1);
            var boost = 1;

            fighters = GetFighters();
            foreach(var f in fighters)
            {
                if (f.Type == 0)
                    f.Attack += boost;
            }
            numImmune = fighters.Count(x => x.Type == 0);
            numInfection = fighters.Count(x => x.Type == 1);
            while (numImmune > 0 && numInfection > 0)
            {
                fighters.Sort();

                var chosen = new HashSet<int>();
                foreach (var f in fighters)
                {
                    var t = f.SelectTarget(fighters.Where(x => !chosen.Contains(x.Id)).ToList());
                    chosen.Add(t);
                }

                fighters = fighters.OrderBy(x => x.Initiative).Reverse().ToList();

                foreach (var f in fighters)
                {
                    if (f.TargetId == -1) continue;
                    var target = fighters.FirstOrDefault(x => x.Id == f.TargetId);
                    f.AttackTarget(target);
                }
                numImmune = fighters.Count(x => x.Type == 0 && x.IsAlive);
                numInfection = fighters.Count(x => x.Type == 1 && x.IsAlive);
            }
                
                

            int res = 0;
            if (numImmune > 0)
                res = fighters.Where(x => x.IsAlive && x.Type == 0).Sum(x => x.Units);
            else if (numInfection > 0)
                res = fighters.Where(x => x.IsAlive && x.Type == 1).Sum(x => x.Units);
                
            
            
            return $"";
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var fighters = GetFighters();

            var numImmune = fighters.Count(x => x.Type == 0);
            var numInfection = fighters.Count(x => x.Type == 1);
            var boost = 0;
            while (true)
            {
                fighters = GetFighters();
                foreach (var f in fighters)
                {
                    if (f.Type == 0)
                        f.Attack += boost;
                }
                var counter = 0;
                numImmune = fighters.Count(x => x.Type == 0);
                numInfection = fighters.Count(x => x.Type == 1);
                while (numImmune > 0 && numInfection > 0)
                {
                    fighters.Sort();

                    var chosen = new HashSet<int>();
                    foreach (var f in fighters)
                    {
                        var t = f.SelectTarget(fighters.Where(x => !chosen.Contains(x.Id)).ToList());
                        chosen.Add(t);
                    }

                    fighters = fighters.OrderBy(x => x.Initiative).Reverse().ToList();

                    foreach (var f in fighters)
                    {
                        if (f.TargetId == -1) continue;
                        var target = fighters.FirstOrDefault(x => x.Id == f.TargetId);
                        f.AttackTarget(target);
                    }

                    numImmune = fighters.Count(x => x.Type == 0 && x.IsAlive);
                    numInfection = fighters.Count(x => x.Type == 1 && x.IsAlive);

                    // Break point to prevent infinite loop (This can vary for different inputs)
                    if (counter > 5000) break;
                    counter++;

                }


                boost++;
                int res = 0;
                if (numImmune > 0)
                    res = fighters.Where(x => x.IsAlive && x.Type == 0).Sum(x => x.Units);
                else if (numInfection > 0)
                    res = fighters.Where(x => x.IsAlive && x.Type == 1).Sum(x => x.Units);
                if (numImmune > 0 && numInfection == 0)
                    return $"Immune {numImmune}, Infection {numInfection} Result {res}";
            }


            return $"";
        }

        public enum Damage
        {
            Cold,
            Fire,
            Radiation,
            Slashing,
            Bludgeoning
        }
        public class Fighter : IComparable<Fighter>
        {
            public int Units { get; set; }
            public int HitPoints { get; set; }
            public List<Damage> Weakness { get; set; }
            public List<Damage> ImmuneDamage { get; set; }
            public int Attack { get; set; }
            public int EffectivePower { get { return Attack * Units; } }
            public Damage AttackType { get; set; }
            public int Initiative { get; set; }
            public int Type { get; set; }

            private static int idCounter = 0;
            public int Id { get; }
            public int TargetId { get; set; }
            public bool IsAlive { get { return Units > 0; } }


            public Fighter(int units, int hitPoints, List<Damage> weakness, List<Damage> immuneDamage, int attack, Damage attackType, int initiative, int type)
            {
                Units = units;
                HitPoints = hitPoints;
                Weakness = weakness;
                ImmuneDamage = immuneDamage;
                Attack = attack;
                AttackType = attackType;
                Initiative = initiative;
                Type = type;
                Id = idCounter++;
            }

            public int SelectTarget(List<Fighter> fighters)
            {
                var enemies = fighters.Where(x => x.Type != Type && x.IsAlive);
                var max = 1;
                Fighter target = null;

                foreach(var e in enemies)
                {
                    var tDmg = EffectivePower;
                    if (e.Weakness.Contains(AttackType))
                        tDmg *= 2;
                    else if (e.ImmuneDamage.Contains(AttackType))
                        tDmg = 0;

                    if(tDmg > max)
                    {
                        max = tDmg;
                        target = e;
                    }
                    else if(tDmg == max)
                    {
                        if (e.EffectivePower > target.EffectivePower)
                            target = e;
                        else if(e.EffectivePower == target.EffectivePower)
                        {
                            if (e.Initiative > target.Initiative)
                                target = e;
                        }
                    }
                }
                if (target == null)
                    TargetId = -1;
                else
                    TargetId = target.Id;

                return TargetId;
            }

            public void AttackTarget(Fighter target)
            {
                if (!IsAlive) return;

                var tDmg = EffectivePower;
                if (target.Weakness.Contains(AttackType))
                    tDmg *= 2;
                else if (target.ImmuneDamage.Contains(AttackType))
                    tDmg = 0;

                var killedUnits = tDmg / target.HitPoints;
                if (killedUnits > target.Units)
                    killedUnits = target.Units;

                target.Units -= killedUnits;
            }

            public int CompareTo(Fighter other)
            {
                if (EffectivePower > other.EffectivePower)
                    return -1;
                if (EffectivePower < other.EffectivePower)
                    return 1;
                if (Initiative > other.Initiative)
                    return -1;
                if (Initiative < other.Initiative)
                    return 1;
                return 0;
            }
        }

        // Parsed the input by hand. 
        public static List<Fighter> GetFighters()
        {
            var fighters = new List<Fighter>();
            var weak = new List<Damage>();
            var strong = new List<Damage>();
            //Immune System:
            //2743 units each with 4149 hit points with an attack that does 13 radiation damage at initiative 14
            fighters.Add(new Fighter(2743, 4149, weak, strong, 13, Damage.Radiation, 14, 0));

            //8829 units each with 7036 hit points with an attack that does 7 fire damage at initiative 15
            fighters.Add(new Fighter(8829, 7036, weak, strong, 7, Damage.Fire, 15, 0));

            //1928 units each with 10700 hit points(weak to cold; immune to fire, radiation, slashing) with an attack that does 50 slashing damage at initiative 3
            weak = new List<Damage> { Damage.Cold };
            strong = new List<Damage> { Damage.Fire, Damage.Radiation, Damage.Slashing };
            fighters.Add(new Fighter(1928, 10700, weak, strong, 50, Damage.Slashing, 3, 0));

            //6051 units each with 11416 hit points with an attack that does 15 bludgeoning damage at initiative 20
            weak = new List<Damage>();
            strong = new List<Damage>();
            fighters.Add(new Fighter(6051, 11416, weak, strong, 15, Damage.Bludgeoning, 20, 0));

            //895 units each with 10235 hit points(immune to slashing; weak to bludgeoning) with an attack that does 92 bludgeoning damage at initiative 10
            weak = new List<Damage> { Damage.Bludgeoning };
            strong = new List<Damage> { Damage.Slashing };
            fighters.Add(new Fighter(895, 10235, weak, strong, 92, Damage.Bludgeoning, 10, 0));

            //333 units each with 1350 hit points with an attack that does 36 radiation damage at initiative 12
            weak = new List<Damage>();
            strong = new List<Damage>();
            fighters.Add(new Fighter(333, 1350, weak, strong, 36, Damage.Radiation, 12, 0));

            //2138 units each with 8834 hit points(weak to bludgeoning) with an attack that does 35 cold damage at initiative 11
            weak = new List<Damage> { Damage.Bludgeoning };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(2138, 8834, weak, strong, 35, Damage.Cold, 11, 0));

            //4325 units each with 1648 hit points(weak to cold, fire) with an attack that does 3 bludgeoning damage at initiative 8
            weak = new List<Damage> { Damage.Cold, Damage.Fire };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(4325, 1648, weak, strong, 3, Damage.Bludgeoning, 8, 0));

            //37 units each with 4133 hit points(immune to radiation, slashing) with an attack that does 1055 radiation damage at initiative 1
            weak = new List<Damage> { };
            strong = new List<Damage> { Damage.Slashing, Damage.Radiation };
            fighters.Add(new Fighter(37, 4133, weak, strong, 1055, Damage.Radiation, 1, 0));

            //106 units each with 3258 hit points(immune to slashing, radiation) with an attack that does 299 cold damage at initiative 13
            weak = new List<Damage> { };
            strong = new List<Damage> { Damage.Slashing, Damage.Radiation };
            fighters.Add(new Fighter(106, 3258, weak, strong, 299, Damage.Cold, 13, 0));

            //Infection:
            //262 units each with 8499 hit points(weak to cold) with an attack that does 45 cold damage at initiative 6
            weak = new List<Damage> { Damage.Cold };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(262, 8499, weak, strong, 45, Damage.Cold, 6, 1));

            //732 units each with 47014 hit points(weak to cold, bludgeoning) with an attack that does 127 bludgeoning damage at initiative 17
            weak = new List<Damage> { Damage.Cold, Damage.Bludgeoning };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(732, 47014, weak, strong, 127, Damage.Bludgeoning, 17, 1));

            //4765 units each with 64575 hit points with an attack that does 20 radiation damage at initiative 18
            weak = new List<Damage> { };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(4765, 64575, weak, strong, 20, Damage.Radiation, 18, 1));

            //3621 units each with 19547 hit points(immune to radiation, cold) with an attack that does 9 cold damage at initiative 5
            weak = new List<Damage> { };
            strong = new List<Damage> { Damage.Radiation, Damage.Cold };
            fighters.Add(new Fighter(3621, 19547, weak, strong, 9, Damage.Cold, 5, 1));

            //5913 units each with 42564 hit points(immune to radiation, bludgeoning, fire) with an attack that does 14 slashing damage at initiative 9
            weak = new List<Damage> { };
            strong = new List<Damage> { Damage.Radiation, Damage.Bludgeoning, Damage.Fire };
            fighters.Add(new Fighter(5913, 42564, weak, strong, 14, Damage.Slashing, 9, 1));

            //7301 units each with 51320 hit points(weak to radiation, fire; immune to bludgeoning) with an attack that does 11 fire damage at initiative 2
            weak = new List<Damage> { Damage.Radiation, Damage.Fire };
            strong = new List<Damage> { Damage.Bludgeoning };
            fighters.Add(new Fighter(7301, 51320, weak, strong, 11, Damage.Fire, 2, 1));

            //3094 units each with 23713 hit points(weak to slashing, fire) with an attack that does 14 radiation damage at initiative 19
            weak = new List<Damage> { Damage.Slashing, Damage.Fire };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(3094, 23713, weak, strong, 14, Damage.Radiation, 19, 1));

            //412 units each with 36593 hit points(weak to radiation, bludgeoning) with an attack that does 177 slashing damage at initiative 16
            weak = new List<Damage> { Damage.Radiation, Damage.Bludgeoning };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(412, 36593, weak, strong, 177, Damage.Slashing, 16, 1));

            //477 units each with 35404 hit points with an attack that does 146 cold damage at initiative 7
            weak = new List<Damage> { };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(477, 35404, weak, strong, 146, Damage.Cold, 7, 1));

            //332 units each with 11780 hit points(weak to fire) with an attack that does 70 slashing damage at initiative 4
            weak = new List<Damage> { Damage.Fire };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(332, 11780, weak, strong, 70, Damage.Slashing, 4, 1));

            return fighters;
        }

        public static List<Fighter> GetTest()
        {
            var fighters = new List<Fighter>();
            var weak = new List<Damage>();
            var strong = new List<Damage>();

            weak = new List<Damage> { Damage.Radiation, Damage.Bludgeoning };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(17, 5390, weak, strong, 4507, Damage.Fire, 2, 0));

            weak = new List<Damage> { Damage.Slashing, Damage.Bludgeoning };
            strong = new List<Damage> { Damage.Fire };
            fighters.Add(new Fighter(989, 1274, weak, strong, 25, Damage.Slashing, 3, 0));

            weak = new List<Damage> { Damage.Radiation };
            strong = new List<Damage> { };
            fighters.Add(new Fighter(801, 4706, weak, strong, 116, Damage.Bludgeoning, 1, 1));

            weak = new List<Damage> { Damage.Fire, Damage.Cold };
            strong = new List<Damage> { Damage.Radiation };
            fighters.Add(new Fighter(4485, 2961, weak, strong, 12, Damage.Slashing, 4, 1));
            return fighters;
        }
    }
}
