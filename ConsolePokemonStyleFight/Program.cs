using System;
using System.Drawing;
using ConsolePokemonStyleFight.Entities;
using Pastel;

namespace ConsolePokemonStyleFight
{
    class Program
    {
        static void Main()
        {
            var entry = "";
            do
            {
                var game = new Game();

                Console.ResetColor();
                Console.WriteLine();
                Console.Write(new String(' ', 30) + "Restart? ");
                Game.AskYesNo();

                entry = Console.ReadLine();
            } while (entry != null && !entry.Contains("n"));
        }
    }
}