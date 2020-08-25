using MAS.Agents;
using System;

namespace MAS
{
    public class AuctionBet
    {
        public Agent BetHolder { get; set; }
        public DateTime? BetTime { get; set; }
        public int CurrentPrice { get; set; }
        public int MinimunPriceJump { get; set; }
        public AuctionBet(int currentPrice, int minimunPriceJump)
        {
            CurrentPrice = currentPrice;
            MinimunPriceJump = minimunPriceJump;
        }
        public bool UpdateBet(int newPrice, Agent bettingAgent)
        {
           if (newPrice >= CurrentPrice + MinimunPriceJump)
            {
                CurrentPrice = newPrice;
                BetHolder = bettingAgent;
                return true;
            }
            return false;
        }
        
    }
}
