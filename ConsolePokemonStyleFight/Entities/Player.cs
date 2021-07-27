using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsolePokemonStyleFight.Entities
{
    public class Player
    {
        public int Level;
        public int Experience;
        public List<Stat> Stats;

        public Player(int level)
        {
            Level = level;
            Stats = new List<Stat>
            {
                new (Stat.StatName.Defense),
                new (Stat.StatName.Dodge),
                new (Stat.StatName.Health),
                new (Stat.StatName.Strength),
                new (Stat.StatName.Weight),
                new (Stat.StatName.RadioActivity),
                new (Stat.StatName.Psychology),
                new (Stat.StatName.Respect),
                new (Stat.StatName.Money)
            };
            
            var random = new Random();

            while (level > 0)
            {
                level--;
                
                var stat = Stats.ElementAt(random.Next(0, Stats.Count - 1));

                stat.Reset(stat.MaxValue + 1);
            }
        }

        public Stat GetStat(Stat.StatName name)
        {
            return Stats.Find(stat => stat.Name == name);
        }

        public void UpdateStat(Stat.StatName name, int value)
        {
            Stats[Stats.FindIndex(stat => stat.Name == name)].Value += value;
        }

        public int GetNeededExperience()
        {
            return Level * 10 / 3;
        }

        private void LevelUp()
        {
            Experience -= GetNeededExperience();
            
            Level++;
            
            var stat = Stats.ElementAt(new Random().Next(0, Stats.Count - 1));

            stat.Upgrade();
        }

        public void GainExperience(int amount)
        {
            Experience += amount;

            while (Experience >= GetNeededExperience())
            {
                LevelUp();
            }
        }
    }
}