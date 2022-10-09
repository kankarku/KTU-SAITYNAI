namespace Hotelio.Data
{
    public class AdditionalService : BaseModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Room Room { get; set; }
    }
}
