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
        public Player player = new(1);
        
        public int CurrentPNJIndex = 0;
        
        private bool Exited;
        private bool GameWon;
        private bool GameLost;

        private ProgressBar Bar;

        private List<Data.StatName> LostCause;

        public Game()
        {
            Bar = new ProgressBar(Data.PNJList.Count);
            
            Console.OutputEncoding = Encoding.UTF8;
            
            LostCause = new List<Data.StatName>();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n" +
                              "     [                                                   ]\n" +
                              "     [  Hello!                                           ]\n" +
                              "     [                                                   ]\n" +
                              "     [ You are the leader of a post-apocalyptic colony,  ]\n" +
                              "     [ survive long enough for reinforcements to arrive. ]\n" +
                              "     [ Enjoy!                                            ]\n" +
                              "     [                                           𝑮𝒉𝒐𝒎    ]\n" +
                              "     [                                                   ]\n"
            );
            Console.ResetColor();
            Console.Write("Press any key... ");
            Console.ReadKey();
            Console.Clear();
            
            while (!Exited && !GameWon && !GameLost)
            {
                Display();
                
                var entry = GetEntry();
                
                CurrentPNJ().ApplyResponse(entry, this);

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose...");
                Console.ResetColor();
                Console.WriteLine($"The following statistics have a too critical rate, {Data.LandName} is on the verge of collapse.");
                foreach (var statName in LostCause)
                {
                    Console.WriteLine(player.GetStat(statName).ToString());
                }
            }
        }

        string GetEntry()
        {
            return Console.ReadLine() ?? "";
        }

        PNJ CurrentPNJ()
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
            return Bar.Update(
                Data.PNJList.FindAll(pnj => pnj.Passed).Count,
                Data.PNJList.Count
            );
        }

        void Display()
        {
            var pnj = CurrentPNJ();
            Console.Clear();
            Console.WriteLine();
            foreach (Stat stat in player.Stats)
                Console.WriteLine(
                    stat.ToString() + " " + (pnj.CurrentAction().Has(stat.Name) ? new String('•', pnj.StatUpdatingAmount()) : "")
                );
            Console.WriteLine(pnj.ToString());
            Console.WriteLine($"[ {Data.LandName} ]");
            Console.WriteLine($"[ Progress {ProgressBar()} ]");
            Console.WriteLine(player.ToString());
            Console.Write($"[ {"Yes".Pastel(Color.Green)}/{"No".Pastel(Color.Red)} ] => ");
        }
    }
}