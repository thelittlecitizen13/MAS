using MAS.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MAS
{
    public class Auction
    {
        public ConcurrentBag<Agent> Participants { get; set; }
        public IAuctionItem Item { get; private set; }

        public int StartPrice { get; set; }
        public AuctionBet CurrentBet { get; private set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsOver { get; set; }
        public int AuctionStage { get; set; }
        

        public Auction(IAuctionItem auctionItem, DateTime startDate, int startPrice, int minimunJumpPrice)
        {
            Item = auctionItem;
            Participants = new ConcurrentBag<Agent>();
            IsActive = false;
            IsOver = false;
            StartDate = startDate;
            AuctionStage = 1;
            CurrentBet = new AuctionBet(startPrice, minimunJumpPrice, startDate);
        }
        
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine($"Action for item '{Item.Name}'");
            SB.AppendLine($"Details about the product:");
            SB.AppendLine(Item.Description());
            return SB.ToString() ;
        }

    }
}
