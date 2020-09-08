using MAS.Items;
using System;

namespace MAS
{
    public class AuctionRunnerFactory
    {
        private AuctionFactory _auctionFactory;
        public AuctionRunnerFactory()
        {
            _auctionFactory = new AuctionFactory();
        }
        public AuctionRunner CreateAuctionRunner(IAuctionItem auctionItem, DateTime startDate, int startPrice, int minimunJumpPrice)
        {
            Auction auc = _auctionFactory.CreateAuction(auctionItem, startDate, startPrice, minimunJumpPrice);
            return new AuctionRunner(auc);
        }
    }
}
