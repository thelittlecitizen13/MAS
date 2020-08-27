using System.Text;

namespace MAS.Items
{
    public class Office : IBuilding, IAuctionItem
    {
        public int NumberOfRooms { get; set; }
        public int RoomSize { get; set; }
        public int NumberOfToilets { get; set; }
        public bool IsPS5RoomAvailable { get; set; }
        public string Name { get; set; }
        public int UniqueID { get; set; }
        public string Address { get; set; }
        public bool Balcony { get; set; }
        public bool DiningRoom { get; set; }

        public Office(string name, int rooms, int roomSize, int toilets, bool isPS5RoomAvailable, bool balcony, bool diningRoom, int UID, string address)
        {
            Name = name;
            NumberOfRooms = rooms;
            RoomSize = roomSize;
            IsPS5RoomAvailable = isPS5RoomAvailable;
            UniqueID = UID;
            Address = address;
            NumberOfToilets = toilets;
            DiningRoom = diningRoom;
            Balcony = balcony;
        }
        public string Description()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine($"Name: {Name}");
            SB.AppendLine($"Address: {Address}");
            SB.AppendLine($"Number Of Rooms: {NumberOfRooms}");
            SB.AppendLine($"Every Room Size: {RoomSize}");
            string isPS5 = IsPS5RoomAvailable ? "Yes" : "No";
            string isBalcony = Balcony ? "Yes" : "No";
            string isDiningRoom = DiningRoom ? "Yes" : "No";
            SB.AppendLine($"PS5 Room: {isPS5} ");
            SB.AppendLine($"Dining Room: {isDiningRoom} ");
            SB.AppendLine($"Number Of Toilets: {NumberOfToilets}");
            SB.AppendLine($"Balcony: {isBalcony}");
            return SB.ToString();
        }

    }
}
