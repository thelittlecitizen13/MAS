using MAS.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAS
{
    public delegate void Notify(string message);
    public delegate void AskForBets(string message, Auction auction);
}
