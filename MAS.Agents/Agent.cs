namespace MAS.Agents
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
        public void MakeBet()
        {

        }
        public bool DoJoin()
        {
            return true;
        }
        public void PrintToPersonalScreen(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
