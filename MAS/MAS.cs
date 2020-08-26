using MAS.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MAS
{
    class MAS
    {
        public List<Auction> Auctions { get; set; }
        public List<Agent> ListedAgents { get; set; }
        public event Notify NotifyAgents;
        public void Start()
        {
            //System.Timers.Timer runAuctionsTimer = SetInterval(RunAuctions, 10);
            List<Auction> yetStartedAuctions = getNextAuctions();
            while (yetStartedAuctions != null && yetStartedAuctions.Count != 0)
            {
                RunAuctions(yetStartedAuctions);
                Thread.Sleep(10000);
                yetStartedAuctions = getNextAuctions();
            }
        }
        public void RunAuctions(List<Auction> nextAuctions)
        {
            Parallel.ForEach(nextAuctions, auction =>
            {
                Stopwatch auctionStopwatch = new Stopwatch();
                auctionStopwatch.Start();
                while (auctionStopwatch.ElapsedMilliseconds < 10000)
                {
                    DateTime? currentBetTime = auction.CurrentBet.BetTime;
                    
                    if (auctionStopwatch.ElapsedMilliseconds < 2000)
                        auction.NotifyChange("Less then 2 seconds remaining for the auction!");
                    auction.AskForNewBets();
                    Thread.Sleep(500); // Let the agents make bets
                    DateTime? newBetTime = auction.CurrentBet.BetTime;
                    if (newBetTime != currentBetTime)
                    {
                        auctionStopwatch.Reset();
                        auctionStopwatch.Start();
                    }
                }
                auction.ShowWinner();
            });
        }
        private void StartNextAuction(Auction auction)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("New Action!!");
            SB.AppendLine(auction.ToString());
            SB.AppendLine($"Would you like to join?");
            NotifyAgents?.Invoke(SB.ToString());
            List<Agent> auctionAgents = new List<Agent>();
            Parallel.ForEach(ListedAgents, (agent) =>
            {
                if (agent.DoJoin(auction.Item))
                    auction.addAgentToAuction(agent);
            });
        }
        private List<Auction> getNextAuctions()
        {
            List<Auction> nextAuctions = Auctions.Where(auc => auc.IsActive == false && auc.IsOver == false && auc.StartDate < DateTime.Now).ToList();
            return nextAuctions;
        }
        private static System.Timers.Timer SetInterval(Action Act, int Interval)
        {
            System.Timers.Timer tmr = new System.Timers.Timer();
            tmr.Elapsed += (sender, args) => Act();
            tmr.AutoReset = true;
            tmr.Interval = Interval;
            tmr.Start();

            return tmr;
        }

    }
}
