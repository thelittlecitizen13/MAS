using MAS.Agents;
using MAS.Items;
using System;
using System.Collections.Generic;

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
    }
}
