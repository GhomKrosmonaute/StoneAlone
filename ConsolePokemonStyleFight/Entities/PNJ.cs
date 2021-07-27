using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pastel;

namespace ConsolePokemonStyleFight.Entities
{
    public class PNJ
    {
        private static readonly string LandName = "Rock Alone Land".Pastel(Color.DarkCyan);
        private static readonly List<Action> Actions = new ()
        {
            new (
                "Want a chocolate bar?", 
                new []{ Stat.StatName.Health, Stat.StatName.Weight },
                null, 
                new []{ Stat.StatName.Psychology },
                new []{ Stat.StatName.Weight }
            ),
            new (
                $"Should {LandName} use nuclear power?", 
                new []{ Stat.StatName.RadioActivity, Stat.StatName.Money },
                null,
                null,
                new []{ Stat.StatName.RadioActivity }
            ),
            new (
                "My child is sick, do not want my son to go to the mine, he will die there!",
                new [] { Stat.StatName.Respect },
                new [] { Stat.StatName.Money },
                new [] { Stat.StatName.Money, Stat.StatName.RadioActivity },
                new [] { Stat.StatName.Respect }
            )
        };
        
        public string Face;
        public int Level;
        public bool Passed;
        public int ActionIndex;
        public int AskCount;
        
        public PNJ(string face, int level)
        {
            Face = face;
            Level = level;
            AskCount = Math.Max(Level / 10, 1);

            ChangeAction();
        }

        public void ApplyResponse(string entry, Game game)
        {
            var action = CurrentAction();
            
            if (entry.ToLower().Contains("no"))
            {
                if(action.NoUp != null) foreach (var statName in action.NoUp)
                    game.player.UpdateStat(statName, Math.Max(Level / 10, 1));
                
                if(action.NoDown != null) foreach (var statName in action.NoDown)
                    game.player.UpdateStat(statName, -Math.Max(Level / 10, 1));
            }
            else
            {
                if(action.YesUp != null) foreach (var statName in action.YesUp)
                    game.player.UpdateStat(statName, Math.Max(Level / 10, 1));
                
                if(action.YesDown != null) foreach (var statName in action.YesDown)
                    game.player.UpdateStat(statName, -Math.Max(Level / 10, 1));
            }

            AskCount--;
            if (AskCount <= 0)
            {
                Passed = true;
                game.CurrentPNJIndex++;
            }
            
            ChangeAction();
        }

        public Action CurrentAction()
        {
            return Actions.ElementAt(ActionIndex);
        }

        public void ChangeAction()
        {
            var lastAsk = CurrentAction().Ask;
            var random = new Random();
            while (CurrentAction().Ask == lastAsk)
            {
                ActionIndex = random.Next(0, Actions.Count - 1);
            }
        }

        public new string ToString()
        {
            return $"[ {Face.Pastel(Color.Chocolate)} :: {CurrentAction().Ask} ]";
        }
    }
}