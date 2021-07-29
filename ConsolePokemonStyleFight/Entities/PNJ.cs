using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pastel;

namespace ConsolePokemonStyleFight.Entities
{
    public class PNJ
    {
        public string Face;
        public int Level;
        public bool Passed;
        public int ActionIndex;
        public int AskCount;
        
        public PNJ(string face, int level)
        {
            Face = face;
            Level = level;
            AskCount = Math.Min(Level * 2, 5);
            
            ChangeAction(null);
        }

        public int StatUpdatingAmount()
        {
            return Level;
        }

        public void ApplyResponse(string entry, Game game)
        {
            var action = CurrentAction();
            var amount = StatUpdatingAmount();
            var yes = !entry.ToLower().Contains("n");
            List<Data.StatName> criticalStatNames = new List<Data.StatName>();

            if(action.Callback != null)
                action.Callback(game, yes);

            if (!yes)
            {
                if (action.NoUp != null) foreach (var statName in action.NoUp)
                    if (!game.player.UpdateStat(statName, amount))
                        criticalStatNames.Add(statName);
                
                if (action.NoDown != null) foreach (var statName in action.NoDown)
                    if (!game.player.UpdateStat(statName, -amount))
                        criticalStatNames.Add(statName);
            }
            else
            {
                if (action.YesUp != null) foreach (var statName in action.YesUp)
                    if (!game.player.UpdateStat(statName, amount))
                        criticalStatNames.Add(statName);
                
                if (action.YesDown != null) foreach (var statName in action.YesDown)
                    if (!game.player.UpdateStat(statName, -amount))
                        criticalStatNames.Add(statName);
            }

            if (criticalStatNames.Count > 0)
            {
                game.Over(criticalStatNames);
                return;
            }

            AskCount--;
            if (AskCount <= 0)
            {
                Passed = true;
                game.player.GainExperience(Level);
                game.CurrentPNJIndex++;
            }
            
            ChangeAction(game);
        }

        public Action CurrentAction()
        {
            return Data.Actions.ElementAt(ActionIndex);
        }

        public void ChangeAction(Game game)
        {
            var random = new Random();
            
            if (game == null)
            {
                var safeActions = Data.Actions.FindAll(action => action.Filter == null);
                
                ActionIndex = Data.Actions.IndexOf(safeActions[random.Next(safeActions.Count)]);
                
                return;
            }
            
            var lastAsk = CurrentAction().Ask;

            var forcedActions = Data.Actions.FindAll(action =>
            {
                if (action.Force == null) return false;
                if (action.Filter == null) return true;
                return action.Force(game) && action.Filter(game);
            });

            if (forcedActions.Count > 0)
                ActionIndex = Data.Actions.IndexOf(forcedActions[random.Next(forcedActions.Count)]);
            else 
                while (
                    CurrentAction().Ask == lastAsk || 
                    (
                        CurrentAction().Filter != null && 
                        !CurrentAction().Filter(game)
                    )
                )
                    ActionIndex = random.Next(Data.Actions.Count);
        }

        public new string ToString()
        {
            return (
                "\n" +
                $"     {"(".Pastel(Color.White)} {CurrentAction().Ask} {")".Pastel(Color.White)}\n" +
                $"         {"|/".Pastel(Color.White)}\n" +
                $"     {Face.Pastel(Color.CadetBlue)}\n"
            );
        }
    }
}