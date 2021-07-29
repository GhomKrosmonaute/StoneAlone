using System;
using System.Drawing;
using Pastel;

namespace ConsolePokemonStyleFight.Entities
{
    public class Stat
    {
        public Data.StatName Name;

        public int Value;
        public int InitialValue;
        public int MaxValue;
        public bool GoodBehaviorIsDown;

        public ProgressBar Bar;

        public Stat(Data.StatName name, bool goodBehaviorIsDown = false)
        {
            Name = name;
            GoodBehaviorIsDown = goodBehaviorIsDown;
            Bar = new ProgressBar(5);
            Reset(5);
        }

        private string UpdateBar()
        {
            Bar.Update(Value, MaxValue);

            return ToString();
        }

        public void Reset()
        {
            Value = InitialValue;
            UpdateBar();
        }

        public void Reset(int value)
        {
            MaxValue = value;
            InitialValue = GoodBehaviorIsDown ? 0 : MaxValue;
            Value = InitialValue;
            UpdateBar();
        }

        public void Upgrade()
        {
            MaxValue ++;
            InitialValue = GoodBehaviorIsDown ? 0 : MaxValue;
            if (!GoodBehaviorIsDown) Value++;
            UpdateBar();
        }

        public bool Update(int amount)
        {
            Value += amount;
            if (Value <= 0)
            {
                Value = 0;
                if (!GoodBehaviorIsDown)
                {
                    UpdateBar();
                    return false;
                }
            }
            if (Value > MaxValue)
            {
                Value = MaxValue;
                if (GoodBehaviorIsDown)
                {
                    UpdateBar();
                    return false;
                }
            }
            UpdateBar();
            return true;
        }

        public int ToPercent()
        {
            return (Value * 100) / MaxValue;
        }

        public float ToFactor()
        {
            return (float)Value / MaxValue;
        }
        
        public bool Test()
        {
            return GoodBehaviorIsDown 
                ? new Random().Next(100) > ToPercent() 
                : new Random().Next(100) < ToPercent();
        }

        public Color GetBarColor()
        {
            Color color;

            var percent = ToPercent();

            if (GoodBehaviorIsDown) percent = 100 - percent;
            
            if(percent < 20) color = Color.Red;
            else if(percent < 50) color = Color.Yellow;
            else color = Color.Green;

            return color;
        }

        public new string ToString()
        {
            var color = GetBarColor();
            return $"[ {Bar.ToString().Pastel(color)} ] ( {Value.ToString().Pastel(color)}/{MaxValue} ) {(ToPercent() + "%").Pastel(color)} {Name}";
        }
    }
}