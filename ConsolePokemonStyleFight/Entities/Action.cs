using System.Linq;

namespace ConsolePokemonStyleFight.Entities
{
    public class Action
    {
        public delegate void callback(Game game, bool yes);
        public delegate bool filter(Game game);

        public delegate bool force(Game game);

        public string Ask;
        public Data.StatName[] YesUp;
        public Data.StatName[] YesDown;
        public Data.StatName[] NoUp;
        public Data.StatName[] NoDown;
        public force Force;
        public filter Filter;
        public callback Callback;

        public Action(string ask, 
            Data.StatName[] yesUp, 
            Data.StatName[] yesDown, 
            Data.StatName[] noUp, 
            Data.StatName[] noDown,
            force force,
            filter filter,
            callback callback)
        {
            Ask = ask;
            YesUp = yesUp;
            YesDown = yesDown;
            NoUp = noUp;
            NoDown = noDown;
            Force = force;
            Filter = filter;
            Callback = callback;
        }
        
        public bool Has(Data.StatName statName)
        {
            return
                (YesUp != null && YesUp.Contains(statName)) ||
                (YesDown != null && YesDown.Contains(statName)) || 
                (NoDown != null && NoDown.Contains(statName)) ||
                (NoUp != null && NoUp.Contains(statName));
        }
    }
}