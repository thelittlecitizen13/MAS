using System;

namespace MAS
{
    public class AuctionBet
    {
        public Agent BetHolder { get; set; }
        public DateTime? BetTime { get; set; }
        public int CurrentPrice { get; set; }
        public int MinimunPriceJump { get; set; }
        private static object _lockUpdateBet = new object();
        public AuctionBet(int currentPrice, int minimunPriceJump, DateTime betTime)
        {
            CurrentPrice = currentPrice;
            MinimunPriceJump = minimunPriceJump;
            BetTime = betTime;
        }
        public bool UpdateBet(AgentBet bet)
        {
            lock (_lockUpdateBet)
            {
                if ((bet.NewPrice >= CurrentPrice + MinimunPriceJump))
                {
                    if (BetHolder == null)
                    {
                        CurrentPrice = bet.NewPrice;
                        BetHolder = bet.BettingAgent;
                        return true;
                    }
                    else
                    {
                        if (bet.BettingAgent.Name != BetHolder.Name)
                        {
                            CurrentPrice = bet.NewPrice;
                            BetHolder = bet.BettingAgent;
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        
    }
}
