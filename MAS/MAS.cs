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
        private List<Agent> _listedAgents { get; set; }
        private event Notify _notifyAgents;
        public MAS()
        {
            Auctions = new List<Auction>();
            _listedAgents = new List<Agent>();
        }
        public void Start()
        {
            
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
                StartNextAuction(auction);
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
            _notifyAgents?.Invoke(SB.ToString());
            List<Agent> auctionAgents = new List<Agent>();
            Parallel.ForEach(_listedAgents, (agent) =>
            {
                if (agent.DoJoin(auction.Item))
                    auction.addAgentToAuction(agent);
            });
            
        }
        private void EndAuction(Auction Auction)
        {
            // Change Is Over
        }
        private List<Auction> getNextAuctions()
        {
            List<Auction> nextAuctions = Auctions.Where(auc => auc.IsActive == false && auc.IsOver == false && auc.StartDate < DateTime.Now).ToList();
            return nextAuctions;
        }
        
        public void AddAgent(Agent agent)
        {
            _notifyAgents += agent.PrintToPersonalScreen;
            _listedAgents.Add(agent);
        }
    }
}
