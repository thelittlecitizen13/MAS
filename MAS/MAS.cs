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
        private List<Agent> _listedAgents;
        private List<Auction> _dueToBeginAuctions;
        private event Notify _notifyAgents;
        public MAS()
        {
            Auctions = new List<Auction>();
            _listedAgents = new List<Agent>();
            _dueToBeginAuctions = new List<Auction>();
        }
        public void Start()
        {
            
            updateNextAuctions();
            while (_dueToBeginAuctions != null && _dueToBeginAuctions.Count != 0)
            {
                RunAuctions(_dueToBeginAuctions);
                Thread.Sleep(10000);
                updateNextAuctions();
            }
        }
        public void RunAuctions(List<Auction> nextAuctions)
        {
            Parallel.ForEach(nextAuctions, auction =>
            {
                if (StartNextAuction(auction))
                {
                    _dueToBeginAuctions.Remove(auction);
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
                }
                auction.ShowWinner();
            });
        }
        private bool StartNextAuction(Auction auction)
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
            if (auction.Participants.Count == 0)
            {
                EndAuction(auction);
                Console.WriteLine($"Auction over {auction.Item.Name}, UID {auction.Item.UniqueID} is closed due to no participants.");
                return false;
            }
            auction.IsActive = true;
            return true;
            
        }
        private void EndAuction(Auction auction)
        {
            auction.IsOver = true;
            auction.IsActive = false;
        }
        private void updateNextAuctions()
        {
            List<Auction> nextAuctions = Auctions.Where(auc => auc.IsActive == false && auc.IsOver == false && 
            auc.StartDate < DateTime.Now && !_dueToBeginAuctions.Contains(auc)).ToList();
            foreach (var auc in nextAuctions)
            {
                _dueToBeginAuctions.Add(auc);
            }
        }
        
        public void AddAgent(Agent agent)
        {
            _notifyAgents += agent.PrintToPersonalScreen;
            _listedAgents.Add(agent);
        }
    }
}
