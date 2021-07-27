using System;
using System.Drawing;
using Pastel;

namespace ConsolePokemonStyleFight.Entities
{
    public class Stat
    {
        public enum StatName
        {
            Health,
            Strength,
            Weight,
            Defense,
            Dodge,
            RadioActivity,
            Psychology,
            Respect,
            Money,
        }

        public StatName Name;

        public int Value;
        public int InitialValue;
        public int MaxValue;
        public bool GoodBehaviorIsDown;

        public Stat(StatName name, bool goodBehaviorIsDown = false)
        {
            Name = name;
            GoodBehaviorIsDown = goodBehaviorIsDown;
            Reset(5);
        }

        public void Reset()
        {
            Value = InitialValue;
        }

        public void Reset(int value)
        {
            MaxValue = value;
            InitialValue = GoodBehaviorIsDown ? 0 : MaxValue;
            Value = InitialValue;
        }

        public void Upgrade()
        {
            MaxValue ++;
            InitialValue = GoodBehaviorIsDown ? 0 : MaxValue;
            if (!GoodBehaviorIsDown) Value++;
        }

        public int ToPercent()
        {
            return (int)ToFactor() * 100;
        }

        public float ToFactor()
        {
            return (float)Value / MaxValue;
        }

        public string ToBar()
        {
            return (
                new String('▰', Math.Max(0, Value)) + 
                new String('▱', Math.Min(MaxValue, Math.Max(0, MaxValue - Value)))
            );
        }

        public Color GetBarColor()
        {
            Color color;

            var percent = ToPercent();
            
            if (GoodBehaviorIsDown)
            {
                if(percent > 80) color = Color.Red;
                else if(percent > 50) color = Color.Yellow;
                else color = Color.Green;
            }
            else
            {
                if(percent < 20) color = Color.Red;
                else if(percent < 50) color = Color.Yellow;
                else color = Color.Green;
            }

            return color;
        }

        public new string ToString()
        {
            var color = GetBarColor();
            return $"[ {ToBar().Pastel(color)} ] ( {Value.ToString().Pastel(color)} / {MaxValue} ) {Name}";
        }
    }
}