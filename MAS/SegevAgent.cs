using System;
using MAS.Items;

namespace MAS
{
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

        public override void MakeBet(string message, Auction auction)
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
