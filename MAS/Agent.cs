using MAS;
using MAS.Items;
using System;

namespace MAS
{
    public abstract class Agent
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        private ConsoleColor _consoleColor;
        private static Random _random = new Random();

        public Agent(string name, int cash)
        {
            Name = name;
            Cash = cash;
            _consoleColor = chooseConsoleColor();
        }
        public abstract void MakeBet(string message, Auction auction);
        public abstract bool DoJoin(IAuctionItem item);
        public void PrintToPersonalScreen(string message)
        {
            Console.ForegroundColor = _consoleColor;
            System.Console.WriteLine(Name);
            System.Console.WriteLine(message);
            Console.ResetColor();
        }
        private ConsoleColor chooseConsoleColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length));
        }
    }
}
