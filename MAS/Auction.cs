﻿using MAS.Agents;
using MAS.Items;
using System;
using System.Collections.Generic;
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
        public DateTime StartDate{ get; set; }
        public bool IsActive { get; set; }
        public AuctionBet CurrentBet { get; private set; }
        public void MakeBet(AgentBet bet)
        {
            if (!IsActive)
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
            var agentsInvocationLis = GetAgentsBets.GetInvocationList();
            Parallel.ForEach(agentsInvocationLis, (agentMetod) =>
            {
                agentMetod.DynamicInvoke("bla");
            });
            //GetAgentsBets?.Invoke($"Would you like to bet on {Item.Name}? Minimun bet: {CurrentBet.CurrentPrice + CurrentBet.MinimunPriceJump}$", this);
        }
        public void addAgentToAuction(Agent agent)
        {
            Participants.Add(agent);
            agent.PrintToPersonalScreen($"Welcome to {Item.Name}`s auction!");
            NotifyAgents += agent.PrintToPersonalScreen;

        }
        public override string ToString()
        {
            
        }
    }
}
