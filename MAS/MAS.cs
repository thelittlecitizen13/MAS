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
        public List<AuctionRunner> AuctionRunners { get; set; }
        private List<Agent> _listedAgents;
        private List<AuctionRunner> _dueToBeginAuctions;
        private AgentNotifier _notifier;
        private System.Timers.Timer _runAuctionsTimer { get; set; }
        public MAS()
        {
            AuctionRunners = new List<AuctionRunner>();
            _listedAgents = new List<Agent>();
            _dueToBeginAuctions = new List<AuctionRunner>();
            _notifier = new AgentNotifier();
        }
        public void Start()
        {
            PrepareAuctions();
            _runAuctionsTimer = SetInterval(RunAuctions, 5000);
            
        }
        public void RunAuctions()
        { 
            updateNextAuctions();
            List<AuctionRunner> nextAuctions = new List<AuctionRunner>(_dueToBeginAuctions);
            Parallel.ForEach(nextAuctions ?? new List<AuctionRunner>(), runner =>
            {
                _dueToBeginAuctions.Remove(runner);
                runner.RunAuction();
            });

            if (AuctionRunners?.Where(runner => runner.auction.IsOver == true).ToList().Count == AuctionRunners.Count)
            {
                string msg = "All auctions are over. See you next Time!";
                _notifier.NotifyChange(msg);
                _runAuctionsTimer.Stop();
            }
        }
        public void PrepareAuctions()
        {
            Parallel.ForEach(AuctionRunners, (runner) =>
            {
                Thread.CurrentThread.IsBackground = false;
                _notifier.NotifyChange(runner.NewAuction());

                foreach (var agent in _listedAgents)
                {

                    if (agent.DoJoin(runner.auction.Item, 0, runner.auction.StartPrice))
                        runner.addAgentToAuction(agent);
                }
            });
        }

        private void updateNextAuctions()
        {
            List<AuctionRunner> nextAuctions = AuctionRunners.Where(runner => runner.auction.IsActive == false && runner.auction.IsOver == false &&
            runner.auction.StartDate < DateTime.Now && !_dueToBeginAuctions.Contains(runner)).ToList();
            foreach (var runner in nextAuctions)
            {
                _dueToBeginAuctions.Add(runner);
            }
        }

        public void AddAgent(Agent agent)
        {
            _notifier.AddAgentToNotifyList(agent);
            _listedAgents.Add(agent);
        }
        public void CreateAuction(IAuctionItem auctionItem, DateTime startDate, int startPrice, int minimunJumpPrice)
        {
            AuctionRunners.Add(new AuctionRunner(auctionItem, startDate, startPrice, minimunJumpPrice));
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
