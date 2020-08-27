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
        private System.Timers.Timer _runAuctionsTimer { get; set; }
        public MAS()
        {
            Auctions = new List<Auction>();
            _listedAgents = new List<Agent>();
            _dueToBeginAuctions = new List<Auction>();
        }
        public void Start()
        { 
            _runAuctionsTimer = SetInterval(RunAuctions, 5000);
        }
        public void RunAuctions()
        {
            updateNextAuctions();
            List<Auction> nextAuctions = new List<Auction>(_dueToBeginAuctions);
            Parallel.ForEach(nextAuctions ?? new List<Auction>(), auction =>
            {
                Thread.CurrentThread.IsBackground = false;
                if (StartNextAuction(auction))
                {
                    _dueToBeginAuctions.Remove(auction);
                    while (auction.IsOver == false)
                    {
                        DateTime lastBetTime = auction.CurrentBet.BetTime.Value;
                        
                        if (lastBetTime.AddSeconds(5) < DateTime.Now && auction.AuctionStage != 4)
                        {
                            auction.AuctionStage = 4;
                            auction.NotifyChange($"Last call for {auction.Item.Name} auction! Less then 2 seconds remaining for ");
                        }
                        auction.AskForNewBets();
                        Thread.Sleep(1000);
                        if (lastBetTime.AddSeconds(5) > DateTime.Now)
                        {
                            auction.AuctionStage = 3;
                        }
                        if (lastBetTime.AddSeconds(8) < DateTime.Now && auction.AuctionStage == 4)
                        {
                            auction.EndAuction();
                            
                        }


                    }
                }
                
                
            });
            if (Auctions?.Where(auc => auc.IsOver == true).ToList().Count == Auctions.Count)
            {
                _notifyAgents?.Invoke("All auctions are over. See you next Time!");
                _runAuctionsTimer.Stop();
            }
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
                Thread.CurrentThread.IsBackground = false;
                if (agent.DoJoin(auction.Item, 0, auction.StartPrice))
                    auction.addAgentToAuction(agent);
            });
            

            if (auction.Participants.Count == 0)
            {
                auction.EndAuction();
                Console.WriteLine($"Auction over {auction.Item.Name}, UID {auction.Item.UniqueID} is closed due to no participants.");
                return false;
            }
            auction.IsActive = true;
            return true;
            
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
