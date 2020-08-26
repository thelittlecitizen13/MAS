using System;
using MAS.Items;

namespace MAS
{
    public class SegevAgent : Agent
    {
        private static object _newBetPriceLocker = new object();

        public SegevAgent(string name, int cash) : base(name, cash)
        {

        }
        public override bool DoJoin(IAuctionItem item, int currentBid, int startPrice)
        {
            Random rand = new Random();
            return (rand.Next(0, 100) < 60) && (currentBid <= Cash*0.6) && (startPrice <= Cash);
        }
        protected override int generateNewBetPrice(int minimunBet)
        {
            lock (_newBetPriceLocker)
            {
                Random rand = new Random();
                return rand.Next(minimunBet, (int)(Cash * 0.6));
            }
        }
    }
}
