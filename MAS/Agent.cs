using MAS;
using MAS.Items;

namespace MAS
{
    public abstract class Agent
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        
        public Agent(string name, int cash)
        {
            Name = name;
            Cash = cash;
        }
        public abstract void MakeBet(string message, Auction auction);
        public abstract bool DoJoin(IAuctionItem item);
        public void PrintToPersonalScreen(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
