using MAS.Items;
using System;

namespace MAS
{
    public class AuctionFactory
    {
        public Auction CreateAuction(IAuctionItem auctionItem, DateTime startDate, int startPrice, int minimunJumpPrice)
        {
            return new Auction(auctionItem, startDate, startPrice, minimunJumpPrice);
        }
    }
}
