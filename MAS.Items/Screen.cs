using System.Text;

namespace MAS.Items
{
    public class Screen : IAuctionItem
    {
        public string Name { get; set; }
        private string _description { get; set; }
        public int UniqueID { get; set; }
        public Screen(string name, string description, int uniqueID)
        {
            Name = name;
            _description = description;
            UniqueID = uniqueID;
        }
        public string Description()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine($"Name: {Name}");
            SB.AppendLine($"Description: {_description}");
            return SB.ToString();
        }
    }
}
