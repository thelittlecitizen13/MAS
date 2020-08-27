using MAS.Items;
using System;

namespace MAS
{
    class Program
    {
        static void Main(string[] args)
        {
            MAS MamasAuction = new MAS();
            SegevAgent segevG = new SegevAgent("SegevG Course", 7000);
            SegevAgent segevH = new SegevAgent("SegevH Course", 7000);
            SegevAgent segevI = new SegevAgent("SegevI Course", 7000);
            SegevAgent segevJ = new SegevAgent("SegevJ Course", 7000);
            Screen screen21 = new Screen("HP Elite 21", "21inch 4K Screen", 111);
            Screen screen24 = new Screen("BenQ 24", "24inch 4K Screen", 112);
            Screen screen27 = new Screen("Sony 27", "27inch 4K Screen", 113);
            Screen screen32 = new Screen("Samsung 32", "32inch 4K Screen", 114);
            Screen screen77 = new Screen("LG 77", "77inch 4K Screen", 115);
            Auction screenAuction21 = new Auction(screen21, DateTime.Now.AddSeconds(3), 200, 5);
            Auction screenAuction24 = new Auction(screen24, DateTime.Now.AddSeconds(7), 300, 5);
            Auction screenAuction27 = new Auction(screen27, DateTime.Now.AddSeconds(15),400, 5);
            Auction screenAuction32 = new Auction(screen32, DateTime.Now.AddSeconds(20), 800, 5);
            Auction screenAuction77 = new Auction(screen77, DateTime.Now.AddSeconds(30), 1200, 10);
            MamasAuction.AddAgent(segevH);
            MamasAuction.AddAgent(segevG);
            MamasAuction.AddAgent(segevI);
            MamasAuction.AddAgent(segevJ);
            MamasAuction.Auctions.Add(screenAuction21);
            MamasAuction.Auctions.Add(screenAuction24);
            MamasAuction.Auctions.Add(screenAuction27);
            MamasAuction.Auctions.Add(screenAuction32);
            MamasAuction.Auctions.Add(screenAuction77);
            MamasAuction.Start();
            Console.Read();

        }
    }
    
}
