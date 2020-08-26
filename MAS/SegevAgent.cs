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
        protected override int generateNewBetPrice(int minimunBet)
        {
            Random rand = new Random();
            return rand.Next(minimunBet, Cash);
        }
    }
}
