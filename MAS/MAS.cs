using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MAS
{
    class MAS
    {
        public List<Auction> Auctions { get; set; }
        public List<Agent> ListedAgents { get; set; }
        public event Notify NotifyAgents;
        public void RunAuctions()
        {

        }
        private void StartNextAuction(Auction auction)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("New Action!!");
            SB.AppendLine(auction.ToString());
            SB.AppendLine($"Would you like to join?");
            NotifyAgents?.Invoke($"");
            List<Agent> auctionAgents = new List<Agent>();
            Parallel.ForEach(ListedAgents, (agent) =>
            {
                if (agent.DoJoin(auction.Item))
                    auction.addAgentToAuction(agent);
            });
        }
        
    }
}
