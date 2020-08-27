using System.Text;

namespace MAS.Items
{
    public class Residence : IBuilding, IAuctionItem
    {
        public int NumberOfRooms { get; set; }
        public int RoomSize { get; set; }
        public int NumberOfToilets { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Name { get; set; }
        public int UniqueID { get; set; }
        public string Address { get; set; }
        public bool Balcony { get; set; }

        public Residence(string name, int rooms, int roomSize, int toilets, int bathrooms, int UID, string address, bool balcony)
        {
            Name = name;
            NumberOfRooms = rooms;
            RoomSize = roomSize;
            UniqueID = UID;
            Address = address;
            NumberOfToilets = toilets;
            NumberOfBathrooms = bathrooms;
            Balcony = balcony;
        }
        public string Description()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine($"Name: {Name}");
            SB.AppendLine($"Address: {Address}");
            SB.AppendLine($"Number Of Rooms: {NumberOfRooms}");
            SB.AppendLine($"Every Room Size: {RoomSize}");
            string isBalcony = Balcony ? "Yes" : "No";
            SB.AppendLine($"Number Of Toilets: {NumberOfToilets}");
            SB.AppendLine($"Number Of Bathrooms: {NumberOfBathrooms}");
            SB.AppendLine($"Balcony: {isBalcony}");
            return SB.ToString();
        }

    }
}
