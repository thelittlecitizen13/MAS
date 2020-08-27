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
            Office AppleTown = new Office("Apple Town", 1000, 50, 2500, true, true, true, 116, "Sillicon Valley, CA, USA");
            Residence AssaHome = new Residence("Assa`s Home", 5, 12, 3, 2, 117, "Maagal HaShalom, Rishon Lezzion", true);
            Screen screen77 = new Screen("LG 77", "77inch 4K Screen", 115);

            MamasAuction.AddAgent(segevH);
            MamasAuction.AddAgent(segevG);
            MamasAuction.AddAgent(segevI);
            MamasAuction.AddAgent(segevJ);
            MamasAuction.CreateAuction(screen21, DateTime.Now.AddSeconds(3), 200, 5);
            MamasAuction.CreateAuction(screen24, DateTime.Now.AddSeconds(7), 300, 5);
            MamasAuction.CreateAuction(AppleTown, DateTime.Now.AddSeconds(20), 1200, 100);
            MamasAuction.CreateAuction(AssaHome, DateTime.Now.AddSeconds(20), 1000, 80);
            MamasAuction.CreateAuction(screen77, DateTime.Now.AddSeconds(30), 600, 10);

            MamasAuction.Start();
            Console.Read();

        }
    }
    
}
