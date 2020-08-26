using MAS.Items;
using System;

namespace MAS
{
    class Program
    {
        static void Main(string[] args)
        {
            MAS MamasAuction = new MAS();
            SegevAgent segevG = new SegevAgent("SegevG Course", 5000);
            SegevAgent segevH = new SegevAgent("SegevH Course", 5000);
            Screen screen = new Screen("HP Elite", "21inch 4K Screen", 111);
            Auction screenAuction = new Auction(screen, DateTime.Now.AddSeconds(10), 1000, 100);
            MamasAuction.ListedAgents.Add(segevH);
            MamasAuction.ListedAgents.Add(segevG);
            MamasAuction.Auctions.Add(screenAuction);

        }
    }
    
}
