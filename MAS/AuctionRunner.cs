using System;
using System.Text;
using System.Threading;

namespace MAS
{
    public class AuctionRunner
    {
        public event Notify NotifyAgents;
        public event AskForBets GetAgentsBets;
        private static object _lockMakeBet = new object();
        public Auction auction { get; set; }

        public AuctionRunner()
        {

        }
        public void RunAuction()
        {
            if (auction.Participants.Count == 0)
            {
                EndAuction();
                Console.WriteLine($"Auction over {auction.Item.Name}, UID {auction.Item.UniqueID} is closed due to no participants.");
                return;
            }
            auction.IsActive = true;
            Thread.CurrentThread.IsBackground = false;             
            while (auction.IsOver == false)
            {
                DateTime lastBetTime = auction.CurrentBet.BetTime.Value;

                if (lastBetTime.AddSeconds(5) < DateTime.Now && auction.AuctionStage != 4)
                {
                    auction.AuctionStage = 4;
                    NotifyChange($"Last call for {auction.Item.Name} auction! Less then 3 seconds remaining for ");
                }
                AskForNewBets();
                Thread.Sleep(1000);
                if (lastBetTime.AddSeconds(5) > DateTime.Now)
                {
                    auction.AuctionStage = 3;
                }
                if (lastBetTime.AddSeconds(8) < DateTime.Now && auction.AuctionStage == 4)
                {
                    EndAuction();
                }
            }
        }
        public void addAgentToAuction(Agent agent)
        {
            auction.Participants.Add(agent);
            agent.PrintToPersonalScreen($"Welcome to {auction.Item.Name}`s auction!");
            NotifyAgents += agent.PrintToPersonalScreen;
            GetAgentsBets += agent.MakeBet;

        }
        public void NotifyChange(string message)
        {
            NotifyAgents?.Invoke(message);
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
                        NotifyChange($"{bet.BettingAgent.Name} is now leading the auction over {auction.Item.Name} with price tag of {bet.NewPrice}$");
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
            GetAgentsBets?.Invoke($"Would you like to bet on {auction.Item.Name}? Minimun bet: {auction.CurrentBet.CurrentPrice + auction.CurrentBet.MinimunPriceJump}$", auction);
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
            auction.AuctionStage = 5;
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
