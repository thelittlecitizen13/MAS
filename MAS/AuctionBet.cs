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
        public AuctionBet(int currentPrice, int minimunPriceJump, DateTime betTime)
        {
            CurrentPrice = currentPrice;
            MinimunPriceJump = minimunPriceJump;
            BetTime = betTime;
        }
        public bool UpdateBet(AgentBet bet)
        {
           if (bet.NewPrice >= CurrentPrice + MinimunPriceJump)
            {
                CurrentPrice = bet.NewPrice;
                BetHolder = bet.BettingAgent;
                return true;
            }
            return false;
        }
        
    }
}
