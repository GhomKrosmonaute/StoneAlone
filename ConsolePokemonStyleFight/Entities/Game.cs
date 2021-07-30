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
            Console.Write($"\n                              {"Yes".Pastel(Color.Green)}/{"No".Pastel(Color.Red)} =>");
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
                Console.WriteLine(Data.LoseMail);
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
                            $"{SizedString.Reverse(bar).Pastel(color)} " +
                            $"{SizedString.Constrain($"{stat.Name}", 11, SizedString.Alignment.Right)} │ ", 
                            50, 
                            SizedString.Alignment.Right
                        )
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
                    }
                    
                    Console.WriteLine(
                        SizedString.Constrain(
                            $"{SizedString.Constrain($"{stat.Name}", 14, SizedString.Alignment.Left)} " +
                            $"{bar.Pastel(color)} " +
                            $"{stat.Value.ToString().Pastel(color)}/{stat.MaxValue} " +
                            $"{(stat.ToPercent() + "%").Pastel(color)}",
                            50,
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
            Console.WriteLine(player.ToString());
            
            AskYesNo();
        }
    }
}