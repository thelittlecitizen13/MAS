namespace MAS.Items
{
    public interface IBuilding
    {
        public int NumberOfRooms { get; set; }
        public int RoomSize { get; set; }
        public int NumberOfToilets { get; set; }
        public string Address { get; set; }
    }
}
