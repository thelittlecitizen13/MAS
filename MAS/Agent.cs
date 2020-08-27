using MAS;
using MAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MAS
{
    public abstract class Agent
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        private int _availableCash;
        private bool _isBetOnGoing;
        private ConsoleColor _consoleColor;
        private static Random _random = new Random();
        private List<IAuctionItem> _ownedProducts;
        private static object _MakeBetLocker = new object();
        public Agent(string name, int cash)
        {
            Name = name;
            Cash = cash;
            _availableCash = Cash;
            _consoleColor = chooseConsoleColor();
            _ownedProducts = new List<IAuctionItem>();
        }
        public void MakeBet(string message, AuctionRunner runner)   
        {
            ThreadPool.QueueUserWorkItem(obj =>
                {
                    Thread.CurrentThread.IsBackground = false;
                    int currentBet = runner.auction.CurrentBet.CurrentPrice;
                    int priceJump = runner.auction.CurrentBet.MinimunPriceJump;
                    if (DoJoin(runner.auction.Item, currentBet + priceJump, runner.auction.StartPrice))
                    {
                        int newBetPrice = generateNewBetPrice(currentBet + priceJump);

                        runner.MakeBet(new AgentBet(newBetPrice, this));

                    }
            });
        }
        protected abstract int generateNewBetPrice(int minimunBet);
        public abstract bool DoJoin(IAuctionItem item, int currentBid, int startPrice);
        public void PrintToPersonalScreen(string message)
        {
            Console.ForegroundColor = _consoleColor;
            System.Console.WriteLine($"{Name}: {message}");
            Console.ResetColor();
        }
        private ConsoleColor chooseConsoleColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            ConsoleColor cR;
            do { cR = (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length)); }
            while (cR == ConsoleColor.Black);
            return cR;
        }
        public void BuyProduct(IAuctionItem product, int productPrice)
        {
            Cash -= productPrice;
            _ownedProducts.Add(product);
            PrintToPersonalScreen($"You now have {Cash}$ left");
            string ownProducts = string.Join(", ", (_ownedProducts.Select(p => p.Name).ToList()));
            PrintToPersonalScreen($"Owned prodcts: {ownProducts}");
        }
    }
}
