namespace MAS
{
    public class AgentBet
    {
        public int NewPrice { get; set; }
        public Agent BettingAgent{ get; set; }
        public AgentBet(int newPrice, Agent agent)
        {
            NewPrice = newPrice;
            BettingAgent = agent;
        }
    }
}
