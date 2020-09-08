using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MAS
{
    public class AgentFactory
    {
        public Agent CreateAgent(string type, string name, int cash)
        {
            switch(type)
            {
                case "SegevAgent":
                    return new SegevAgent(name, cash);
                default:
                    return null;
            }
        }
    }
}
