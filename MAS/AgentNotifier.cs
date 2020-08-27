using System;
using System.Collections.Generic;
using System.Text;

namespace MAS
{
    public delegate void Notify(string message);
    public delegate void AskForBets(string message, AuctionRunner runner);
    class AgentNotifier
    {
        private event Notify _notifyAgents;
        private event AskForBets _getAgentsBets;
        public AgentNotifier()
        {

        }
        public void NotifyChange(string message)
        {
            _notifyAgents?.Invoke(message);
        }
        public void GetBets (string message, AuctionRunner runner)
        {
            _getAgentsBets?.Invoke(message, runner);
        }
        public void AddAgentToNotifyList(Agent agent)
        {
            _notifyAgents += agent.PrintToPersonalScreen;
        }
        public void AddAgentToBetList(Agent agent)
        {
            _getAgentsBets += agent.MakeBet;
        }

    }
}
