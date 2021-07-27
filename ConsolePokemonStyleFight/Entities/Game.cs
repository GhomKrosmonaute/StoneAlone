using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsolePokemonStyleFight.Entities
{
    public class Game
    {
        public Player player = new(10);

        private List<PNJ> PNJList;
        
        public int MaxLevel = 30;
        public int CurrentPNJIndex = 0;
        
        public bool Exited;
        public bool GameWon;
        public bool GameLost;

        public Game()
        {
            PNJList = new List<PNJ>
            {
                new ("(‡▼益▼)", 1),
                new ("((╬◣﹏◢))", 2),
                new ("ψ(▼へ▼メ)～→", 3),
                new ("(凸ಠ益ಠ)凸", 4),
                new ("(‡▼益▼)", 5),
                new ("((╬◣﹏◢))", 6),
                new ("ψ(▼へ▼メ)～→", 7),
                new ("(凸ಠ益ಠ)凸", 8),
                new ("(‡▼益▼)", 9),
                new ("((╬◣﹏◢))", 10),
                new ("ψ(▼へ▼メ)～→", 12),
                new ("(凸ಠ益ಠ)凸", 15),
                new ("୧((#Φ益Φ#))୨", 20) // Boss
            };
            
            Console.OutputEncoding = Encoding.UTF8;
            
            while (!Exited && !GameWon && !GameLost)
            {
                DisplayGame();
                
                var entry = GetEntry();
                
                CurrentPNJ().ApplyResponse(entry, this);

                GameLost = player.GetStat(Stat.StatName.Health).Value <= 0;
                GameWon = PNJList.All(pnj => pnj.Passed);
                Exited = entry == "exit";
            }
            
            Console.Clear();

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose...");
            }
        }

        string GetEntry()
        {
            return Console.ReadLine() ?? "";
        }

        PNJ CurrentPNJ()
        {
            return PNJList.ElementAt(CurrentPNJIndex);
        }

        void DisplayGame()
        {
            Console.Clear();
            Console.WriteLine($"[ Lvl. {player.Level} ]");
            foreach (Stat stat in player.Stats)
                Console.WriteLine(stat.ToString());
            Console.WriteLine(CurrentPNJ().ToString());
            Console.WriteLine("Yes / No");
        }
    }
}