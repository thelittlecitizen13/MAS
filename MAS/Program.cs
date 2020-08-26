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
            MamasAuction.AddAgent(segevH);
            MamasAuction.AddAgent(segevG);
            MamasAuction.Auctions.Add(screenAuction);
            System.Timers.Timer runAuctionsTimer = SetInterval(MamasAuction.Start, 10);
            Console.Read();

        }
        private static System.Timers.Timer SetInterval(Action Act, int Interval)
        {
            System.Timers.Timer tmr = new System.Timers.Timer();
            tmr.Elapsed += (sender, args) => Act();
            tmr.AutoReset = true;
            tmr.Interval = Interval;
            tmr.Start();

            return tmr;
        }
    }
    
}
