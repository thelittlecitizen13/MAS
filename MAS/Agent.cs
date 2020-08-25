using System;
using MAS;
using MAS.Items;

namespace MAS
{
    public abstract class Agent
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        
        public Agent(string name, int cash)
        {
            Name = name;
            Cash = cash;
        }
        public abstract void MakeBet(Auction auction);
        public abstract bool DoJoin(IAuctionItem item);
        public void PrintToPersonalScreen(string message)
        {
            System.Console.WriteLine(message);
        }
    }
    public class SegevAgent : Agent
    {
        public SegevAgent(string name, int cash) : base(name, cash)
        {

        }
        public override bool DoJoin(IAuctionItem item)
        {
            Random rand = new Random();
            return rand.Next(0, 100) < 70;
        }

        public override void MakeBet(Auction auction)
        {
            int currentBet = auction.CurrentBet.CurrentPrice;
            int priceJump = auction.CurrentBet.MinimunPriceJump;
            if (DoJoin(auction.Item) && (Cash >= currentBet + priceJump))
            {
                int newBetPrice = generateNewBetPrice(currentBet + priceJump);
                auction.MakeBet(new AgentBet(newBetPrice, this));
            }
        }
        private int generateNewBetPrice(int minimunBet)
        {
            Random rand = new Random();
            return rand.Next(minimunBet, Cash);
        }
    }
}
