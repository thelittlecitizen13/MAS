using MAS.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MAS
{
    public class AuctionRunner
    {        
        public ConcurrentBag<Agent> Participants {get; set;}
        private static object _lockMakeBet = new object();
        public Auction auction { get; set; }
        private AgentNotifier _notifier;
        private int AuctionStage;

        public AuctionRunner(Auction auc)
        {
            auction = auc;
            Participants = new ConcurrentBag<Agent>();
            _notifier = new AgentNotifier();
            AuctionStage = 1;
        }
        public void RunAuction()
        {
            if (Participants.Count == 0)
            {
                EndAuction();
                Console.WriteLine($"Auction over {auction.Item.Name}, UID {auction.Item.UniqueID} is closed due to no participants.");
                return;
            }
            _notifier.NotifyChange($"Auction for {auction.Item.Name} started!");
            auction.IsActive = true;
            Thread.CurrentThread.IsBackground = false;             
            while (auction.IsOver == false)
            {
                DateTime lastBetTime = auction.CurrentBet.BetTime.Value;

                if (lastBetTime.AddSeconds(5) < DateTime.Now && AuctionStage != 4)
                {
                    AuctionStage = 4;
                    _notifier.NotifyChange($"Last call for {auction.Item.Name} auction! Less then 3 seconds remaining for ");
                }
                AskForNewBets();
                Thread.Sleep(1000);
                if (lastBetTime.AddSeconds(5) > DateTime.Now)
                {
                    AuctionStage = 3;
                }
                if (lastBetTime.AddSeconds(8) < DateTime.Now && AuctionStage == 4)
                {
                    EndAuction();
                }
            }
        }
        public void addAgentToAuction(Agent agent)
        {
            Participants.Add(agent);
            agent.PrintToPersonalScreen($"Welcome to {auction.Item.Name}`s auction!");
            _notifier.AddAgentToNotifyList(agent);
            _notifier.AddAgentToBetList(agent);

        }
        
        public void MakeBet(AgentBet bet)
        {
            lock (_lockMakeBet)
            {
                if (!auction.IsActive)
                {
                    return;
                }
                else
                {
                    if (auction.CurrentBet.UpdateBet(bet))
                    {
                        _notifier.NotifyChange($"{bet.BettingAgent.Name} is now leading the auction over {auction.Item.Name} with price tag of {bet.NewPrice}$");
                    }
                }
            }
        }
        public void AskForNewBets()
        { 
            //var agentsInvocationLis = GetAgentsBets?.GetInvocationList();
            //Parallel.ForEach(agentsInvocationLis, (agentMetod) =>
            //{
            //    agentMetod.DynamicInvoke($"Would you like to bet on {Item.Name}? Minimun bet: {CurrentBet.CurrentPrice + CurrentBet.MinimunPriceJump}$");
            //});
            string msg = $"Would you like to bet on {auction.Item.Name}? Minimun bet: {auction.CurrentBet.CurrentPrice + auction.CurrentBet.MinimunPriceJump}$";
            _notifier.GetBets(msg, this);
        }
        public void ShowWinner()
        {
            Agent winner = auction.CurrentBet.BetHolder;
            if (winner is null)
            {
                Console.WriteLine($"No winner for {auction.Item.Name} auction!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{auction.CurrentBet.BetHolder.Name} wins the auction of {auction.Item.Name} for {auction.CurrentBet.CurrentPrice}$ ");
                Console.ResetColor();
                auction.CurrentBet.BetHolder.PrintToPersonalScreen($"Congratulations {auction.CurrentBet.BetHolder.Name}! You won {auction.Item.Name} for {auction.CurrentBet.CurrentPrice}$ ");

            }
        }

        public void EndAuction()
        {
            _notifier.NotifyChange($"Auction for {auction.Item.Name} is over!");
            AuctionStage = 5;
            auction.IsOver = true;
            auction.IsActive = false;
            if (auction.CurrentBet.BetHolder == null)
            {
                return;
            }
            auction.CurrentBet.BetHolder.BuyProduct(auction.Item, auction.CurrentBet.CurrentPrice);
            ShowWinner();

        }
        public string NewAuction()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("New Action!!");
            SB.AppendLine(auction.ToString());
            SB.AppendLine($"Would you like to join?");
            return SB.ToString();
        }
    }   
}
