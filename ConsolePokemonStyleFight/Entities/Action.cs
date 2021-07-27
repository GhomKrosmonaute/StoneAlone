namespace ConsolePokemonStyleFight.Entities
{
    public class Action
    {
        public string Ask;
        public Stat.StatName[] YesUp;
        public Stat.StatName[] YesDown;
        public Stat.StatName[] NoUp;
        public Stat.StatName[] NoDown;
        
        public Action(string ask, 
            Stat.StatName[] yesUp, 
            Stat.StatName[] yesDown, 
            Stat.StatName[] noUp, 
            Stat.StatName[] noDown)
        {
            Ask = ask;
            YesUp = yesUp;
            YesDown = yesDown;
            NoUp = noUp;
            NoDown = noDown;
        }
    }
}