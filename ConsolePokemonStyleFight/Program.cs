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
                Console.Write($"Restart? {"Yes".Pastel(Color.Green)}/{"No".Pastel(Color.Red)} => ");
                
                entry = Console.ReadLine();
            } while (!entry.Contains("n"));
        }
    }
}