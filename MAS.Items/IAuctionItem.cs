namespace MAS.Items
{
    public interface IAuctionItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UniqueID { get; set; }
        public string ToString();
    }

}
