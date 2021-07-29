using System;

namespace ConsolePokemonStyleFight.Entities
{
    public class ProgressBar
    {
        private int Value;
        private int Max;
        private char Full;
        private char Void;
        
        public ProgressBar(int max, char Full = '▰', char Void = '▱')
        {
            Max = max;
            this.Full = Full;
            this.Void = Void;
        }

        public string Update(int value, int? max)
        {
            Value = value;
            Max = max ?? Max;
            return ToString();
        }

        public new string ToString()
        {
            return (
                new String(Full, Math.Max(0, Value)) + 
                new String(Void, Math.Min(Max, Math.Max(0, Max - Value)))
            );
        }
    }
}