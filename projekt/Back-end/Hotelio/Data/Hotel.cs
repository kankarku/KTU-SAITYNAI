namespace Hotelio.Data
{
    public class Hotel : BaseModel
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms;
    }
}
