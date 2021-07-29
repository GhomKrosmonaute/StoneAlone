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

            Console.Clear();
            Console.WriteLine(
                (
                    "\n\n" +
                    "  ____  ____  __   __ _  ____     __   __     __   __ _  ____ \n" +
                    " / ___)(_  _)/  \\ (  ( \\(  __)   / _\\ (  )   /  \\ (  ( \\(  __)\n" +
                    " \\___ \\  )( (  O )/    / ) _)   /    \\/ (_/\\(  O )/    / ) _) \n" +
                    " (____/ (__) \\__/ \\_)__)(____)  \\_/\\_/\\____/ \\__/ \\_)__)(____)"
                ).Pastel(Color.LawnGreen) +
              "\n\n" +
              "     ┌───────────────────────────────────────────────────┐\n" +
              "     │                                                   │\n" +
              "     │  Hello!                                           │\n" +
              "     │                                                   │\n" +
              "     │ You are the leader of a post apocalyptic colony.  │\n" +
              "     │ Survive long enough for reinforcements to arrive. │\n" +
              "     │ Enjoy!                                            │\n" +
              "     │                                           Ghom    │\n" +
              "     └───────────────────────────────────────────────────┘\n"
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
                Console.WriteLine(
                    "\n\n" +
                    "     ┌──────────────────────────────────────────────────────────────────────────┐\n" +
                    "     │                                                                          │\n" +
                    $"     │  {"You lose...".Pastel(Color.Red)}                                                             │\n" +
                    "     │                                                                          │\n" +
                    "     │ The following statistics have a too critical rate,                       │\n" +
                    $"     │ {Data.ColoredColonyName()} is on the verge of collapse.{new String(' ', Math.Max(0, 44 - Data.ColonyName.Length))}│\n" +
                    "     │ Disappointing.                                                           │\n" +
                    "     │                                                                  Ghom    │\n" +
                    "     └──────────────────────────────────────────────────────────────────────────┘\n"
                );
                foreach (var statName in LostCause)
                {
                    Console.WriteLine("     " + player.GetStat(statName).ToString());
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(
                    "\n" +
                    "          _.-^^---....,,--\n"+
                    "      _--                  --_\n" +
                    "     <                        >)\n" +
                    "     |                         |\n" +
                    "      \\._                   _./\n" +
                    "         ```--. . , ; .--'''\n" +
                    "               | |   |\n" +
                    "            .-=||  | |=-.\n" +
                    "            `-=#$%&%$#=-'\n" +
                    "               | ;  :|\n" +
                    "      _____.,-#%&$@%#&#~,._____"
                );
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
            Console.WriteLine($"[ {Data.ColoredColonyName()} ] [ Progress {ProgressBar()} ]");
            Console.WriteLine(player.ToString());
            Console.Write($"\n[ {"Yes".Pastel(Color.Green)}/{"No".Pastel(Color.Red)} ] => ");
        }
    }
}