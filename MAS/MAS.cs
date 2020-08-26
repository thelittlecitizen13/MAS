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
                if (StartNextAuction(auction))
                {
                    _dueToBeginAuctions.Remove(auction);
                    Stopwatch auctionStopwatch = new Stopwatch();
                    auctionStopwatch.Start();
                    while (auctionStopwatch.ElapsedMilliseconds < 10000)
                    {
                        DateTime? currentBetTime = auction.CurrentBet.BetTime;
                        Thread.CurrentThread.IsBackground = false;
                        if (auctionStopwatch.ElapsedMilliseconds > 8000)
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
                EndAuction(auction);
                
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
            if (auction.CurrentBet.BetHolder == null)
            {
                return;
            }
            auction.CurrentBet.BetHolder.BuyProduct(auction.Item, auction.CurrentBet.CurrentPrice);
            auction.ShowWinner();

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
