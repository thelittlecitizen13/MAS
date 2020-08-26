namespace MAS.Items
{
    public class Screen : IAuctionItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UniqueID { get; set; }
        public Screen(string name, string description, int uniqueID)
        {
            Name = name;
            Description = description;
            UniqueID = uniqueID;
        }

    }
}
