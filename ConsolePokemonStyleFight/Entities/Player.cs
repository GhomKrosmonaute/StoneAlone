using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pastel;

namespace ConsolePokemonStyleFight.Entities
{
    public class Player
    {
        public int Level;
        public int Experience;
        public List<Stat> Stats;
        public List<string> Flags;
        public bool IsInCoupleWithTheGhoul;
        public bool IsNuclearManProposed;

        private ProgressBar Bar;

        public Player(int level)
        {
            Level = level;
            Flags = new List<string>();
            Stats = new List<Stat>
            {
                new (Data.StatName.Defense),
                new (Data.StatName.Dodge),
                new (Data.StatName.Health),
                new (Data.StatName.Strength),
                new (Data.StatName.Weight, true),
                new (Data.StatName.RadioActivity, true),
                new (Data.StatName.Psychology),
                new (Data.StatName.Respect),
                new (Data.StatName.Money)
            };
            
            var random = new Random();

            while (level > 1)
            {
                level--;
                
                var stat = Stats.ElementAt(random.Next(Stats.Count));

                stat.Reset(stat.MaxValue + 1);
            }
            
            Bar = new ProgressBar(GetNeededExperience());
        }

        public Stat GetStat(Data.StatName name)
        {
            return Stats.Find(stat => stat.Name == name);
        }

        /**
         * @returns true is stat reach dangerous critical deadline
         */
        public bool UpdateStat(Data.StatName name, int value)
        {
            return Stats[Stats.FindIndex(stat => stat.Name == name)].Update(value);
        }

        public int GetNeededExperience()
        {
            return Level * 10 / 3;
        }

        private void LevelUp()
        {
            Experience -= GetNeededExperience();
            
            Level++;

            foreach (var stat in Stats)
                stat.Upgrade();

            var randomStat = Stats.ElementAt(new Random().Next(Stats.Count));

            randomStat.Upgrade();
        }

        public void GainExperience(int amount)
        {
            Experience += amount;

            while (Experience >= GetNeededExperience())
            {
                LevelUp();
            }
        }
        
        public string ExperienceBar()
        {
            Bar.Update(Experience, GetNeededExperience());
            return Bar.ToString();
        }

        public new string ToString()
        {
            string flags = Flags.Count > 0 
                ? string.Join(", ", Flags.ConvertAll(flag => flag.Pastel(Color.HotPink))) 
                : "nothing".Pastel(Color.Gray);
            
            return (
                $"[ Lvl. {Level.ToString().Pastel(Color.Orange)} ] " +
                $"[ Exp. {Experience.ToString().Pastel(Color.Orange)}/{GetNeededExperience()} {ExperienceBar()} ]\n" +
                $"[ Flags: {flags} ]"
            );
        }
    }
}