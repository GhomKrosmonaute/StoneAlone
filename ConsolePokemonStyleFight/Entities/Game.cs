using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Pastel;

namespace ConsolePokemonStyleFight.Entities
{
    public class Game
    {
        public static void AskYesNo()
        {
            Console.Write($"{"Yes".Pastel(Color.Green)}/{"No".Pastel(Color.Red)} => ");
        }
        
        public Player player = new(1);
        
        public int CurrentPNJIndex = 0;
        
        private bool Exited;
        private bool GameWon;
        private bool GameLost;

        private ProgressBar Bar;

        private List<Data.StatName> LostCause;

        public Game()
        {
            Data.Reset();
            
            Bar = new ProgressBar(Data.PNJList.Count);
            
            Console.OutputEncoding = Encoding.UTF8;
            
            LostCause = new List<Data.StatName>();

            Console.Clear();
            Console.WriteLine(Data.TitleScreen);
            Console.ResetColor();
            Console.Write("Press any key... ");
            Console.ReadKey();
            Console.Clear();
            
            while (!Exited && !GameWon && !GameLost)
            {
                Display();
                
                var entry = GetEntry();
                
                CurrentPNJ().ApplyResponse(entry, this);
                
                player.GainExperience(1);

                GameWon = Data.PNJList.All(pnj => pnj.Passed);
                Exited = entry == "exit";
            }

            if (Exited)
            {
                Console.WriteLine("Thanks for playing!");
            }
            else if (GameWon)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulation, last level done!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine(Data.LoseMail());
                foreach (var statName in LostCause)
                {
                    Console.WriteLine("     " + player.GetStat(statName).ToString());
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(Data.NukeBomb);
            }
        }

        string GetEntry()
        {
            return Console.ReadLine() ?? "";
        }

        public PNJ CurrentPNJ()
        {
            return Data.PNJList.ElementAt(CurrentPNJIndex);
        }

        public void Over(List<Data.StatName> cause)
        {
            LostCause = cause;
            GameLost = true;
        }
        
        public string ProgressBar()
        {
            Bar.Update(Data.PNJList.FindAll(pnj => pnj.Passed).Count, Data.PNJList.Count);
            return $"{Bar.ToString()} {Bar.ToPercent().ToString().Pastel(Color.PaleVioletRed)}%";
        }

        void Display()
        {
            var pnj = CurrentPNJ();
            Console.Clear();
            Console.WriteLine();

            var maxStat = 0;

            foreach (var stat in player.Stats)
            {
                maxStat = Math.Max(maxStat, stat.MaxValue);
            }
            
            for (var i=0; i<player.Stats.Count; i++)
            {
                var stat = player.Stats[i];
                var color = stat.GetBarColor();
                var bar = stat.Bar.ToString();
                if (i % 2 == 0)
                {
                    Console.Write(
                        SizedString.Constrain(
                            $"{(stat.ToPercent() + "%").Pastel(color)} " +
                            $"{stat.Value.ToString().Pastel(color)}/{stat.MaxValue} " +
                            $"{SizedString.Constrain(SizedString.Reverse(bar), maxStat, SizedString.Alignment.Right).Pastel(color)} " +
                            $"{SizedString.Constrain($"{stat.Name}".Pastel(pnj.CurrentAction().Has(stat.Name) ? Color.White : Color.DarkGray), 11, SizedString.Alignment.Right)}", 
                            40, 
                            SizedString.Alignment.Right
                        ) + 
                        SizedString.Constrain(new String('•', pnj.CurrentAction().Has(stat.Name) ? pnj.StatUpdatingAmount() : 0), 5, SizedString.Alignment.Right) +
                        " │ "
                    );
                }
                else
                {
                    var dataLine = " | ";

                    switch (i)
                    {
                        case 1: 
                            dataLine += Data.ColoredColonyName();
                            break;
                        case 3:
                            dataLine += $"Progress {ProgressBar()} ";
                            break;
                        case 5:
                            dataLine += $"Lvl. {player.Level.ToString().Pastel(Color.Orange)} Exp. {player.Experience.ToString().Pastel(Color.Orange)}/{player.GetNeededExperience()} {player.ExperienceBar()}";
                            break;
                        case 7:
                            dataLine += $"Flags: {player.FlagChain()}";
                            break;
                    }
                    
                    Console.WriteLine(
                        SizedString.Constrain(new String('•', pnj.CurrentAction().Has(stat.Name) ? pnj.StatUpdatingAmount() : 0), 5, SizedString.Alignment.Left) +
                        SizedString.Constrain(
                            $"{SizedString.Constrain($"{stat.Name}".Pastel(pnj.CurrentAction().Has(stat.Name) ? Color.White : Color.DarkGray), 14, SizedString.Alignment.Left)} " +
                            $"{SizedString.Constrain(bar, maxStat, SizedString.Alignment.Left).Pastel(color)} " +
                            $"{stat.Value.ToString().Pastel(color)}/{stat.MaxValue} " +
                            $"{(stat.ToPercent() + "%").Pastel(color)}",
                            40,
                            SizedString.Alignment.Left
                        ) + dataLine
                    );
                }
            }
            Console.WriteLine("\n");
            
            // foreach (Stat stat in player.Stats)
            //     Console.WriteLine(
            //         stat.ToString() + " " + (pnj.CurrentAction().Has(stat.Name) ? new String('•', pnj.StatUpdatingAmount()) : "")
            //     );
            
            Console.WriteLine(pnj.ToString());
            
            Console.Write(new String(' ', 30));
            AskYesNo();
        }
    }
}