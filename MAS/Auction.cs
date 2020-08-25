using MAS.Agents;
using MAS.Items;
using System.Collections.Generic;

namespace MAS
{
    public class Auction
    {
        public List<Agent> Participants { get; set; }
        public IAuctionItem Item { get; private set; }
        public event Notify NotifyAgents;

    }
}
