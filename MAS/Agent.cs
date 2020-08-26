using MAS;
using MAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS
{
    public abstract class Agent
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        private ConsoleColor _consoleColor;
        private static Random _random = new Random();
        private List<IAuctionItem> _ownedProducts;
        public Agent(string name, int cash)
        {
            Name = name;
            Cash = cash;
            _consoleColor = chooseConsoleColor();
            _ownedProducts = new List<IAuctionItem>();
        }
        public abstract void MakeBet(string message, Auction auction);
        public abstract bool DoJoin(IAuctionItem item);
        public void PrintToPersonalScreen(string message)
        {
            Console.ForegroundColor = _consoleColor;
            System.Console.WriteLine($"{Name}: {message}");
            Console.ResetColor();
        }
        private ConsoleColor chooseConsoleColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length));
        }
        public void BuyProduct(IAuctionItem product, int productPrice)
        {
            Cash -= productPrice;
            _ownedProducts.Add(product);
            PrintToPersonalScreen($"You now have {Cash}$ left");
            PrintToPersonalScreen($"Owned prodcts: {String.Format(", ", _ownedProducts.Select(p => p.Name).ToList())}");
        }
    }
}
