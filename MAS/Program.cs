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
            Screen screen21 = new Screen("HP Elite", "21inch 4K Screen", 111);
            Screen screen24 = new Screen("BenQ", "24inch 4K Screen", 112);
            Auction screenAuction21 = new Auction(screen21, DateTime.Now.AddSeconds(3), 1000, 5);
            //Auction screenAuction24 = new Auction(screen24, DateTime.Now.AddSeconds(7), 1, 1);
            MamasAuction.AddAgent(segevH);
            MamasAuction.AddAgent(segevG);
            MamasAuction.Auctions.Add(screenAuction21);
            //MamasAuction.Auctions.Add(screenAuction24);
            MamasAuction.Start();
            Console.Read();

        }
    }
    
}
