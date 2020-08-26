using MAS.Agents;
using MAS.Items;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MAS
{
    public class Auction
    {
        public List<Agent> Participants { get; set; }
        public IAuctionItem Item { get; private set; }
        public event Notify NotifyAgents;
        public event AskForBets GetAgentsBets;
        public int StartPrice { get; set; }
        public AuctionBet CurrentBet { get; private set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsOver { get; set; }


        public Auction(IAuctionItem auctionItem, DateTime startDate, int startPrice, int minimunJumpPrice)
        {
            Item = auctionItem;
            Participants = new List<Agent>();
            IsActive = false;
            IsOver = false;
            StartDate = startDate;
            CurrentBet = new AuctionBet(startPrice, minimunJumpPrice, startDate);
        }
        

        
        public void MakeBet(AgentBet bet)
        {
            if (!IsActive && CurrentBet.BetHolder != bet.BettingAgent)
            {
                return;
            }
            if (CurrentBet.UpdateBet(bet))
            {
                NotifyChange($"{bet.BettingAgent.Name} is now leading the auction over {Item.Name} with price tag of {bet.NewPrice}$");
            }
        }
        public void NotifyChange(string message)
        {
            NotifyAgents?.Invoke(message);
        }
        public void AskForNewBets()
        {
            
            //var agentsInvocationLis = GetAgentsBets?.GetInvocationList();
            //Parallel.ForEach(agentsInvocationLis, (agentMetod) =>
            //{
            //    agentMetod.DynamicInvoke($"Would you like to bet on {Item.Name}? Minimun bet: {CurrentBet.CurrentPrice + CurrentBet.MinimunPriceJump}$");
            //});
            GetAgentsBets?.Invoke($"Would you like to bet on {Item.Name}? Minimun bet: {CurrentBet.CurrentPrice + CurrentBet.MinimunPriceJump}$", this);
        }
        public void addAgentToAuction(Agent agent)
        {
            Participants.Add(agent);
            agent.PrintToPersonalScreen($"Welcome to {Item.Name}`s auction!");
            NotifyAgents += agent.PrintToPersonalScreen;
            GetAgentsBets += agent.MakeBet;

        }
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine($"Action for item '{Item.Name}'");
            SB.AppendLine($"Details about the product:");
            SB.AppendLine(Item.ToString());
            return SB.ToString() ;
        }
        public void ShowWinner()
        {
            Agent winner = CurrentBet.BetHolder;
            if (winner == null)
            {
                Console.WriteLine($"No winner for {Item.Name} auction!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{CurrentBet.BetHolder.Name} wins the auction of {Item.Name} for {CurrentBet.CurrentPrice}$ ");
                Console.ResetColor();
                CurrentBet.BetHolder.PrintToPersonalScreen($"Congratulations {CurrentBet.BetHolder.Name}! You won {Item.Name} for {CurrentBet.CurrentPrice}$ ");
            }
        }
    }
}
